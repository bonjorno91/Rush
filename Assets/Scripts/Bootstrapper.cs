using System;
using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private InitializeOnStartSO[] _initializeSO;
    [SerializeField] private Spawner _enemySpawner;

    private void Awake()
    {
        foreach (var initializeOnStartSo in _initializeSO)
        {
            initializeOnStartSo.Initialize();
        }
    }

    private void Start()
    {
        if (_enemySpawner) _enemySpawner.enabled = true;
    }
}
