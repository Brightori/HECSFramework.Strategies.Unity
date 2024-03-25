using Components;
using HECSFramework.Core;
using UnityEngine.UI;

namespace Strategies
{
    [Documentation(Doc.Strategy, "We get button from ui access element")]
    public class GetImage : GenericNode<Image>
    {
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
            return entity.GetOrAddComponent<UIAccessProviderComponent>().Get.GetImage(AccessIdentifier);
        }
    }
}
