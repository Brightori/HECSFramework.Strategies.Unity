using HECSFramework.Core;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "Int t")]
    public class FloatToString : GenericNode<string>
    {
        [Connection(ConnectionPointType.In, "<int> In")]
        public GenericNode<float> In;
        [Connection(ConnectionPointType.Out, "<string> Out")]
        public BaseDecisionNode Out;
        public override string TitleOfNode { get; } = "FloatToString";
        public override void Execute(Entity entity)
        {
        }
        public override string Value(Entity entity)
        {
            return In.Value(entity).ToString();
        }
    }
}
