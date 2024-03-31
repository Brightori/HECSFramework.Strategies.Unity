using HECSFramework.Core.Generator;
using Strategies;

public class SimpleStringNode : BaseNodeConstruction
{
    [ExposeField]
    public string Text;

    public override string Name { get; } = "SimpleStringNode";

    public override ISyntax GetSyntax()
    {
        return new SimpleSyntax(Text);
    }
} 
