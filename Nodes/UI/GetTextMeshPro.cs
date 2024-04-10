using Components;
using HECSFramework.Core;
using TMPro;

namespace Strategies
{
    [Documentation(Doc.Strategy, "We get button from ui access element")]
    public class GetTextMeshPro : GenericNode<TextMeshProUGUI>
    {
        [Connection(ConnectionPointType.In, " <UIAccessMonoComponent> In")]
        public GenericNode<UIAccessMonoComponent> AdditionalProvider;
        public override string TitleOfNode { get; } = "GetTextMeshPro";

        [Connection(ConnectionPointType.Out, " <TextMeshProUGUI> Out")]
        public BaseDecisionNode Out;

        [ExposeField]
        public UIAccessIdentifier AccessIdentifier;

        public override void Execute(Entity entity)
        {
        }

        public override TextMeshProUGUI Value(Entity entity)
        {
            if (AdditionalProvider != null)
                return AdditionalProvider.Value(entity).GetTextMeshProUGUI(AccessIdentifier);

            return entity.GetOrAddComponent<UIAccessProviderComponent>().Get.GetTextMeshProUGUI(AccessIdentifier);
        }
    }
}