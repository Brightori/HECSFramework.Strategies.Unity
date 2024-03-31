using HECSFramework.Core.Generator;
using Strategies;

public class AddStepConstructionNode : BaseNodeConstruction
{
    [Connection(ConnectionPointType.In, "Node")]
    public BaseNodeConstruction BaseNodeConstruction;

    [ExposeField]
    public int Step;

    public override string Name { get; } = "GetMethodNode";

    public override ISyntax GetSyntax()
    {
        return new TabSimpleSyntax(Step, BaseNodeConstruction.GetSyntax().ToString());
    }
}