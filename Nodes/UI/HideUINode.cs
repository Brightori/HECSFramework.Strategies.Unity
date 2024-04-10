using Commands;
using HECSFramework.Core;

namespace Strategies
{
    [Documentation(Doc.Strategy, "HideUINode")]
    public sealed class HideUINode : InterDecision
    {
        [Connection(ConnectionPointType.In, "<Entity> Additional Entity")]
        public GenericNode<Entity> AdditionalEntity;

        public override string TitleOfNode { get; } = "HideUINode";

        protected override void Run(Entity entity)
        {
            var needed = AdditionalEntity != null ? AdditionalEntity.Value(entity) : entity;
            needed.Command(new HideUICommand());
            Next.Execute(entity);
        }
    }
}