using UnityEngine;

public interface IWeapon : IShoot
{
    float CooldownTime { get; }
    ProjectileBehaviour Projectile { get; }
    bool Aim(Vector3 position);
    bool IsWithinRange(Vector3 enemy);
    float DistanceTo(Vector3 enemyHitPoint);
}