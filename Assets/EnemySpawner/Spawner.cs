using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
   [Serializable]
    private class Entry
    {
        public EnemyType Enemy;
        public int Amount = 0;
        public float Period = 1;
    }

    [field: SerializeField] public EnemyFactory EnemyFactory { get; private set; }
    [SerializeField] private Entry _entry;
    [SerializeField] private float _spawnTime;
    private readonly Dictionary<Enemy,IValueEntry<Enemy>> _activeEnemies = new();

    private void OnEnable()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            if (EnemyFactory.TryGet(_entry.Enemy,gameObject.transform.position,quaternion.identity, out var enemy))
            {
                _activeEnemies.Add(enemy.Value,enemy);
                enemy.Value.OnDeath += OnEnemyDeath;
                enemy.Value.GetComponent<EnemySteering>().StartFollowPath();
            }
            yield return new WaitForSeconds(_spawnTime);
        }
    }

    private void OnEnemyDeath(Enemy enemy)
    {
        enemy.OnDeath -= OnEnemyDeath;
        
        if (_activeEnemies.Remove(enemy, out var handle))
        {
            handle.Release();
            handle.Value.transform.position = gameObject.transform.position;
        }
    }
}
