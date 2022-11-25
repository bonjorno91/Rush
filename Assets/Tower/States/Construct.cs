using System.Collections.Generic;
using UnityEngine;

public sealed class Construct : ITowerState, IStatePayload<Transform>
{
    private readonly ITowerBrain _towerBrain;
    private readonly Queue<Transform> _partsToConstruct;
    private Transform _currentConstructPart;
    private Transform _origin;
    private float _partConstructTime;
    private float _timeLeft;
    private UIBuildProgressBar _progressBar;
    private float _totalProgress;

    public Construct(ITowerBrain towerBrain, UIBuildProgressBar progressBar)
    {
        _progressBar = progressBar;
        _towerBrain = towerBrain;
        _partsToConstruct = new Queue<Transform>();
    }

    public void OnEnter(Transform payload)
    {
        if (!payload)
        {
            Complete();
            return;
        }
        
        _origin = payload;
        _partConstructTime = _towerBrain.ConstructionTime / _origin.childCount;
        _totalProgress = 0;
        FindParts(payload);
        DisableParts(payload);
        _progressBar.Show(payload);
    }

    private void FindParts(Transform origin)
    {
        _partsToConstruct.Clear();
        foreach (Transform child in origin) _partsToConstruct.Enqueue(child);
    }

    private void DisableParts(Transform origin)
    {
        foreach (Transform part in origin) part.gameObject.SetActive(false);
    }

    private void Complete()
    {
        _towerBrain.EnterState<TargetSeek, IWeapon>(_towerBrain.Weapon);
        _progressBar.Hide();
    }
    
    public void Tick()
    {
        var deltaTime = Time.deltaTime;
        
        if (_currentConstructPart == null)
        {
            if (!_partsToConstruct.TryDequeue(out _currentConstructPart)) Complete();
            else
            {
                _timeLeft += deltaTime;
            }
        }
        else
        {
            if (_timeLeft >= _partConstructTime)
            {
                _currentConstructPart.gameObject.SetActive(true);
                _currentConstructPart = null;
                _timeLeft -= _partConstructTime;
            }
            else
            {
                _timeLeft += deltaTime;
                _progressBar.ModifyProgress(deltaTime / _towerBrain.ConstructionTime);
            }
        }
    }

    public void OnExit()
    {
        _origin = null;
        _partConstructTime = 0;
        _partsToConstruct.Clear();
    }
}