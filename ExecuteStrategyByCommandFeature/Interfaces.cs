using HECSFramework.Core;
using Strategies;

namespace Components
{
    public interface IStrategyLocalExecutor<T> : IStrategyProvider where T : struct, ICommand
    {
        T Value { get; set; }
    }

    public interface IStrategyGlobalExecutor<T> : IStrategyProvider where T : struct, IGlobalCommand
    {
        T Value { get; set; }
    }

    public interface IStrategyProvider
    {
        BaseStrategy BaseStrategy { get; }
    }
}