using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TargetSensor : MonoBehaviour
{
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private Weapon _weapon;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _range = 10;
    private Vector3 _idleTarget;
    private Enemy _target;

    private void Awake()
    {
        _idleTarget = _weapon.transform.forward;
    }

    private void Update()
    {
        if (_target) AimTarget();
        else FindTarget();
    }

    private void FindTarget()
    {
        if (!_target)
            if (_enemyFactory.TryGetAllActive(out var enemies))
            {
                var enemiesByDistance = from enemy in enemies
                    where InRange(enemy.transform)
                    orderby DistanceBetween(gameObject.transform, enemy.transform) descending
                    select enemy;

                if (enemiesByDistance.Any()) TargetAcquired(enemiesByDistance.First());
                else Idle();
            }
    }

    private void Idle()
    {
        var angle = Vector3.Angle(_weapon.transform.forward, _idleTarget);
        
        if (angle > 3)
        {
            var offset = Vector3.Lerp(_weapon.transform.forward, _idleTarget, Time.deltaTime);
            _weapon.transform.LookAt(_weapon.transform.position + offset);
        }
        else
        {
            _idleTarget = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2)).normalized;
        }
    }

    private void TargetAcquired(Enemy enemy)
    {
        _target = enemy;
        _weapon.transform.LookAt(_target.HitPoint);
        _particle.Play();
        _target.OnDeath += TargetLost;
    }

    private void TargetLost(Enemy enemy)
    {
        _particle.Stop(false);
        _idleTarget = _weapon.transform.forward;
        enemy.OnDeath -= TargetLost;
        _target = null;
        FindTarget();
    }

    private void AimTarget()
    {
        if (InRange(_target.transform))
            _weapon.transform.LookAt(_target.HitPoint);
            
        else
            TargetLost(_target);
    }

    private bool InRange(Transform targetTransform)
    {
        return DistanceBetween(gameObject.transform, targetTransform) <= _range;
    }

    private float DistanceBetween(Transform a, Transform b)
    {
        return Mathf.Sqrt(Mathf.Pow(a.position.x - b.position.x, 2) + Mathf.Pow(a.position.z - b.position.z, 2));
    }
}