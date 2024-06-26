﻿using Components;
using HECSFramework.Core;
using UnityEngine.UI;

namespace Strategies
{
    [NodeTypeAttribite("Meta")]
    [Documentation(Doc.Strategy, "We get button image ui access element")]
    public sealed class GetImage : GenericNode<Image>, IInitable
    {
        [Connection(ConnectionPointType.In, " <UIAccessMonoComponent> In")]
        public GenericNode<UIAccessMonoComponent> AdditionalProvider;

        public override string TitleOfNode { get; } = "GetImage";

        [Connection(ConnectionPointType.Out, " <Image> Out")]
        public BaseDecisionNode Out;

        [MetaNode]
        [Connection(ConnectionPointType.Out, "<GameObject> Out")]
        public GetGameObjectMetaNode GameObject;

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

        public void Init()
        {
            if (GameObject != null)
                GameObject.GetComponentFromNode = Value;
        }
    }
}
