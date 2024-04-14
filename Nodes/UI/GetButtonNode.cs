using Components;
using HECSFramework.Core;
using UnityEngine.UI;

namespace Strategies
{
    [NodeTypeAttribite("Meta")]
    [Documentation(Doc.Strategy, "We get button from ui access element")]
    public class GetButtonNode : GenericNode<Button>, IInitable
    {
        public override string TitleOfNode { get; } = "GetButtonNode";

        [Connection(ConnectionPointType.Out, " <Button> Out")]
        public BaseDecisionNode Out;

        [Connection(ConnectionPointType.In, " <UIAccessMonoComponent> In")]
        public GenericNode<UIAccessMonoComponent> AdditionalProvider;

        [ExposeField]
        public UIAccessIdentifier AccessIdentifier;

        [MetaNode]
        [Connection(ConnectionPointType.Out, "<GameObject> Out")]
        public GetGameObjectMetaNode GameObject;

        public override void Execute(Entity entity)
        {
        }

        public override Button Value(Entity entity)
        {
            if (AdditionalProvider != null)
                return AdditionalProvider.Value(entity).GetButton(AccessIdentifier);

            return entity.GetComponent<UIAccessProviderComponent>().Get.GetButton(AccessIdentifier);
        }

        public void Init()
        {
            if (GameObject != null)
                GameObject.GetComponentFromNode = Value;
        }
    }
}