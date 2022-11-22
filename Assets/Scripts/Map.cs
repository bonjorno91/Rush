using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class Map : MonoBehaviour
{
    [field: SerializeField] public int SnapSize { get; private set; } = 10;
    public Dictionary<Vector2Int, Node> Grid { get; private set; } = new();
    [SerializeField] private Vector2Int _size;
#if UNITY_EDITOR
    [SerializeField] private bool _showGizmo;
#endif

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
        return new(coords.x * SnapSize, transform.position.y, coords.y * SnapSize);
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

#if UNITY_EDITOR
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
                    Gizmos.DrawSphere(position, 1f);
                }

                if (node.Value.IsExplored)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(position, 1f);
                }
            }
        }
    }
#endif
}