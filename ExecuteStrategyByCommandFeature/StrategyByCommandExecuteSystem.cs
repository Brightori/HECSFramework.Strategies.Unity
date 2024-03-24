using Components;
using HECSFramework.Core;
using Strategies;

namespace Systems
{
    public abstract class StrategyByCommandExecuteSystem<T> : BaseSystem, IReactCommand<T> where T : struct, ICommand
    {
        private IStrategyLocalExecutor<T> strategyExecutor;

        public void CommandReact(T command)
        {
            strategyExecutor.Value = command;
            strategyExecutor.BaseStrategy.Execute(Owner);
        }

        public override void InitSystem()
        {
            strategyExecutor = Owner.GetComponentTypeOf<IStrategyLocalExecutor<T>>();

            if (strategyExecutor == null)
            {
                HECSDebug.LogError($"we dont have needed type {typeof(T).Name} on {Owner.ID}");
            }
        }
    }
}

