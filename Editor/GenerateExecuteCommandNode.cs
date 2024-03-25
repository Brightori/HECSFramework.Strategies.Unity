using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HECSFramework.Core;
using HECSFramework.Core.Generator;
using HECSFramework.Unity.Editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;

public class GenerateExecuteCommandNode : OdinEditorWindow
{
    private static string ConstructedNodes = "/ConstructedNodes/";

    [ValueDropdown(nameof(GetFilteredTypeList))]
    public Type CommandType;

    [ValueDropdown(nameof(GetFieldsList))]
    public FieldInfo[] FieldInfos = new FieldInfo[0];

    public bool IsGlobal;

    [MenuItem("HECS Options/Helpers/Strategies/Generate ExecuteCommand Node")]
    public static void GetGenerateExecuteCommandNodeWindow()
    {
        GetWindow<GenerateExecuteCommandNode>();
    }

    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(ICommand).Assembly.GetTypes().Where(x => x.IsCastableTo(typeof(ICommand)));
        return q;
    }

    public IEnumerable<FieldInfo> GetFieldsList()
    {
        if (CommandType == null)
            return Enumerable.Empty<FieldInfo>();

        return CommandType.GetFields();
    }

    [Button]
    public void GenerateCommandExecuteNode()
    {
        if (CommandType == null)
        {
            HECSDebug.LogError("command type is null");
            return;
        }

        var path = InstallHECS.ScriptPath + InstallHECS.HECSGenerated + ConstructedNodes;
        InstallHECS.CheckFolder(path);
        InstallHECS.SaveToFile(GetInterDecision().ToString(), path + $"/Execute{CommandType.Name}Node.cs");
        Close();
        AssetDatabase.Refresh();
    }

    public ISyntax GetFields()
    {
        var node = new TreeSyntaxNode();

        foreach (var field in FieldInfos) 
        {
            node.Add(GetInputField( new CreateFieldInfo { ConnectionPointType = Strategies.ConnectionPointType.In, Name = field.Name, Type = field.FieldType.Name })); 
        }
        return node;
    }

    private ISyntax GetInputField(CreateFieldInfo createFieldInfo)
    {
        var tree = new TreeSyntaxNode();
        tree.Add(new TabSimpleSyntax(2, $"[Connection(ConnectionPointType.In, {CParse.Quote}<{GetProperName(createFieldInfo.Type)}> {createFieldInfo.Name}{CParse.Quote})]"));
        tree.Add(new TabSimpleSyntax(2, $"public GenericNode<{GetProperName( createFieldInfo.Type)}> {createFieldInfo.Name};"));
        return tree;
    }

    private string GetProperName(string type)
    {
        return StrategyGraphView.FromDotNetTypeToCSharpType(type);
    }

    private ISyntax GetInterDecision()
    {
        var node = new TreeSyntaxNode();
        var fieldsInput = new TreeSyntaxNode();
        var body = new TreeSyntaxNode();

        node.Tree.Add(new UsingSyntax("Commands"));
        node.Tree.Add(new UsingSyntax(nameof(System)));
        node.Tree.Add(new UsingSyntax("HECSFramework.Core", 1));
        node.Tree.Add(new NameSpaceSyntax("Strategies"));
        node.Tree.Add(new LeftScopeSyntax());//{
        node.Tree.Add(new TabSimpleSyntax(1, $"[Documentation(Doc.Strategy, Doc.HECS, {CParse.Quote}this node execute command {CommandType.Name} {CParse.Quote})]"));
        node.Tree.Add(new TabSimpleSyntax(1, $"public class Execute{CommandType.Name}Node : InterDecision"));
        node.Tree.Add(new LeftScopeSyntax(1));//{{
        node.Tree.Add(GetFields());
        node.Tree.Add(new TabSimpleSyntax(2, $"public override string TitleOfNode {{ get; }} = {CParse.Quote} Execute {CommandType.Name}{CParse.Quote};"));
        node.Tree.Add(body);
        node.Tree.Add(new RightScopeSyntax(1));//}}
        node.Tree.Add(new RightScopeSyntax());//}

        body.AddUnique(new TabSimpleSyntax(2, "protected override void Run(Entity entity)"));
        body.Tree.Add(new LeftScopeSyntax(2)); //{{{
        body.Tree.Add(GetCommand());

        if (IsGlobal)
            body.Tree.Add(new TabSimpleSyntax(3, $"entity.World.Command(command);"));
        else
            body.Tree.Add(new TabSimpleSyntax(3, $"entity.Command(command);"));

        body.Tree.Add(new TabSimpleSyntax(3, " Next.Execute(entity);"));
        body.Tree.Add(new RightScopeSyntax(2)); //}}}



        return node;
    }

    private ISyntax GetCommand()
    {
        var node = new TreeSyntaxNode();
        var fields = new TreeSyntaxNode();

        node.Add(new TabSimpleSyntax(3, $"var command = new {CommandType.Name}"));
        node.Add(new LeftScopeSyntax(3));
        node.Add(fields);
        node.Add(new RightScopeSyntax(3, true));

        foreach (var field in FieldInfos)
        {
            fields.Add(new TabSimpleSyntax(4, $"{field.Name} = this.{field.Name}.Value(entity),"));
        }

        return node;
    }
}
