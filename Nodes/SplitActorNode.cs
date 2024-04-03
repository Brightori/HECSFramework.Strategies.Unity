using HECSFramework.Core;
using HECSFramework.Unity;

namespace Strategies
{
    [Documentation(Doc.HECS, Doc.Strategy, Doc.UniversalNodes, "this node needed when we need split generic actor  node result to two branches for reuse result without repeat calculation")]
    public sealed class SplitActorNode : SplitGenericNode<Actor>
    {
        public override string TitleOfNode { get; } = "SplitActorNode";

        public override void Execute(Entity entity)
        {
            throw new System.NotImplementedException();
        }
    }
}