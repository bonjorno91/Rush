using System;
using UnityEngine;

[RequireComponent(typeof(EnemySteering))]
[RequireComponent(typeof(EnemyHealth))]
[SelectionBase]
public class Enemy : MonoBehaviour
{
    public event Action<Enemy> OnDeath;
    [field: SerializeField] public Transform HitPoint { get; private set; }
    [SerializeField] private BankAccount _account;
    [SerializeField] private int _goldReward = 25;
    [SerializeField] private int _goldPenalty = 25;
    private EnemyHealth _enemyHealth;
    private EnemySteering _enemySteering;
    
    private void Awake()
    {
        _enemySteering = GetComponent<EnemySteering>();
        _enemyHealth = GetComponent<EnemyHealth>();
    }

    private void OnEnable()
    {
        _enemyHealth.OnDeath += OnDead;
        _enemySteering.OnArrived += OnArrive;
    }

    private void OnDisable()
    {
        _enemyHealth.OnDeath -= OnDead;
        _enemySteering.OnArrived -= OnArrive;
    }

    private void OnDead()
    {
        _account.Deposit(_goldReward);
        Reclaim();
    }

    private void OnArrive()
    {
        _account.Withdraw(_goldPenalty);
        Reclaim();
    }

    private void Reclaim()
    {
        OnDeath?.Invoke(this);
    }
}