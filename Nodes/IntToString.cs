using HECSFramework.Core;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "Int t")]
	public class IntToString : GenericNode<string>
	{
		[Connection(ConnectionPointType.In, "<int> In")]
		public GenericNode<int> In;
		[Connection(ConnectionPointType.Out, "<string> Out")]
		public BaseDecisionNode Out;
		public override string TitleOfNode { get; } = "IntToString";
		public override void Execute(Entity entity)
		{
		}
		public override string Value(Entity entity)
		{
			return In.Value(entity).ToString();
		}
	}
}
