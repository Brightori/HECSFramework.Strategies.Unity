using HECSFramework.Core;
using UnityEngine;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "LerpFloatNode")]
    public class LerpFloatNode : GenericNode<float>
    {
        [Connection(ConnectionPointType.In, "<float> From")]
        public GenericNode<float> From;
        [Connection(ConnectionPointType.In, "<float> To")]
        public GenericNode<float> To;

        [Connection(ConnectionPointType.In, "<float> Progress")]
        public GenericNode<float> Progress;

        [Connection(ConnectionPointType.Out, "<float> Out")]
        public BaseDecisionNode Out;

        public override string TitleOfNode { get; } = "LerpFloatNode";

        public override void Execute(Entity entity)
        {
            throw new System.NotImplementedException();
        }

        public override float Value(Entity entity)
        {
            return Mathf.Lerp(From.Value(entity), To.Value(entity), Progress.Value(entity));
        }
    }
}