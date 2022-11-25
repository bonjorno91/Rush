using System.Linq;

public sealed class TargetSeek : ITowerState, IStatePayload<IWeapon>
{
    private readonly ITowerBrain _towerBrain;
    private readonly EnemyFactory _enemyFactory;
    private IWeapon _weapon;
    private Enemy _target;
    
    public TargetSeek(ITowerBrain towerBrain, EnemyFactory enemyFactory)
    {
        _enemyFactory = enemyFactory;
        _towerBrain = towerBrain;
    }

    public void OnEnter(IWeapon payload)
    {
        _weapon = payload;
    }

    public void Tick()
    {
        if (_enemyFactory.TryGetAllActive(out var enemies))
        {
            var enemiesInRange = from enemy in enemies 
                where _weapon.IsWithinRange(enemy.HitPoint.position)
                orderby _weapon.DistanceTo(enemy.HitPoint.position) descending 
                select enemy;

            if (enemiesInRange.Any())
            {
                AcquireTarget(enemiesInRange.First());
            }
        }
    }

    private void AcquireTarget(Enemy enemy)
    {
        _towerBrain.EnterState<TargetAcquired,Enemy>(enemy);
    }

    public void OnExit()
    {
        _weapon = null;
    }
}