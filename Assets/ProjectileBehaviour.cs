using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))][RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _releaseSound;
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    private AudioSource _audioSource;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        enabled = false;
        _audioSource = GetComponent<AudioSource>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public bool PerformShoot(Transform targetPosition)
    {
        transform.parent = null;
        enabled = true;
        _audioSource.PlayOneShot(_releaseSound);
        Destroy(gameObject,3);
        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            enemyHealth.TakeDamage(_damage);
        }

        _audioSource.PlayOneShot(_hitSound);
        Destroy(gameObject);
    }

    private void Update()
    {
        _rigidbody.MovePosition(gameObject.transform.position + gameObject.transform.forward * _speed * Time.deltaTime);
        // gameObject.transform.Translate(gameObject.transform.forward * _speed * Time.deltaTime, _relativeTo);
    }

    public ProjectileBehaviour Initialize()
    {
        return this;
    }
}