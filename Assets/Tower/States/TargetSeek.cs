using System.Linq;
using UnityEngine;

public sealed class TargetSeek : ITowerState, IStatePayload<IWeapon>
{
    private readonly Tower _tower;
    private readonly EnemyFactory _enemyFactory;
    private IWeapon _weapon;
    private Enemy _target;
    private Vector3 _fakeTarget;

    public TargetSeek(Tower tower, EnemyFactory enemyFactory)
    {
        _enemyFactory = enemyFactory;
        _tower = tower;
    }

    public void OnEnter(IWeapon payload)
    {
        _weapon = payload;
        _fakeTarget = GetFakeTarget();
    }

    public void Tick()
    {
        if (TryGetTarget())
        {
            AcquireTarget(_target);
        }
        else
        {
            SearchTarget();
        }
    }

    private void SearchTarget()
    {
        if (_weapon.Aim(_fakeTarget))
        {
            _fakeTarget = GetFakeTarget();
        }
    }

    private Vector3 GetFakeTarget()
    {
        var random = Random.onUnitSphere * 10;
        return _tower.transform.position + new Vector3(random.x, Mathf.Clamp(random.y,1,3), random.z);
    }

    private bool TryGetTarget()
    {
        if (!_enemyFactory.TryGetAllActive(out var enemies)) return false;
        
        var enemiesInRange = from enemy in enemies
            where _weapon.IsWithinRange(enemy.HitPoint.position)
            orderby _weapon.DistanceTo(enemy.HitPoint.position) descending
            select enemy;

        if (enemiesInRange.Any())
        {
            _target = enemiesInRange.First();
            return true;
        }

        return false;
    }

    private void AcquireTarget(Enemy enemy)
    {
        _tower.EnterState<TargetAcquired, Enemy>(enemy);
    }

    public void OnExit()
    {
        _weapon = null;
    }
}