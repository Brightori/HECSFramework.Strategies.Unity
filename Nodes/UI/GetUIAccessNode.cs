using Components;
using HECSFramework.Core;

namespace Strategies
{
    [Documentation(Doc.Strategy, "")]
    public class GetUIAccessNode : GenericNode<UIAccessMonoComponent>
    {
        [Connection(ConnectionPointType.In, "<UIAccessMonoComponent> AdditionalAccess")]
        public GenericNode<UIAccessMonoComponent> AdditionalAccess;
        [Connection(ConnectionPointType.Out, "<UIAccessMonoComponent> Out")]
        public BaseDecisionNode Out;

        [ExposeField]
        public UIAccessIdentifier UIAccessIdentifier;
        public override string TitleOfNode { get; } = "GetUIAccess";
        public override void Execute(Entity entity)
        {
        }
        public override UIAccessMonoComponent Value(Entity entity)
        {
            if (AdditionalAccess != null)
                return AdditionalAccess.Value(entity).GetUIAccess(UIAccessIdentifier);

            return entity.GetOrAddComponent<UIAccessProviderComponent>().Get.GetUIAccess(UIAccessIdentifier);
        }
    }
}
