using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HECSFramework.Core;
using HECSFramework.Core.Generator;
using HECSFramework.Unity.Editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class ReactiveStrategiesHelper : OdinEditorWindow
{
    private const string FolderName = "/ForStrategiesGenerated/";

    private string FullPath => InstallHECS.ScriptPath + FolderName;

    [ValueDropdown(nameof(GetFilteredTypeList))]
    public Type CommandType;

    [MenuItem("HECS Options/Helpers/Strategies/ReactiveStrategiesHelper" , priority = 10)]
    public static void GetReactiveStrategiesHelperWindow()
    {
        GetWindow<ReactiveStrategiesHelper>();
    }

    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(ICommand).Assembly.GetTypes().Where(x=> x.IsCastableTo(typeof(ICommand)));
        return q;
    }

    [Button]
    public void GenerateLocalListeners()
    {
        var componentData = GetLocalListenerComponent().ToString();
        SaveToFile($"/Local{CommandType.Name}ToStrategyComponent.cs", componentData);

        var systemData = GetLocalListenerSystem().ToString();
        SaveToFile($"/Local{CommandType.Name}ToStrategySystem.cs", systemData);

        AssetDatabase.Refresh();
    }

    [Button]
    public void GenerateGlobalListeners()
    {
        var componentData = GetGlobalListenerComponent().ToString();
        SaveToFile($"/Global{CommandType.Name}ToStrategyComponent.cs", componentData);

        var systemData = GetGlobalListenerSystem().ToString();
        SaveToFile($"/Global{CommandType.Name}ToStrategySystem.cs", systemData);

        AssetDatabase.Refresh();
    }

    private ISyntax GetLocalListenerSystem()
    {
        var tree = new TreeSyntaxNode();

        tree.Add(new UsingSyntax(nameof(Commands)));
        tree.Add(new UsingSyntax(nameof(Components)));
        tree.Add(new UsingSyntax("HECSFramework.Core"));
        tree.Add(new UsingSyntax(nameof(System), 1));
        tree.Add(new NameSpaceSyntax(nameof(Systems)));
        
        tree.Add(new LeftScopeSyntax());

        tree.Add(new TabSimpleSyntax(1, $"[RequiredAtContainer(typeof(Local{CommandType.Name}ToStrategyComponent))]"));
        tree.Add(new TabSimpleSyntax(1, $"[Serializable][Documentation(Doc.Strategy, {CParse.Quote}this system execute strategy by listen command {CommandType.Name} {CParse.Quote})]"));
        tree.Add(new TabSimpleSyntax(1, $"public sealed class Local{CommandType.Name}ToStrategySystem : BaseSystem, IReactCommand<{CommandType.Name}> "));
        tree.Add(new LeftScopeSyntax(1));
        tree.Add(Body("Local"));
        tree.Add(new RightScopeSyntax(1));
        tree.Add(new RightScopeSyntax());
        return tree;
    }

    private ISyntax Body(string globalLocal)
    {
        var node = new TreeSyntaxNode();
        node.Add(new TabSimpleSyntax(3, $"private IStrategy{globalLocal}Executor<{CommandType.Name}> strategyExecutor;"));
        node.Add(new ParagraphSyntax());

        if (globalLocal == "Global")
            node.Add(new TabSimpleSyntax(3, $"public void CommandGlobalReact({CommandType.Name} command)"));
        else
            node.Add(new TabSimpleSyntax(3, $"public void CommandReact({CommandType.Name} command)"));

        node.Add(new LeftScopeSyntax(3));
        node.Add(new TabSimpleSyntax(4, "strategyExecutor.Value = command;"));
        node.Add(new TabSimpleSyntax(4, "strategyExecutor.BaseStrategy.Execute(Owner);"));
        node.Add(new RightScopeSyntax(3));

        node.Add(new TabSimpleSyntax(3, "public override void InitSystem()"));
        node.Add(new LeftScopeSyntax(3));
        node.Add(new TabSimpleSyntax(4, $"strategyExecutor = Owner.GetComponentTypeOf<IStrategy{globalLocal}Executor<{CommandType.Name}>>();"));
        node.Add(new ParagraphSyntax());
        node.Add(new TabSimpleSyntax(4, "if (strategyExecutor == null)"));
        node.Add(new LeftScopeSyntax(4));
        node.Add(new TabSimpleSyntax(5, $"HECSDebug.LogError({"$"}{CParse.Quote}we dont have executor for {CommandType.Name} {CParse.LeftScope}Owner.ID{CParse.RightScope}{CParse.Quote});"));
        node.Add(new RightScopeSyntax(4));
        node.Add(new RightScopeSyntax(3));
        return node;
    }

    private ISyntax GetGlobalListenerSystem()
    {
        var tree = new TreeSyntaxNode();

        tree.Add(new UsingSyntax(nameof(Commands)));
        tree.Add(new UsingSyntax(nameof(Components)));
        tree.Add(new UsingSyntax("HECSFramework.Core"));
        tree.Add(new UsingSyntax(nameof(System), 1));
        tree.Add(new NameSpaceSyntax(nameof(Systems)));

        tree.Add(new LeftScopeSyntax());

        tree.Add(new TabSimpleSyntax(1, $"[RequiredAtContainer(typeof(Global{CommandType.Name}ToStrategyComponent))]"));
        tree.Add(new TabSimpleSyntax(1, $"[Serializable][Documentation(Doc.Strategy, {CParse.Quote}this system execute strategy by listen command {CommandType.Name} {CParse.Quote})]"));
        tree.Add(new TabSimpleSyntax(1, $"public sealed class Global{CommandType.Name}ToStrategySystem : BaseSystem, IReactGlobalCommand<{CommandType.Name}>"));
        tree.Add(new LeftScopeSyntax(1));
        tree.Add(Body("Global"));
        tree.Add(new RightScopeSyntax(1));
        tree.Add(new RightScopeSyntax());
        return tree;
    }

    private ISyntax GetLocalListenerComponent()
    {
        var tree = new TreeSyntaxNode();

        tree.Add(new UsingSyntax(nameof(Commands)));
        tree.Add(new UsingSyntax("HECSFramework.Core"));
        tree.Add(new UsingSyntax(nameof(System), 1));
        tree.Add(new NameSpaceSyntax(nameof(Components)));

        tree.Add(new LeftScopeSyntax());

        tree.Add(new TabSimpleSyntax(1, $"[Serializable][Documentation(Doc.Strategy, {CParse.Quote}this component holds strategy for execute strategy by listen command {CommandType.Name} {CParse.Quote})]"));
        tree.Add(new TabSimpleSyntax(1, $"public sealed class Local{CommandType.Name}ToStrategyComponent : StrategyByLocalCommandComponent<{CommandType.Name}> "));
        tree.Add(new LeftScopeSyntax(1));
        tree.Add(new RightScopeSyntax(1));

        tree.Add(new RightScopeSyntax());
        return tree;
    }

    private ISyntax GetGlobalListenerComponent()
    {
        var tree = new TreeSyntaxNode();

        tree.Add(new UsingSyntax(nameof(Commands)));
        tree.Add(new UsingSyntax("HECSFramework.Core"));
        tree.Add(new UsingSyntax(nameof(System), 1));
        tree.Add(new NameSpaceSyntax(nameof(Components)));

        tree.Add(new LeftScopeSyntax());

        tree.Add(new TabSimpleSyntax(1, $"[Serializable][Documentation(Doc.Strategy, {CParse.Quote}this component holds strategy for execute strategy by listen command {CommandType.Name} {CParse.Quote})]"));
        tree.Add(new TabSimpleSyntax(1, $"public sealed class Global{CommandType.Name}ToStrategyComponent : StrategyByGlobalCommandComponent<{CommandType.Name}> "));
        tree.Add(new LeftScopeSyntax(1));
        tree.Add(new RightScopeSyntax(1));

        tree.Add(new RightScopeSyntax());
        return tree;
    }

    private void SaveToFile(string filename, string data)
    {
        try
        {
            if (!Directory.Exists(FullPath))
                Directory.CreateDirectory(FullPath);

            File.WriteAllText(FullPath + filename, data);
            var sourceFile2 = FullPath.Replace(Application.dataPath, "Assets");
            AssetDatabase.ImportAsset(sourceFile2);
        }
        catch
        {
            Debug.LogError("не смогли ослить " + FullPath + filename);
        }
    }
}
