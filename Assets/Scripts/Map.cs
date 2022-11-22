using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [field: SerializeField] public int SnapSize { get; private set; } = 10;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private bool _showGizmo;
    public Dictionary<Vector2Int, Node> Grid { get; private set; } = new();

    public void BlockNode(Vector2Int coords)
    {
        if (Grid.ContainsKey(coords)) Grid[coords].IsWalkable = false;
    }

    public void ResetNodes()
    {
        foreach (var node in Grid)
        {
            node.Value.Parent = null;
            node.Value.IsExplored = false;
            node.Value.IsPath = false;
        }
    }
    
    public Vector2Int WorldToCoords(Vector3 worldPosition)
    {
        return new(Mathf.RoundToInt(worldPosition.x / SnapSize), Mathf.RoundToInt(worldPosition.z / SnapSize));
    }

    public Vector3 CoordsToWorld(Vector2Int coords)
    {
        return new(coords.x * SnapSize + transform.position.x, transform.position.y, coords.y * SnapSize + transform.position.z);
    }
    
    private void Awake()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Grid.Add(new(x, y), new Node(new(x, y), true));
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_showGizmo)
        {
            foreach (var node in Grid)
            {
                var position = new Vector3(node.Key.x * 10, 0, node.Key.y * 10) + transform.position;
                if (node.Value.IsPath)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(position,1f);
                }
            }
        }
    }
}

[Serializable]
public class Node
{
    public Node Parent;
    public Vector2Int Coordinates;
    public bool IsWalkable;
    public bool IsExplored;
    public bool IsPath;

    public Node(Vector2Int coordinates, bool isWalkable)
    {
        Coordinates = coordinates;
        IsWalkable = isWalkable;
    }
}
