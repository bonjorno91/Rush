using UnityEngine;

public class Ready : IWeaponState<IProjectile>
{
    private IProjectile _projectile;
    
    public bool Shoot(Transform target)
    {
        _projectile.PerformShoot(target);
        return true;
    }

    public void Tick()
    {
        
    }

    public void OnEnter(IProjectile payload)
    {
        _projectile = payload;
    }

    public void OnExit()
    {
        _projectile = null;
    }
}