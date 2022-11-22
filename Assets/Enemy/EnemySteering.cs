using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySteering : MonoBehaviour
{
    public event Action OnArrived;
    [Range(1, 10)] [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private List<Node> _path = new();
    private Pathfinder _pathfinder;
    private Map _map;

    private void Awake()
    {
        _map = FindObjectOfType<Map>();
        _pathfinder = FindObjectOfType<Pathfinder>();
    }

    public void StartFollowPath()
    {
        _path = _pathfinder.GetNewPath();
        StartCoroutine(FollowPath());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void UpdatePath(Pathfinder pathfinder)
    {
        StopAllCoroutines();
        _path.Clear();
        _path = pathfinder.GetNewPathFor(_map.WorldToCoords(gameObject.transform.position));
        StartCoroutine(FollowPath());
    }

    private IEnumerator FollowPath()
    {
        for (int i = 1; i < _path.Count; i++)
        {
            var travelPercent = 0f;
            var startPosition = gameObject.transform.position;
            var endPosition = _map.CoordsToWorld(_path[i].Coordinates);
            
            gameObject.transform.LookAt(endPosition);

            while (travelPercent < 1f)
            {
                travelPercent += Time.deltaTime * _moveSpeed;
                gameObject.transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
        }

        OnArrived?.Invoke();
    }
}