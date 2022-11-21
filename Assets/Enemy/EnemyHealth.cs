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

    private void OnParticleCollision(GameObject other) => ProcessHit();

    private void ProcessHit()
    {
        if (--_currentHitPoints <= 0)
        {
            OnDead();
        }
    }
}