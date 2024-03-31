using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Strategies;
using System;
using HECSFramework.Unity;

[InitializeOnLoad]
public class NodeConstructorGraphViewWIndow : EditorWindow
{
    private NodeConstructorGraphView graphView;
    private NodeConstructor strategy;
    private string assetPath;
    private NodeConstructorSearchWindow _searchWindow;

    private static void ShowWindowReact(BaseStrategy strategy, string path)
    {
        var window = CreateWindow<StrategyGraphViewWIndow>();
        window.titleContent = new GUIContent(strategy.name);
        window.OnInit(strategy, path);
    }

    internal void OnInit(NodeConstructor strategy)
    {
        this.strategy = strategy;

        AssetDatabase.GetAssetPath(strategy);

        graphView = new NodeConstructorGraphView(strategy, this, AssetDatabase.GetAssetPath(strategy));
        graphView.StretchToParentSize();
        rootVisualElement.Add(graphView);
        CreateMiniMap();
        graphView.AddSearchWindow(this);
        graphView.FrameAll();
    }

    private void CreateMiniMap()
    {
        var minimap = new MiniMap() { anchored = true };
        minimap.SetPosition(new Rect(10, 10, 200, 140));

        graphView.Add(minimap);
    }

    private void OnDisable()
    {
        if (!strategy)
            return;

        if (graphView != null)
        {
            rootVisualElement.Remove(graphView);
            //graphView.Dispose();
        }

        EditorUtility.SetDirty(strategy);
        AssetDatabase.SaveAssets();
    }
}