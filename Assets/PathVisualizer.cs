using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathVisualizer : MonoBehaviour
{
    [SerializeField] private float _yOffset = 0.2f;
    [SerializeField]private Pathfinder _pathfinder;
    private LineRenderer _renderer;
    private void Awake() => _renderer = GetComponent<LineRenderer>();

    private void OnEnable() => _pathfinder.OnPathUpdated += OnPathUpdated;

    private void OnDisable() => _pathfinder.OnPathUpdated -= OnPathUpdated;

    private void OnPathUpdated()
    {
        _renderer.positionCount = _pathfinder.Path.Count;
        
        for (int i = 0; i < _pathfinder.Path.Count; i++)
        {
            var position = _pathfinder.Map.CoordsToWorld(_pathfinder.Path[i].Coordinates);
            position.y += _yOffset;
            _renderer.SetPosition(i,position);
        }
    }
}
