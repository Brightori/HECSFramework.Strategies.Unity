using HECSFramework.Core;
using HECSFramework.Unity;
using Systems;

namespace Strategies
{
    [Documentation(Doc.Strategy, "ReturnToPoolNode")]
    public class ReturnToPoolNode : FinalDecision
    {
        public override string TitleOfNode { get; } = "ReturnToPoolNode";

        protected override void Run(Entity entity)
        {
            entity.World.GetSingleSystem<PoolingSystem>().Release(entity.AsActor());
        }
    }
}
