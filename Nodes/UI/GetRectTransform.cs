using Components;
using HECSFramework.Core;
using UnityEngine;

namespace Strategies
{
    [NodeTypeAttribite("Meta")]
    [Documentation(Doc.Strategy, "We get button image ui access element")]
    public sealed class GetRectTransform : GenericNode<RectTransform>, IInitable
    {
        [Connection(ConnectionPointType.In, " <UIAccessMonoComponent> In")]
        public GenericNode<UIAccessMonoComponent> AdditionalProvider;

        public override string TitleOfNode { get; } = "GetRectTransform";

        [Connection(ConnectionPointType.Out, " <RectTransform> Out")]
        public BaseDecisionNode Out;

        [MetaNode]
        [Connection(ConnectionPointType.Out, "<GameObject> Out")]
        public GetGameObjectMetaNode GameObject;

        [Connection(ConnectionPointType.In, " <int> UIAccessID")]
        public GenericNode<int> GetAccessID;

        public override void Execute(Entity entity)
        {
        }

        public override RectTransform Value(Entity entity)
        {
            if (AdditionalProvider != null)
                return AdditionalProvider.Value(entity).GetRectTransform(GetAccessID.Value(entity));

            return entity.GetOrAddComponent<UIAccessProviderComponent>().Get.GetRectTransform(GetAccessID.Value(entity));
        }

        public void Init()
        {
            if (GameObject != null)
                GameObject.GetComponentFromNode = Value;
        }
    }
}
