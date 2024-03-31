using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HECSFramework.Core;
using HECSFramework.Core.Generator;
using HECSFramework.Unity.Helpers;
using HECSFramework.Unity;
using Sirenix.Utilities;
using Strategies;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Direction = UnityEditor.Experimental.GraphView.Direction;
using UnityEditor.Compilation;

public class DrawConstructorNodeViewGraph : Node
{
    public string GUID;
    public string Name;
    public BaseNodeConstruction InnerNode;
    public Dictionary<Port, (FieldInfo member, Direction direction, BaseNodeConstruction node)> ConnectedPorts = new Dictionary<Port, (FieldInfo member, Direction direction, BaseNodeConstruction node)>();

    public void ClearConnections()
    {
        foreach (var connection in ConnectedPorts)
        {
            if (connection.Value.member is FieldInfo fieldInfo)
            {
                fieldInfo.SetValue(InnerNode, null);
            }
        }

        InnerNode.ConnectionContexts.Clear();
    }
}

public class NodeConstructorGraphView : GraphView
{
    private readonly NodeConstructor strategy;
    private readonly string path;
    private readonly NodeConstructorGraphViewWIndow strategyGraphViewWIndow;
    private List<DrawConstructorNodeViewGraph> drawNodes = new List<DrawConstructorNodeViewGraph>(16);
    private NodeConstructorSearchWindow _searchWindow;
    public static List<CopyNode> copyNodes = new List<CopyNode>();
    private MouseUpEvent mouseData;
    private Vector2 mousePosition;
    private GridBackground grid;

    public NodeConstructorGraphView(NodeConstructor strategy, NodeConstructorGraphViewWIndow strategyGraphViewWIndow, string path)
    {
        this.path = path;

        styleSheets.Add(Resources.Load<StyleSheet>("StrategiesGraph"));

        SetupZoom(0.01f, 2f);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        this.AddManipulator(new FreehandSelector());

        grid = new GridBackground();
        Insert(0, grid);
        grid.StretchToParentSize();
        this.strategy = strategy;
        this.strategyGraphViewWIndow = strategyGraphViewWIndow;

        foreach (var dn in strategy.ConstructorNodes)
        {
            AddNodeToDecision(dn);
        }

        ConnectStrategyNodes();
        graphViewChanged += Changes;
        RegisterCallback<MouseMoveEvent>(MouseReact);
    }

    public void AddSearchWindow(NodeConstructorGraphViewWIndow editorWindow)
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeConstructorSearchWindow>();
        _searchWindow.Configure(editorWindow, this);
        nodeCreationRequest = context =>
            SearchWindow.Open(new SearchWindowContext(context.screenMousePosition, requestedWidth: 800), _searchWindow);
    }

    public (BaseNodeConstruction node, DrawConstructorNodeViewGraph drawNode) AddNodeToStrategyGraph(Vector2 position, Type type)
    {
        var newNode = OnClickAddNode(position, type);
        var drawNode = AddNodeToDecision(newNode);
        return (newNode, drawNode);
    }

    private BaseNodeConstruction OnClickAddNode(Vector2 mousePosition, Type type)
    {
        var asset = ScriptableObject.CreateInstance(type);
        var parent = AssetDatabase.LoadAssetAtPath<NodeConstructor>(path);
        asset.name = type.ToString();
        AssetDatabase.AddObjectToAsset(asset, parent);
        strategy.ConstructorNodes.Add(asset as BaseNodeConstruction);
        (asset as BaseNodeConstruction).coords = mousePosition;
        AssetDatabase.SaveAssets();
        return asset as BaseNodeConstruction;
    }

    private void ConnectStrategyNodes()
    {
        foreach (var dn in drawNodes)
        {
            foreach (var connect in dn.ConnectedPorts)
            {
                if (connect.Value.direction == Direction.Output)
                    continue;

                var value = connect.Value.member;
                var current = value.GetValue(dn.InnerNode) as BaseNodeConstruction;

                if (current == null)
                    continue;

                foreach (var dn2 in drawNodes)
                {
                    if (dn2.InnerNode == current)
                    {
                        var needed = dn2.ConnectedPorts.FirstOrDefault(x => x.Value.direction == Direction.Output
                            && (x.Value.member.GetValue(dn2.InnerNode) as BaseNodeConstruction) == dn.InnerNode);

                        if (needed.Value.node != null)
                        {
                            LinkNodesTogether(needed.Key, connect.Key);
                        }
                    }
                }
            }
        }
    }

    private void LinkNodesTogether(Port outputSocket, Port inputSocket)
    {
        var tempEdge = new Edge()
        {
            output = outputSocket,
            input = inputSocket
        };

        tempEdge?.input.Connect(tempEdge);
        tempEdge?.output.Connect(tempEdge);

        tempEdge?.styleSheets.Add(Resources.Load<StyleSheet>("StrategiesGraph"));
        this.Add(tempEdge);
    }

    private DrawConstructorNodeViewGraph AddNodeToDecision(BaseNodeConstruction node)
    {
        var drawNode = new DrawConstructorNodeViewGraph();

        drawNode.title = node.Name;
        drawNode.GUID = Guid.NewGuid().ToString();
        drawNode.InnerNode = node;

        var attrs = node.GetType().GetAttributes(true);
        var nodeType = attrs.FirstOrDefault(x => x is NodeTypeAttribite) as NodeTypeAttribite;

        drawNode.SetPosition(new Rect(node.coords, new Vector2(150, 50)));
        GeneratePorts(drawNode);
        GenerateCustomField(drawNode);
        AddElement(drawNode);
        drawNodes.Add(drawNode);
        return drawNode;
    }

    private void GeneratePorts(DrawConstructorNodeViewGraph drawNode)
    {
        var members = drawNode.InnerNode.GetType().GetMembers();

        foreach (var m in members)
        {
            if (m is FieldInfo field)
            {
                var atrs = field.GetCustomAttributes();

                foreach (var a in atrs)
                {
                    if (a is ConnectionAttribute connection)
                    {
                        switch (connection.ConnectionPointType)
                        {
                            case ConnectionPointType.In:
                                var port = GeneratePort(drawNode, Direction.Input, connection.NameOfField, Port.Capacity.Single);
                                drawNode.ConnectedPorts.Add(port, (field, Direction.Input, drawNode.InnerNode));
                                break;

                            case ConnectionPointType.Out:
                                var port2 = GeneratePort(drawNode, Direction.Output, connection.NameOfField, Port.Capacity.Single);
                                drawNode.ConnectedPorts.Add(port2, (field, Direction.Output, drawNode.InnerNode));
                                break;
                        }
                    }
                }
            }
        }
    }

    private void GenerateCustomField(DrawConstructorNodeViewGraph drawNode)
    {
        var so = new SerializedObject(drawNode.InnerNode);

        if (drawNode.InnerNode != null)
        {
            foreach (var m in drawNode.InnerNode.GetType().GetMembers())
            {
                foreach (var a in m.GetCustomAttributes())
                {
                    if (a is SerializeField || a is ExposeFieldAttribute)
                    {
                        var field = m as FieldInfo;

                        if (field.FieldType == typeof(int))
                        {
                            var intField = new IntegerField(m.Name + ":");
                            intField.value = (int)(m as FieldInfo).GetValue(drawNode.InnerNode);

                            intField.RegisterValueChangedCallback((evt) => UpdateField(evt, m as FieldInfo, drawNode.InnerNode));
                            drawNode.contentContainer.Add(intField);
                        }
                        else if (field.FieldType == typeof(float))
                        {
                            var floatField = new FloatField(m.Name + ":");
                            floatField.value = (float)field.GetValue(drawNode.InnerNode);
                            floatField.RegisterValueChangedCallback((evt) => UpdateField(evt, field, drawNode.InnerNode));
                            floatField.style.minWidth = 20;
                            drawNode.contentContainer.Add(floatField);
                        }
                        else if (field.FieldType == typeof(string))
                        {
                            var floatField = new TextField(m.Name + ":");
                            floatField.value = (string)field.GetValue(drawNode.InnerNode);
                            floatField.RegisterValueChangedCallback((evt) => UpdateField(evt, field, drawNode.InnerNode));
                            drawNode.contentContainer.Add(floatField);
                        }
                        else if (field.FieldType == typeof(Vector3))
                        {
                            var floatField = new Vector3Field(m.Name + ":");
                            floatField.value = (Vector3)field.GetValue(drawNode.InnerNode);
                            floatField.RegisterValueChangedCallback((evt) => UpdateField(evt, field, drawNode.InnerNode));
                            drawNode.contentContainer.Add(floatField);
                        }
                        else if (field.FieldType == typeof(bool))
                        {
                            var boolField = new Toggle(m.Name + ":");
                            boolField.value = (bool)field.GetValue(drawNode.InnerNode);
                            boolField.RegisterValueChangedCallback((evt) => UpdateField(evt, field, drawNode.InnerNode));
                            drawNode.contentContainer.Add(boolField);
                        }

                        else
                        {
                            var objectField = new ObjectField(m.Name + ":") { objectType = field.FieldType };
                            var value = (m as FieldInfo).GetValue(drawNode.InnerNode) as UnityEngine.Object;

                            objectField.value = value;
                            objectField.RegisterValueChangedCallback((evt) => UpdateField(evt, m as FieldInfo, drawNode.InnerNode));
                            drawNode.contentContainer.Add(objectField);
                        }

                        drawNode.RefreshPorts();
                        drawNode.RefreshExpandedState();
                    }

                    if (a is TypeConstructionField)
                    {
                        var field = m as FieldInfo;
                        var boolField = new TextField(field.Name + ":");
                        boolField.value = (string)field.GetValue(drawNode.InnerNode);
                        boolField.RegisterValueChangedCallback((evt) => UpdateField(evt, field, drawNode.InnerNode));
                        var button = new Button(() => ReactOnComponentSearchClick(field, drawNode, boolField)) { text = "SelectType" };
                        boolField.contentContainer.Add(button);
                        drawNode.contentContainer.Add(boolField);
                    }
                }
            }
        }
    }

    private void IdentifierDropDownReact(ChangeEvent<string> evt, FieldInfo field, BaseNodeConstruction innerNode)
    {
        var identifier = IndexGenerator.GenerateIndex(evt.newValue);
        field.SetValue(innerNode, identifier);
    }

    private void ReactOnComponentSearchClick(FieldInfo dropDown, DrawConstructorNodeViewGraph node, TextField textField)
    {
        var winow = EditorWindow.GetWindow<SearchTypeWindow>();
        winow.Init(dropDown, node, textField);
    }

    private void UpdateField<T>(ChangeEvent<T> evt, FieldInfo field, BaseNodeConstruction innerNode)
    {
        field.SetValue(innerNode, evt.newValue);
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();
        var startPortView = startPort;

        ports.ForEach((port) =>
        {
            var portView = port;
            if (startPortView != portView && startPortView.node != portView.node)
                compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }


    private Port GeneratePort(DrawConstructorNodeViewGraph node, Direction direction, string portName, Port.Capacity capacity = Port.Capacity.Single)
    {
        var port = node.InstantiatePort(Orientation.Horizontal, direction, capacity, typeof(BaseNodeConstruction));
        //var port = LocalPort.Create<Edge>(Orientation.Horizontal, direction, Port.Capacity.Single, typeof(ConstructorNode));
        port.portName = portName;
        node.outputContainer.Add(port);

        node.RefreshExpandedState();
        node.RefreshPorts();
        return port;
    }

    private void MouseReact(MouseMoveEvent evt)
    {
        mousePosition = evt.localMousePosition;
    }

    private DrawConstructorNodeViewGraph NeededNode(Port port, Direction direction)
    {
        foreach (var dn in drawNodes)
        {
            foreach (var portInfo in dn.ConnectedPorts)
            {
                if (portInfo.Key == port && portInfo.Value.direction == direction)
                    return dn;
            }
        }

        return null;
    }

    private GraphViewChange Changes(GraphViewChange graphViewChange)
    {
        //moving and update coords of node
        if (graphViewChange.movedElements != null)
        {
            foreach (var moved in graphViewChange.movedElements)
            {
                if (moved is DrawConstructorNodeViewGraph drawNode)
                    drawNode.InnerNode.coords = drawNode.GetPosition().position;
            }
        }

        //create connection
        if (graphViewChange.edgesToCreate != null)
        {
            foreach (var e in graphViewChange.edgesToCreate)
            {
                var input = NeededNode(e.input, Direction.Input);
                var output = NeededNode(e.output, Direction.Output);

                if (input != null && output != null)
                {
                    ((FieldInfo)output.ConnectedPorts[e.output].member).SetValue(output.InnerNode, input.InnerNode);
                    ((FieldInfo)input.ConnectedPorts[e.input].member).SetValue(input.InnerNode, output.InnerNode);
                }
            }
        }

        //clean connection
        if (graphViewChange.elementsToRemove != null)
        {
            foreach (var remove in graphViewChange.elementsToRemove)
            {
                if (remove is Edge edge)
                {
                    foreach (var dn in drawNodes)
                    {
                        if (dn.ConnectedPorts.TryGetValue(edge.input, out var info))
                        {
                            var field = info.member;
                            var nameOut = info.member.Name;

                            if (field == null)
                                continue;

                            var valueInConnection = field.GetValue(dn.InnerNode) as BaseNodeConstruction;
                            field.SetValue(dn.InnerNode, null);

                            if (valueInConnection == null)
                                continue;

                            foreach (var dnOut in drawNodes)
                            {
                                if (dnOut.InnerNode == valueInConnection)
                                {
                                    dnOut.InnerNode.ConnectionContexts.Remove(new ConnectionContext { Out = dnOut.ConnectedPorts[edge.output].member.Name, In = dn.ConnectedPorts[edge.input].member.Name });
                                }
                            }
                        }
                    }
                }

                if (remove is DrawConstructorNodeViewGraph node)
                {
                    strategy.ConstructorNodes.Remove(node.InnerNode);
                    RemoveNode(node.InnerNode);
                }
            }
        }

        return graphViewChange;
    }

    private void RemoveNode(BaseNodeConstruction innerNode)
    {
        var path = AssetDatabase.GetAssetPath(strategy);
        var allSo = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);

        foreach (var go in allSo)
        {
            if (go == innerNode)
            {
                AssetDatabase.RemoveObjectFromAsset(go);
                MonoBehaviour.DestroyImmediate(go);
            }
        }

        AssetDatabase.SaveAssets();
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        base.BuildContextualMenu(evt);
        evt.menu.InsertAction(1, "Duplicate", (x) => DuplicateNode());
        //evt.menu.InsertAction(2, "Copy", (x) => CopyNodes());
        //evt.menu.InsertAction(3, "Paste Nodes", (x) => PasteNode());
    }

    protected void DuplicateNode()
    {
        var newSelection = new List<ISelectable>();

        foreach (var node in selection)
        {
            if (node is DrawConstructorNodeViewGraph drawNode)
            {
                var toJSON = JsonUtility.ToJson(drawNode.InnerNode);
                var newNode = AddNodeToStrategyGraph(drawNode.GetPosition().center + new Vector2(2, 0), drawNode.InnerNode.GetType());
                JsonUtility.FromJsonOverwrite(toJSON, newNode.node);
                newSelection.Add(newNode.drawNode);
                newNode.drawNode.ClearConnections();
            }
        }

        ClearSelection();

        foreach (var n in newSelection)
            AddToSelection(n);
    }
}