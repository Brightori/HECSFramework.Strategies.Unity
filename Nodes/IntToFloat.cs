using HECSFramework.Core;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "IntToFloat")]
    public class IntToFloat : GenericNode<float>
    {
        [Connection(ConnectionPointType.In, "<int> In")]
        public GenericNode<int> In;
        [Connection(ConnectionPointType.Out, "<float> Out")]
        public BaseDecisionNode Out;
        public override string TitleOfNode { get; } = "IntToFloat";
        public override void Execute(Entity entity)
        {
        }
        public override float Value(Entity entity)
        {
            return In.Value(entity);
        }
    }
}
