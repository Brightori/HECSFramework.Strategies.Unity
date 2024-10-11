using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HECSFramework.Core;
using HECSFramework.Core.Generator;
using HECSFramework.Unity.Editor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

public class GenerateGetNodeForCommandWindow : OdinEditorWindow
{
    [OnValueChanged(nameof(ClearField))]
    [ValueDropdown(nameof(GetCommands))]
    public Type Command;

    [HideIf("@Command == null")]
    [ValueDropdown(nameof(GetFields))]
    public string Field;

    private const string StrategyNodes = "/StrategyNodes/";

    private Type[] commands;

    [MenuItem("HECS Options/Helpers/Strategies/Generate GetNode For Command")]
    public static void GetGenerateValueNodeFromComponentWindow()
    {
        GetWindow<GenerateGetNodeForCommandWindow>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        var type = typeof(ICommand);
        commands = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p)).ToArray();
    }

    private IEnumerable<Type> GetCommands()
    {
        
        return commands;
    }

    private void ClearField()
    {
        Field = string.Empty;
    }

    private IEnumerable<string> GetFields()
    {
        if (Command == null)
            return default;



        var fields = Command.GetFields(BindingFlags.Public | BindingFlags.Instance);
        var properties = Command.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        var list = new List<string>(16);

        foreach (var f in fields)
        {
            list.Add(f.Name);
        }

        foreach (var f in properties)
        {
            list.Add(f.Name);
        }

        return list;
    }

    [Button]
    [HideIf("@string.IsNullOrEmpty(Field)")]
    private void GenerateNode()
    {
        var field = Command.GetMember(Field)[0];

        var tree = new TreeSyntaxNode();
        var usings = new TreeSyntaxNode();

        tree.Add(usings);
        tree.Add(new UsingSyntax("HECSFramework.Core"));
        tree.Add(new UsingSyntax("Components"));
        tree.Add(new UsingSyntax("Commands"));
        tree.AddUnique(new UsingSyntax(GetNameSpace(field)));

        tree.Add(new NameSpaceSyntax("Strategies"));
        tree.Add(new LeftScopeSyntax());

        tree.Add(new TabSimpleSyntax(1, $"public sealed class {Command.Name}Get{Field}: GenericNode<{GetFieldType(field)}>"));
        tree.Add(new LeftScopeSyntax(1));
        tree.Add(new TabSimpleSyntax(2, $"public override string TitleOfNode {CParse.LeftScope} get; {CParse.RightScope} = {CParse.Quote}{Command.Name}Get{Field}{CParse.Quote};"));

        tree.Add(new TabSimpleSyntax(2, $"[Connection(ConnectionPointType.In, {CParse.Quote}Command{CParse.Quote})]"));
        tree.Add(new TabSimpleSyntax(2, $"public GenericNode<{Command.Name}> Command;"));

        tree.Add(new TabSimpleSyntax(2, $"[Connection(ConnectionPointType.Out, {CParse.Quote}<{GetFieldType(field)}> Out{CParse.Quote})]"));
        tree.Add(new TabSimpleSyntax(2, "public BaseDecisionNode Out;"));

        tree.Add(new TabSimpleSyntax(2, "public override void Execute(Entity entity)"));
        tree.Add(new LeftScopeSyntax(2));
        tree.Add(new RightScopeSyntax(2));

        tree.Add(new TabSimpleSyntax(2, $"public override {GetFieldType(field)} Value(Entity entity)"));
        tree.Add(new LeftScopeSyntax(2));
        tree.Add(new TabSimpleSyntax(3, $"return Command.Value(entity).{Field};"));
        tree.Add(new RightScopeSyntax(2));

        tree.Add(new RightScopeSyntax(1));
        tree.Add(new RightScopeSyntax());

        InstallHECS.CheckFolder(InstallHECS.ScriptPath + InstallHECS.HECSGenerated + StrategyNodes);
        InstallHECS.SaveToFile(tree.ToString(), InstallHECS.ScriptPath + InstallHECS.HECSGenerated + StrategyNodes + $"{Command.Name}Get{Field}.cs");
        AssetDatabase.SaveAssets();
        Close();
    }

    private string GetNameSpace(MemberInfo member)
    {
        if (member is FieldInfo field)
            return field.FieldType.Namespace;
        else
            if (member is PropertyInfo property)
            return property.PropertyType.Namespace;

        return null;
    }


    private string GetFieldType(MemberInfo memberInfo)
    {
        if (memberInfo is FieldInfo fieldInfo)
        {
            if (fieldInfo.FieldType.IsGenericType)
            {
                if (fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return $"List<{fieldInfo.FieldType.GetGenericArguments()[0].Name}>";
                }

                if (fieldInfo.FieldType.GetGenericTypeDefinition() == typeof(HECSList<>))
                {
                    return $"HECSList<{fieldInfo.FieldType.GetGenericArguments()[0].Name}>";
                }
            }

            return fieldInfo.FieldType.Name;
        }

        if (memberInfo is PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsGenericType)
            {
                if (propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
                {
                    return $"List<{propertyInfo.PropertyType.GetGenericArguments()[0].Name}>";
                }

                if (propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(HECSList<>))
                {
                    return $"HECSList<{propertyInfo.PropertyType.GetGenericArguments()[0].Name}>";
                }
            }

            return propertyInfo.PropertyType.Name;
        }

        return "";
    }
}