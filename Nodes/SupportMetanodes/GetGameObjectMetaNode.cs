using HECSFramework.Core;
using UnityEngine;

namespace Strategies
{
    public delegate Component GetComponentFromNode (Entity entity);

    [Documentation(Doc.Strategy, Doc.HECS, "we use this node for metanodes, when we want convert monobeh to gameobject and provide access to monobehaviours gameobject")]
    public sealed class GetGameObjectMetaNode : GenericNode<GameObject>
    {
        public override string TitleOfNode { get; } = "GetGameObjectMetaNode";

        [Connection(ConnectionPointType.Out, " <GameObject> Out")]
        public BaseDecisionNode Out;

        public GetComponentFromNode GetComponentFromNode;

        public override void Execute(Entity entity)
        {
        }

        public override GameObject Value(Entity entity)
        {
            return GetComponentFromNode.Invoke(entity).gameObject;
        }
    }
}
