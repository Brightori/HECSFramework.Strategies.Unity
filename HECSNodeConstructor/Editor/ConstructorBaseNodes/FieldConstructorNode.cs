using System;
using HECSFramework.Core.Generator;
using Strategies;

public class FieldConstructorNode : BaseNodeConstruction
{
    [ExposeField]
    public string FieldName;

    [TypeConstructionField]
    public string Type;

    [ExposeField]
    public int Step;

    public override string Name { get; } = "FieldConstructorNode";

    public override ISyntax GetSyntax()
    {
        return new TabSimpleSyntax(Step, $"public {Type} {FieldName}");
    }
}
