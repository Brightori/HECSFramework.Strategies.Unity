using Commands;
using Components;
using HECSFramework.Core;
using System;

namespace Systems
{
	[RequiredAtContainer(typeof(GlobalShowUICommandToStrategyComponent))]
	[Serializable][Documentation(Doc.Strategy, "this system execute strategy by listen command ShowUICommand ")]
	public sealed class GlobalShowUICommandToStrategySystem : BaseSystem, IReactGlobalCommand<ShowUICommand> 
	{
        private IStrategyGlobalExecutor<ShowUICommand> strategyExecutor;

        public void CommandGlobalReact(ShowUICommand command)
        {
            strategyExecutor.Value = command;
            strategyExecutor.BaseStrategy.Execute(Owner);
        }

        public override void InitSystem()
        {
            strategyExecutor = Owner.GetComponentTypeOf<IStrategyGlobalExecutor<ShowUICommand>>();

            if (strategyExecutor == null)
            {
                HECSDebug.LogError($"we dont have needed type {typeof(ShowUICommand).Name} on {Owner.ID}");
            }
        }
    }
}
