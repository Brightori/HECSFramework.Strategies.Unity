using HECSFramework.Core.Generator;
using Strategies;

public class GenerateClass : BaseNodeConstruction
{
    [Connection(ConnectionPointType.In, "Usings")]
    public BaseNodeConstruction Usings;
  
    [Connection(ConnectionPointType.In, "Body")]
    public BaseNodeConstruction Body;

    [Connection(ConnectionPointType.In, "Fields")]
    public BaseNodeConstruction Fields;


    [Connection(ConnectionPointType.In, "ClassName")]
    public BaseNodeConstruction ClassName;

    [Connection(ConnectionPointType.In, "Parent")]
    public BaseNodeConstruction Parent;

    public override string Name { get; } = "GenerateClass";

    [ExposeField]
    public int Step;

    [ExposeField]
    public string ClassModifiers;

    public override ISyntax GetSyntax()
    {
        var tree = new TreeSyntaxNode();
        tree.Add(Usings.GetSyntax());
        tree.Add(new TabSimpleSyntax(Step, $"public {ClassModifiers} class {ClassName.GetSyntax()} : {Parent.GetSyntax()}"));
        tree.Add(new LeftScopeSyntax(Step));
        tree.Add(Fields.GetSyntax());
        tree.Add(Body.GetSyntax());
        tree.Add(new RightScopeSyntax(Step));
        return tree;
    }
}

public class GenerateGenericNodeClass : BaseNodeConstruction
{
    [Connection(ConnectionPointType.In, "Usings")]
    public BaseNodeConstruction Usings;

    [Connection(ConnectionPointType.In, "Body")]
    public BaseNodeConstruction Body;

    [Connection(ConnectionPointType.In, "Fields")]
    public BaseNodeConstruction Fields;


    [Connection(ConnectionPointType.In, "ClassName")]
    public BaseNodeConstruction ClassName;

    [Connection(ConnectionPointType.In, "GenericType")]
    public BaseNodeConstruction GenericType;

    public override string Name { get; } = "GenerateClass";

    [ExposeField]
    public int Step;

    [ExposeField]
    public string ClassModifiers;

    public override ISyntax GetSyntax()
    {
        var tree = new TreeSyntaxNode();
        tree.Add(Usings.GetSyntax());
        tree.Add(new TabSimpleSyntax(Step, $"public {ClassModifiers} class {ClassName.GetSyntax()} : GenericNode<{GenericType.GetSyntax()}>"));
        tree.Add(new LeftScopeSyntax(Step));
        tree.Add(Fields.GetSyntax());
        
        tree.Add(new TabSimpleSyntax(2, $"public override {GenericType.GetSyntax()} Value(Entity entity)"));
        tree.Add(new LeftScopeSyntax(2));
        tree.Add(Body.GetSyntax());
        tree.Add(new RightScopeSyntax(2));
        
        tree.Add(new RightScopeSyntax(Step));
        return tree;
    }
}

