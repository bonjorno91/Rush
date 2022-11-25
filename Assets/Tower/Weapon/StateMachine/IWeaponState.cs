public interface IWeaponState<in TPayload> : IWeaponState, IStatePayload<TPayload>
{
    
}

public interface IWeaponState : IShoot, IStateExitable
{
    void Tick();
}