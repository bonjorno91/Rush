using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int _hitPointsMax = 5;
    public event Action OnDeath;
    private int _currentHitPoints;

    private void OnEnable()
    {
        _currentHitPoints = _hitPointsMax;
    }
    
    private void OnDead()
    {
        OnDeath?.Invoke();
    }

    public void TakeDamage(int damage)
    {
        _currentHitPoints -= damage;
        
        if (_currentHitPoints <= 0)
        {
            OnDead();
        }
    }
}