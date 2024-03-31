using HECSFramework.Core.Generator;
using Strategies;

public class ScopeConstructorNode : BaseNodeConstruction
{
    [Connection(ConnectionPointType.In, "ScopeHeaderFunc")]
    public BaseNodeConstruction ScopeHeaderFunc;

    [Connection(ConnectionPointType.In, "Body")]
    public BaseNodeConstruction Body;

    [ExposeField]
    public int Step;

    public override string Name { get; } = "ScopeConstructorNode";

    public override ISyntax GetSyntax()
    {
        var node = new TreeSyntaxNode();
        node.Add(ScopeHeaderFunc.GetSyntax());
        node.Add(new LeftScopeSyntax(Step));
        node.Add(Body.GetSyntax());
        node.Add(new RightScopeSyntax(Step));
        return node;
    }
}
