using System;
using UnityEngine;

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