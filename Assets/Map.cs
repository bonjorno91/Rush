using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private Vector2Int _size;
    public Dictionary<Vector2Int, Node> Grid { get; private set; } = new();

    private void Awake()
    {
        for (int x = 0; x < _size.x; x++)
        {
            for (int y = 0; y < _size.y; y++)
            {
                Grid.Add(new(x,y),new Node(new(x,y),true));
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
