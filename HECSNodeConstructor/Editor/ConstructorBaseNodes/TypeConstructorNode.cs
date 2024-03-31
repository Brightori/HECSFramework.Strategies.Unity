using HECSFramework.Core.Generator;

public class TypeConstructorNode : BaseNodeConstruction
{
    [TypeConstructionField]
    public string Type;
   
    public override string Name { get; } = "TypeConstructorNode";

    public override ISyntax GetSyntax()
    {
        return new SimpleSyntax($"{Type}");
    }
}
