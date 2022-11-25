using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Map))]
[DefaultExecutionOrder(-50)]
public class Pathfinder : MonoBehaviour
{
    private static readonly Vector2Int[] Directions = {Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down};

    public List<Node> Path { get; private set; }

    private readonly Queue<Node> _frontier = new();
    private Node _currentInspectNode;
    private Node _destinationNode;

    private Dictionary<Vector2Int, Node> _grid;
    public Map Map { get; private set; }

    private readonly Dictionary<Vector2Int, Node> _reached = new();
    private Node _startingNode;
    [field: SerializeField] public Vector2Int StartingCoords { get; private set; }
    [field: SerializeField] public Vector2Int DestinationCoords { get; private set; }

    private void Awake()
    {
        Map = GetComponent<Map>();
    }

    private void Start()
    {
        if (Map)
        {
            _grid = Map.Grid;
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

    public event Action OnPathUpdated;

    public List<Node> GetNewPath()
    {
        Map.ResetNodes();
        BreathFirstSearch(StartingCoords);
        Path = BuildPath();
        return Path;
    }

    public List<Node> GetNewPathFor(Vector2Int coords)
    {
        Map.ResetNodes();
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
                Path = GetNewPath();
                return true;
            }
        }

        return false;
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

            if (_grid.ContainsKey(neighbourCoordinates)) neighbours.Add(_grid[neighbourCoordinates]);
        }

        foreach (var neighbour in neighbours)
            if (!_reached.ContainsKey(neighbour.Coordinates) && neighbour.IsWalkable)
            {
                neighbour.Parent = _currentInspectNode;
                _reached.Add(neighbour.Coordinates, neighbour);
                _frontier.Enqueue(neighbour);
            }
    }
}