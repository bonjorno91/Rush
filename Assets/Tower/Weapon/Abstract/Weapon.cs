using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public abstract class Weapon : MonoBehaviour, IWeapon
{
    private AudioSource _audioSource;

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
    public Projectile Projectile { get; protected set; }

    [field: SerializeField] protected Projectile.Config _projectileConfig;

    [field: Header("Sound Setting")]
    [field: SerializeField] 
    public AudioClip ReleaseClip { get; protected set; }
    
    public bool Aim(Vector3 targetPosition)
    {
        var offset = Vector3.Lerp(transform.position + transform.forward, targetPosition, AimSpeed * Time.deltaTime);
        transform.LookAt(offset);

        var targetAngle = Vector3.Angle(transform.forward, targetPosition - transform.position);

        return targetAngle <= AngleAccuracy;
    }

    public bool IsWithinRange(Vector3 enemyHitPoint)
    {
        return transform.position.DistanceXZ(enemyHitPoint) <= Range;
    }

    public float DistanceTo(Vector3 enemyHitPoint)
    {
        return transform.position.DistanceXZ(enemyHitPoint);
    }

    protected void PlayReleaseSound()
    {
        _audioSource.PlayOneShot(ReleaseClip);
    }

    public abstract void Reload();

    public abstract bool Shoot(Transform target);

    #region MonoCallbacks

    protected void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    #endregion
}