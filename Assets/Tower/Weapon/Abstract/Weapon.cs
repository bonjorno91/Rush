using UnityEngine;

public abstract class Weapon : MonoBehaviour, IWeapon
{
    [field: Header("Weapon Settings")]
    [field: SerializeField]
    [field: Min(1)]
    public float Range { get; protected set; } = 10f;

    [field: SerializeField]
    [field: Range(2, 45)]
    public float AngleAccuracy { get; protected set; } = 3f;

    [field: SerializeField]
    [field: Range(0, 4)]
    public float AimSpeed { get; protected set; } = 1f;

    [field: SerializeField]
    [field: Range(0.1f, 10)]
    public float CooldownTime { get; protected set; } = 1f;

    [field: Header("Projectile Settings")]
    [field: SerializeField]
    [field: Min(1)]
    public int Damage { get; protected set; } = 1;

    [field: SerializeField]
    [field: Min(0)]
    public float ProjectileSpeed { get; protected set; } = 30;

    [field: SerializeField] 
    public ProjectileBehaviour Projectile { get; protected set; }

    public bool Aim(Vector3 targetPosition)
    {
        var offset = Vector3.Lerp(transform.position + transform.forward, targetPosition, AimSpeed * Time.deltaTime);
        transform.LookAt(offset);

        var targetAngle = Vector3.Angle(transform.forward, targetPosition - transform.position);

        return targetAngle <= AngleAccuracy;
    }

    public bool IsWithinRange(Vector3 enemy)
    {
        return transform.position.DistanceXZ(enemy) <= Range;
    }

    public float DistanceTo(Vector3 enemyHitPoint)
    {
        return transform.position.DistanceXZ(enemyHitPoint);
    }

    public abstract bool Shoot(Transform target);
}