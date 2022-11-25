using UnityEngine;

[SelectionBase][DefaultExecutionOrder(-75)]
public class Obstacle : MonoBehaviour
{
    private void Awake()
    {
        var map = FindObjectOfType<Map>();
        if(map) map.BlockNode(map.WorldToCoords(transform.position));
    }
}
