using Components;
using HECSFramework.Core;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "GetUIAccessNode provide uiaccess component from provider")]
    public class GetUIAccessNodeGeneric : GenericNode<UIAccessMonoComponent>
    {
        [Connection(ConnectionPointType.In, "<UIAccessMonoComponent> AdditionalAccess")]
        public GenericNode<UIAccessMonoComponent> AdditionalAccess;
        [Connection(ConnectionPointType.Out, "<UIAccessMonoComponent> Out")]
        public BaseDecisionNode Out;

        [Connection(ConnectionPointType.In, "<int> AccessID")]
        public GenericNode<int> ID;
        public override string TitleOfNode { get; } = "GetUIAccessNodeGeneric";
        public override void Execute(Entity entity)
        {
        }
        public override UIAccessMonoComponent Value(Entity entity)
        {
            if (AdditionalAccess != null)
                return AdditionalAccess.Value(entity).GetUIAccess(ID.Value(entity));

            return entity.GetOrAddComponent<UIAccessProviderComponent>().Get.GetUIAccess(ID.Value(entity));
        }
    }
}
