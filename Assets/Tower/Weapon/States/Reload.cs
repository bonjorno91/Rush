using UnityEngine;

public sealed class Reload : IWeaponState<float>
{
    private readonly IWeaponBrain _weapon;
    private readonly Transform _projectileTransform;
    private float _timer;

    public Reload(IWeaponBrain weapon,Transform projectileTransform)
    {
        _projectileTransform = projectileTransform;
        _weapon = weapon;
    }

    public bool Shoot(Transform target)
    {
        return false;
    }

    #region State

    public void OnEnter(float payload)
    {
        _timer = payload;
    }

    public void Tick()
    {
        if (_timer <= 0) _weapon.EnterState<Ready, ProjectileBehaviour>(GetProjectile());
        else _timer -= Time.deltaTime;
    }

    public void OnExit()
    {
        _timer = 0;
    }

    #endregion

    private ProjectileBehaviour GetProjectile()
    {
        return Object.Instantiate(_weapon.Projectile, _projectileTransform).Initialize();
    }
}