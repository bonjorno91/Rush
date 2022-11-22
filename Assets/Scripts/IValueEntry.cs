public interface IValueEntry<out TEntry>
{
    TEntry Value { get; }
    void Release();
}