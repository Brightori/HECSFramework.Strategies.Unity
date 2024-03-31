using HECSFramework.Core.Generator;
using Strategies;

public class JoinNode : BaseNodeConstruction
{
    [Connection(ConnectionPointType.In, "Join")]
    public BaseNodeConstruction Join;

    [Connection(ConnectionPointType.In, "Parent")]
    public BaseNodeConstruction Parent;

    public override string Name { get; } = "JoinNode";

    public override ISyntax GetSyntax()
    {
        var node = new TreeSyntaxNode();
        node.Add(Join.GetSyntax());
        node.Add(Parent.GetSyntax());
        return node;
    }
}
