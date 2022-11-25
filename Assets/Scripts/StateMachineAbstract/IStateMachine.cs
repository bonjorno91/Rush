public interface IStateMachine<in TMachineState> where TMachineState : IStateExitable
{
    void EnterState<TState,TPayload>(TPayload payload) where TState : class, TMachineState, IStatePayload<TPayload>;
}