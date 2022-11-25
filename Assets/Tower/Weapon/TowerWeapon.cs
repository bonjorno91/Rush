using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public sealed class TowerWeapon : Weapon, IStateMachine<IWeaponState>
{
    [SerializeField] private Transform _launchTransform;
    private ObjectPool<Projectile> _projectilePool;
    
    public override void Reload()
    {
        _projectilePool.Get(out var projectile);
        EnterState<Ready, IProjectile>(projectile);
    }

    public override bool Shoot(Transform target)
    {
        if (_currentState == null) return false;

        return TryShoot(target);
    }

    private bool TryShoot(Transform target)
    {
        if (_currentState.Shoot(target))
        {
            PlayReleaseSound();
            EnterState<Reload, IWeapon>(this);
            return true;
        }

        return false;
    }

    #region MonoCallbacks

    protected void Awake()
    {
        base.Awake();
        SetupProjectilePool();
        SetupStateMachine();
    }
    
    private void OnEnable()
    {
        EnterState<Reload, IWeapon>(this);
    }

    private void Update()
    {
        _currentState?.Tick();
    }

    #endregion

    #region Pool

    private void SetupProjectilePool()
    {
        _projectilePool = new ObjectPool<Projectile>(Create, Get, Release, Destroy);
    }

    private Projectile Create()
    {
        var instance = Instantiate(Projectile, _launchTransform);
        
        instance.Configure(_projectileConfig);
        instance.SetReleaseHandle(_projectilePool.Release);
        
        return instance;
    }

    private void Get(Projectile obj)
    {
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.Configure(_projectileConfig);
        obj.gameObject.SetActive(true);
    }

    private void Release(Projectile obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(_launchTransform);
    }

    #endregion
    
    #region StateMachine

    private readonly Dictionary<Type, IWeaponState> _weaponStates = new();

    private IWeaponState _currentState;

    public void EnterState<TState, TPayload>(TPayload payload) where TState : class, IWeaponState, IStatePayload<TPayload>
    {
        _currentState?.OnExit();

        if (_weaponStates.TryGetValue(typeof(TState), out _currentState))
            if (_currentState is IWeaponState<TPayload> nextState)
                nextState.OnEnter(payload);
    }

    private void SetupStateMachine()
    {
        var readyState = new Ready();
        var reloadState = new Reload();

        _weaponStates.Add(typeof(Ready), readyState);
        _weaponStates.Add(typeof(Reload), reloadState);
    }

    #endregion
}