using HECSFramework.Core.Generator;
using Strategies;

public class TabSimpleConstructionNode : BaseNodeConstruction
{
    public override string Name { get; } = "TabSimpleConstructionNode";

    [ExposeField]
    public int Step;

    [ExposeField]
    public string Body;

    public override ISyntax GetSyntax()
    {
        return new TabSimpleSyntax(Step, Body);
    }
}
