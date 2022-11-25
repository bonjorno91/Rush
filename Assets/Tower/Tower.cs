using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Tower : MonoBehaviour, ITowerBrain
{
    private UIBuildProgressBar _progressBar;
    [field: SerializeField] public int ConstructionCost { get; private set; }
    [field: SerializeField] public float ConstructionTime { get; private set; } = 1f;
    [field: SerializeField] public EnemyFactory EnemyFactory { get; private set; }
    [field: SerializeField] public IWeapon Weapon { get; private set; }

    public void SetBuildProgressBar(UIBuildProgressBar progressBar)
    {
        _progressBar = progressBar;
    }

    #region MonoCallbacks

    private void Awake()
    {
        Weapon = GetComponentInChildren<IWeapon>();
        SetupStateMachine();
    }

    private void OnEnable()
    {
        EnterState<Construct, Transform>(transform);
    }

    private void Update()
    {
        _currentState?.Tick();
    }

    #endregion

    #region StateMachine

    private readonly Dictionary<Type, ITowerState> _states = new();

    private ITowerState _currentState;

    public void EnterState<TState, TPayload>(TPayload payload) where TState : class, ITowerState, IStatePayload<TPayload>
    {
        _currentState?.OnExit();
        if (_states.TryGetValue(typeof(TState), out _currentState))
            if (_currentState is IStatePayload<TPayload> nextState)
                nextState.OnEnter(payload);
    }

    private void SetupStateMachine()
    {
        var constructState = new Construct(this, _progressBar);
        var targetSeekState = new TargetSeek(this, EnemyFactory);
        var targetAcquiredState = new TargetAcquired(this);
        _states.Add(typeof(Construct), constructState);
        _states.Add(typeof(TargetSeek), targetSeekState);
        _states.Add(typeof(TargetAcquired), targetAcquiredState);
    }

    #endregion
}