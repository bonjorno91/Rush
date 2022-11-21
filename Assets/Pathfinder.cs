using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Map))]
public class Pathfinder : MonoBehaviour
{
    [SerializeField] private Vector2Int _startingCoords;
    [SerializeField] private Vector2Int _destinationCoords;
    [SerializeField] private Node _searchNode;
    private Map _map;
    private readonly Vector2Int[] _directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    private Dictionary<Vector2Int, Node> _grid;
    
    private void Awake()
    {
        _map = GetComponent<Map>();
        _grid = _map.Grid;
    }

    private void Start()
    {
        ExploreNeighbors();
    }

    private void ExploreNeighbors()
    {
        var neighbours = new List<Node>();

        foreach (var direction in _directions)
        {
            var neighbourCoordinates = _searchNode.Coordinates + direction;

            if (_grid.ContainsKey(neighbourCoordinates))
            {
                neighbours.Add(_grid[neighbourCoordinates]);
            }
        }
    }
}