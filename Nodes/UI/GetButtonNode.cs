using Components;
using HECSFramework.Core;
using UnityEngine.UI;

namespace Strategies
{
    [Documentation(Doc.Strategy, "We get button from ui access element")]
    public class GetButtonNode : GenericNode<Button>
    {
        public override string TitleOfNode { get; } = "GetButtonNode";

        [Connection(ConnectionPointType.Out, " <Button> Out")]
        public BaseDecisionNode Out;

        [ExposeField]
        public UIAccessIdentifier AccessIdentifier;

        public override void Execute(Entity entity)
        {
        }

        public override Button Value(Entity entity)
        {
            return entity.GetComponent<UIAccessProviderComponent>().Get.GetButton(AccessIdentifier);
        }
    }
}
