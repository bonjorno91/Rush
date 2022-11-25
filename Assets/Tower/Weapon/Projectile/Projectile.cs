using System;
using Unity.Mathematics;
using UnityEngine;

[SelectionBase]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour, IProjectile
{
    private Collider _collider;
    private Config _config;
    private Action<Projectile> _release;
    private Rigidbody _rigidbody;
    private float _lifeTime;

    public void PerformShoot(Transform targetPosition)
    {
        gameObject.transform.parent = null;
        _collider.enabled = true;
        _rigidbody.velocity = transform.forward * _config.Speed;
    }

    private void Release()
    {
        if (gameObject.activeSelf)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.Sleep();
            _rigidbody.rotation = quaternion.identity;
            _rigidbody.position = Vector3.zero;
            _collider.enabled = false;
            _release.Invoke(this);
        }
    }

    private void Update()
    {
        _lifeTime += Time.deltaTime;

        if (_lifeTime >= _config.LifeTime)
        {
            Release();
        }
    }

    public void SetReleaseHandle(Action<Projectile> handle)
    {
        _release = handle;
    }

    public void Configure(Config config)
    {
        _lifeTime = 0;
        _config = config;
    }

    [Serializable]
    public struct Config
    {
        [SerializeField] public int Damage;
        [SerializeField] public float Speed;
        [SerializeField] public float LifeTime;
    }

    #region MonoCallbacks

    private void Awake()
    {
        enabled = false;
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<EnemyHealth>(out var enemyHealth)) enemyHealth.TakeDamage(_config.Damage);
        Release();
    }

    #endregion
}