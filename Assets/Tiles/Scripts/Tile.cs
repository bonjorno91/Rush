using Unity.Mathematics;
using UnityEngine;

[SelectionBase][DefaultExecutionOrder(-75)]
public class Tile : MonoBehaviour
{
    [field: SerializeField] public ReactiveProperty<bool> IsPlaceable { get; private set; } = new(false);
    [SerializeField] private UITowerBuildPanel _buildPanel;
    private IValueEntry<Tower> _content;
    private Map _map;
    private Pathfinder _pathfinder;
    private Vector2Int _coords;

    public void PlaceTower(IValueEntry<Tower> towerType)
    {
        _content = towerType;
        _map.BlockNode(_coords);
        IsPlaceable.Value = !IsPlaceable.Value;
        _pathfinder.UpdatePath();
    }

    private void Awake()
    {
        _map = FindObjectOfType<Map>();
        _pathfinder = FindObjectOfType<Pathfinder>();
        _buildPanel = FindObjectOfType<UITowerBuildPanel>();
        if (_map) _coords = _map.WorldToCoords(transform.position);
        if (!IsPlaceable.Value) _map.BlockNode(_coords);
    }

    private void OnMouseUp()
    {
        if (_content == null)
        {
            if (_map.Grid[_coords].IsWalkable && !_pathfinder.WillBlockPath(_coords))
            {
                _buildPanel.OnBuild(this);
            }
        }
        else
        {
            // TODO: popup content menu
        }
    }
}