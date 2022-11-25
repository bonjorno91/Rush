public interface IStatePayload<in TPayload> : IStateExitable
{
    void OnEnter(TPayload payload);
}