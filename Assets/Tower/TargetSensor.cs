using System.Linq;
using UnityEngine;

public class TargetSensor : MonoBehaviour
{
    [SerializeField] private EnemyFactory _enemyFactory;
    [SerializeField] private Transform _weapon;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private float _range = 10;
    private Enemy _target;

    private void FindTarget()
    {
        if (!_target)
        {
            if (_enemyFactory.TryGetAllActive(out var active))
            {
                var enemiesByDistance = from aliveEnemy in active
                    where InRange(aliveEnemy.transform)
                    orderby DistanceBetween(gameObject.transform,aliveEnemy.transform) descending
                    select aliveEnemy;

                if (enemiesByDistance.Any()) TargetAcquired( enemiesByDistance.First());
            }
        }
    }

    private void TargetAcquired(Enemy enemy)
    {
        _target = enemy;
        _weapon.LookAt(_target.HitPoint);
        _particle.Play();
        _target.OnDeath += TargetLost;
    }

    private void TargetLost(Enemy enemy)
    {
        _particle.Stop(false);
        _target.OnDeath -= TargetLost;
        _target = null;
        FindTarget();
    }

    private void Update()
    {
        if (_target) AimTarget();
        else FindTarget();
    }

    private void AimTarget()
    {
        if (InRange(_target.transform))
        {
            _weapon.LookAt(_target.HitPoint);
        }
        else
        {
            TargetLost(_target);
        }
    }

    private bool InRange(Transform targetTransform)
    {
        return DistanceBetween(gameObject.transform,targetTransform) <= _range;
    }

    private float DistanceBetween(Transform a, Transform b) => Mathf.Sqrt(Mathf.Pow(a.position.x - b.position.x, 2) + Mathf.Pow(a.position.z - b.position.z, 2));
}