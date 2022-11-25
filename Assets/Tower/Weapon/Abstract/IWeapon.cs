using UnityEngine;

public interface IWeapon : IShoot
{
    float CooldownTime { get; }
    bool Aim(Vector3 position);
    void Reload();
    bool IsWithinRange(Vector3 enemyHitPoint);
    float DistanceTo(Vector3 enemyHitPoint);
}