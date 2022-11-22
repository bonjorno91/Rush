using Unity.Mathematics;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour
{
    [field: SerializeField] public ReactiveProperty<bool> IsPlaceable { get; private set; } = new(false);
    [SerializeField] private TowerFactory _towerFactory;
    [SerializeField] private TowerType _spawnTower;
    private IValueEntry<Tower> _content;
    private Map _map;
    private Pathfinder _pathfinder;
    private Vector2Int _coords;

    private void Awake()
    {
        _map = FindObjectOfType<Map>();
        _pathfinder = FindObjectOfType<Pathfinder>();
        if (_map) _coords = _map.WorldToCoords(transform.position);
    }

    private void OnEnable()
    {
        if (_map)
        {
            if (!IsPlaceable.Value) _map.BlockNode(_coords);
        }
    }

    private void OnMouseDown()
    {
        if (_map.Grid[_coords].IsWalkable && !_pathfinder.WillBlockPath(_coords))
        {
            if (_towerFactory.TryGet(_spawnTower, gameObject.transform.position, quaternion.identity, out _content))
            {
                IsPlaceable.Value = !IsPlaceable.Value;
                _map.BlockNode(_coords);
                _pathfinder.UpdatePath();
            }
        }
    }
}