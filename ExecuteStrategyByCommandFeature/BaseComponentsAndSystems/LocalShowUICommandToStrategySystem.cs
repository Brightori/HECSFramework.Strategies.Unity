using Commands;
using Components;
using HECSFramework.Core;
using System;

namespace Systems
{
	[RequiredAtContainer(typeof(LocalShowUICommandToStrategyComponent))]
	[Serializable][Documentation(Doc.Strategy, "this system execute strategy by listen command ShowUICommand ")]
	public sealed class LocalShowUICommandToStrategySystem : BaseSystem, IReactCommand<ShowUICommand> 
	{
        private IStrategyLocalExecutor<ShowUICommand> strategyExecutor;

        public void CommandReact(ShowUICommand command)
        {
            strategyExecutor.Value = command;
            strategyExecutor.BaseStrategy.Execute(Owner);
        }

        public override void InitSystem()
        {
            strategyExecutor = Owner.GetComponentTypeOf<IStrategyLocalExecutor<ShowUICommand>>();

            if (strategyExecutor == null)
            {
                HECSDebug.LogError($"we dont have needed type {typeof(ShowUICommand).Name} on {Owner.ID}");
            }
        }
    }
}
