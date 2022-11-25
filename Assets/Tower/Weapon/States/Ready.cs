using UnityEngine;

public class Ready : IWeaponState<ProjectileBehaviour>
{
    private readonly IWeaponBrain _weapon;
    private ProjectileBehaviour _projectile;

    public Ready(IWeaponBrain weapon)
    {
        _weapon = weapon;
    }

    public bool Shoot(Transform target)
    {
        if (_projectile)
        {
            if (_projectile.PerformShoot(target))
            {
                _weapon.EnterState<Reload,float>(_weapon.CooldownTime);
                return true;
            }
        }

        return false;
    }

    public void Tick()
    {
    }

    public void OnEnter(ProjectileBehaviour payload)
    {
        _projectile = payload;
    }

    public void OnExit()
    {
        
    }
}