public sealed class TargetAcquired : ITowerState, IStatePayload<Enemy>
{
    private readonly ITowerBrain _tower;
    private Enemy _enemyTarget;

    public TargetAcquired(ITowerBrain tower)
    {
        _tower = tower;
    }

    public void OnEnter(Enemy enemyTarget)
    {
        if (!enemyTarget)
        {
            SearchTarget();
            return;
        }
        
        _enemyTarget = enemyTarget;
        _enemyTarget.OnDeath += OnEnemyTargetLost;
    }

    private void OnEnemyTargetLost(Enemy enemy)
    {
        enemy.OnDeath -= OnEnemyTargetLost;
        SearchTarget();
    }

    private void SearchTarget()
    {
        _tower.EnterState<TargetSeek, IWeapon>(_tower.Weapon);
    }

    public void Tick()
    {
        if (_tower.Weapon.IsWithinRange(_enemyTarget.HitPoint.position))
        {
            if (_tower.Weapon.Aim(_enemyTarget.HitPoint.position))
            {
                _tower.Weapon.Shoot(_enemyTarget.HitPoint.transform);
            }
        }
        else
        {
            OnEnemyTargetLost(_enemyTarget);
        }
    }

    public void OnExit()
    {
        _enemyTarget = null;
    }
}