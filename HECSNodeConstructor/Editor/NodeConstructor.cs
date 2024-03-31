using System.Collections.Generic;
using System.Linq;
using HECSFramework.Unity.Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeConstructor", menuName = "BluePrints/NodeConstructor")]
public class NodeConstructor : SerializedScriptableObject
{
    public const string Alchemy = "/ConstructedNodes/Editor/";
    public string Path = InstallHECS.ScriptPath + InstallHECS.HECSGenerated + Alchemy;

    public List<BaseNodeConstruction> ConstructorNodes = new List<BaseNodeConstruction>();

    [Button]
    public void Open()
    {
        EditorWindow.GetWindow<NodeConstructorGraphViewWIndow>().OnInit(this);
    }

    [Button]
    public void Generate()
    {
        var gatherNode = ConstructorNodes.FirstOrDefault(x => x is GatherNode);
        var gatherStrategyNode = ConstructorNodes.FirstOrDefault(x => x is GatherStrategyNode);

        if (gatherNode != null) 
        { 
            var data =  gatherNode as GatherNode;
            InstallHECS.CheckFolder(Path);
            InstallHECS.SaveToFile(data.GetSyntax().ToString(), Path + data.FileName.GetSyntax().ToString());
        }
        else
        {
            Debug.LogError("we dont have gather node");
        }
    }

    [Button]
    public void OpenMethodsHelper()
    {
        EditorWindow.GetWindow<GetMethodNameWindow>();
    }
}
