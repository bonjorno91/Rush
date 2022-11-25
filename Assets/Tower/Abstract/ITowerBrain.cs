public interface ITowerBrain : IStateMachine<ITowerState>
{
    EnemyFactory EnemyFactory { get; }
    float ConstructionTime { get; }
    IWeapon Weapon { get; }
}