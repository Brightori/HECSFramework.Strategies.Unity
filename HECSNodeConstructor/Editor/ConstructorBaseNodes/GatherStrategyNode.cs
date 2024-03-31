using HECSFramework.Core.Generator;
using Strategies;

public class GatherStrategyNode : BaseNodeConstruction
{
    [Connection(ConnectionPointType.In, "FileName")]
    public BaseNodeConstruction FileName;

    [Connection(ConnectionPointType.In, "Chain")]
    public BaseNodeConstruction Chain;
    public override string Name { get; } = "GatherStrategyNode";

    public override ISyntax GetSyntax()
    {
        return Chain.GetSyntax();
    }
}