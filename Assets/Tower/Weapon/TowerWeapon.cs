using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class TowerWeapon : Weapon, IWeaponBrain
{
    [SerializeField] private Transform _launchTransform;
    [SerializeField] private int _initialCooldown;

    public override bool Shoot(Transform target)
    {
        return _currentState?.Shoot(target) ?? false;
    }

    #region MonoCallbacks

    private void Awake()
    {
        SetupStateMachine();
    }

    private void OnEnable()
    {
        EnterState<Reload, float>(_initialCooldown);
    }

    private void Update()
    {
        _currentState?.Tick();
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
        var readyState = new Ready(this);
        var reloadState = new Reload(this, _launchTransform);

        _weaponStates.Add(typeof(Ready), readyState);
        _weaponStates.Add(typeof(Reload), reloadState);
    }

    #endregion
}