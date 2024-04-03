using HECSFramework.Core;
using HECSFramework.Unity;

namespace Strategies
{
    [Documentation(Doc.HECS, Doc.Strategy, "here we convert actor to entity")]
    public sealed class ActorToEntity : ConvertNode<Actor, Entity>
    {
        public override string TitleOfNode { get; } = "ActorToEntity";

        public override void Execute(Entity entity)
        {
            throw new System.NotImplementedException();
        }

        public override Entity Value(Entity entity)
        {
            return From.Value(entity).Entity;
        }
    }
}