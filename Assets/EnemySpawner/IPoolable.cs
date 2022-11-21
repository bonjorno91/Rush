using System;

public interface IPoolable<T>
{
    Action<T> PoolHandle { get; set; }
}