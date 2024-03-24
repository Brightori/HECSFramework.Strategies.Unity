using Components;
using HECSFramework.Core;

namespace Systems
{
    public abstract class StrategyByGlobalCommandExecuteSystem<T> : BaseSystem, IReactGlobalCommand<T> where T : struct, IGlobalCommand
    {
        private IStrategyGlobalExecutor<T> strategyExecutor;

        public void CommandGlobalReact(T command)
        {
            strategyExecutor.Value = command;
            strategyExecutor.BaseStrategy.Execute(Owner);
        }

        public override void InitSystem()
        {
            strategyExecutor = Owner.GetComponentTypeOf<IStrategyGlobalExecutor<T>>();

            if (strategyExecutor == null)
            {
                HECSDebug.LogError($"we dont have needed type {typeof(T).Name} on {Owner.ID}");
            }
        }
    }
}
