using HECSFramework.Core;
using HECSFramework.Unity;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "this node return int value of identifier")]
    public class IdentifierNode : GenericNode<int>
    {
        public override string TitleOfNode { get; } = "IdentifierNode";

        [Connection(ConnectionPointType.Out, " <int> Out")]
        public BaseDecisionNode Out;

        [ExposeField]
        public IdentifierContainer Identifier;

        public override void Execute(Entity entity)
        {
        }

        public override int Value(Entity entity)
        {
            return Identifier;
        }
    }
}
