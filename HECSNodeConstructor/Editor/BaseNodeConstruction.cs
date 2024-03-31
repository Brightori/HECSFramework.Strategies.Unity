using System;
using System.Collections.Generic;
using HECSFramework.Core;
using HECSFramework.Core.Generator;
using Strategies;
using UnityEngine;

[Serializable]
public abstract class BaseNodeConstruction : ScriptableObject
{
    [Connection(ConnectionPointType.Out, "Out")]
    public BaseNodeConstruction NextNode;
    public abstract ISyntax GetSyntax();
    public abstract string Name { get; }
    public Vector2 coords;
    [IgnoreDraw]
    [HideInInspectorCrossPlatform]
    public List<ConnectionContext> ConnectionContexts = new List<ConnectionContext>();
}