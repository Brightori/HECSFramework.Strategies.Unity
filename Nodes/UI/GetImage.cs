using Components;
using HECSFramework.Core;
using UnityEngine.UI;

namespace Strategies
{
    [Documentation(Doc.Strategy, "We get button from ui access element")]
    public sealed class GetImage : GenericNode<Image>
    {
        [Connection(ConnectionPointType.In, " <UIAccessMonoComponent> In")]
        public GenericNode<UIAccessMonoComponent> AdditionalProvider;

        public override string TitleOfNode { get; } = "GetImage";

        [Connection(ConnectionPointType.Out, " <Image> Out")]
        public BaseDecisionNode Out;

        [ExposeField]
        public UIAccessIdentifier AccessIdentifier;

        public override void Execute(Entity entity)
        {
        }

        public override Image Value(Entity entity)
        {
            if (AdditionalProvider != null)
                return AdditionalProvider.Value(entity).GetImage(AccessIdentifier);

            return entity.GetOrAddComponent<UIAccessProviderComponent>().Get.GetImage(AccessIdentifier);
        }
    }
}
