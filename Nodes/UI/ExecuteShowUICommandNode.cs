using Commands;
using System;
using HECSFramework.Core;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "this node execute command ShowUICommand ")]
    public class ExecuteShowUICommandNode : InterDecision
    {
        [Connection(ConnectionPointType.In, "<int> UIViewType")]
        public GenericNode<int> UIViewType;
        public override string TitleOfNode { get; } = " Execute ShowUICommand";
        protected override void Run(Entity entity)
        {
            var command = new ShowUICommand
            {
                UIViewType = this.UIViewType.Value(entity),
            };
            entity.World.Command(command);
            Next.Execute(entity);
        }
    }
}
