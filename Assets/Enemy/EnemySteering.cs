using System;
using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

public class EnemySteering : MonoBehaviour
{
    public event Action OnArrived;
    [Range(1, 10)] [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private List<Tile> _path = new();

    public void StartFollowPath()
    {
        FindPath();
        StartCoroutine(FollowPath());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void FindPath()
    {
        _path.Clear();

        var tiles = GameObject.FindGameObjectWithTag("Path");

        foreach (Transform child in tiles.transform)
        {
            _path.Add(child.GetComponent<Tile>());
        }
    }

    private IEnumerator FollowPath()
    {
        foreach (var tile in _path)
        {
            var travelPercent = 0f;
            var startPosition = gameObject.transform.position;
            var endPosition = tile.gameObject.transform.position;
            gameObject.transform.LookAt(tile.transform);

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