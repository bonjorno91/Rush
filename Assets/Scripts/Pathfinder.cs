using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Map))][DefaultExecutionOrder(-50)]
public class Pathfinder : MonoBehaviour
{
    public event Action OnPathUpdated;
    private static readonly Vector2Int[] Directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};
    [field: SerializeField] public Vector2Int StartingCoords { get; private set; }
    [field: SerializeField] public Vector2Int DestinationCoords { get; private set; }
    private Node _startingNode;
    private Node _destinationNode;
    private Node _currentInspectNode;
    private Map _map;

    private Dictionary<Vector2Int, Node> _grid;

    private Dictionary<Vector2Int, Node> _reached = new();

    private readonly Queue<Node> _frontier = new();

    public List<Node> _path;

    public List<Node> GetNewPath()
    {
        _map.ResetNodes();
        BreathFirstSearch(StartingCoords);
        return BuildPath();
    }

    public List<Node> GetNewPathFor(Vector2Int coords)
    {
        _map.ResetNodes();
        BreathFirstSearch(coords);
        return BuildPath();
    }

    public void UpdatePath()
    {
        OnPathUpdated?.Invoke();
    }

    public bool WillBlockPath(Vector2Int coords)
    {
        if (_grid.ContainsKey(coords))
        {
            var nodeWalkableCached = _grid[coords].IsWalkable;
            _grid[coords].IsWalkable = false;
            var path = GetNewPath();
            _grid[coords].IsWalkable = nodeWalkableCached;

            if (path.Count <= 1)
            {
                _path = GetNewPath();
                return true;
            }
        }

        return false;
    }

    private void Awake()
    {
        _map = GetComponent<Map>();
    }

    private void Start()
    {
        if (_map)
        {
            _grid = _map.Grid;
            _startingNode = _grid[StartingCoords];
            _destinationNode = _grid[DestinationCoords];
            _startingNode.IsWalkable = true;
            _destinationNode.IsWalkable = true;
        }
        else
        {
            Debug.LogAssertion($"{nameof(Pathfinder)} can't find map component!");
        }

        GetNewPath();
    }

    private List<Node> BuildPath()
    {
        var path = new List<Node>();
        var currentNode = _destinationNode;

        while (currentNode.Parent != null)
        {
            currentNode = currentNode.Parent;
            path.Add(currentNode);
            currentNode.IsPath = true;
        }

        path.Reverse();

        return path;
    }

    private void BreathFirstSearch(Vector2Int coords)
    {
        _frontier.Clear();
        _reached.Clear();

        var isSearching = true;

        _frontier.Enqueue(_grid[coords]);
        _reached.Add(coords, _grid[coords]);

        while (_frontier.Count > 0 && isSearching)
        {
            _currentInspectNode = _frontier.Dequeue();
            _currentInspectNode.IsExplored = true;
            ExploreNeighbors();
            isSearching = _currentInspectNode.Coordinates != DestinationCoords;
        }
    }

    private void ExploreNeighbors()
    {
        var neighbours = new List<Node>();

        foreach (var direction in Directions)
        {
            var neighbourCoordinates = _currentInspectNode.Coordinates + direction;

            if (_grid.ContainsKey(neighbourCoordinates))
            {
                neighbours.Add(_grid[neighbourCoordinates]);
            }
        }

        foreach (var neighbour in neighbours)
        {
            if (!_reached.ContainsKey(neighbour.Coordinates) && neighbour.IsWalkable)
            {
                neighbour.Parent = _currentInspectNode;
                _reached.Add(neighbour.Coordinates, neighbour);
                _frontier.Enqueue(neighbour);
            }
        }
    }
}