using UnityEngine;

public sealed class Reload : IWeaponState<IWeapon>
{
    private float _timer;
    private IWeapon _weapon;
    
    public bool Shoot(Transform target)
    {
        return false;
    }

    #region State

    public void OnEnter(IWeapon payload)
    {
        _weapon = payload;
        _timer = _weapon.CooldownTime;
    }

    public void Tick()
    {
        if (_timer <= 0) _weapon.Reload();
        else _timer -= Time.deltaTime;
    }

    public void OnExit()
    {
        _weapon = null;
        _timer = 0;
    }

    #endregion
}