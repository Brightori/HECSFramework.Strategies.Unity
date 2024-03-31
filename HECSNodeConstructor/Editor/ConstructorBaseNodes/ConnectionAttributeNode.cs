﻿using HECSFramework.Core.Generator;using Strategies;public  class ConnectionAttributeNode : BaseNodeConstruction{	public override string Name { get; }  = "ConnectionAttributeNode";	[Connection(ConnectionPointType.In, "FieldName")]	public BaseNodeConstruction FieldName;	[ExposeField]	public int Step;	public override ISyntax GetSyntax()	{		return new TabSimpleSyntax(Step, $"[Connection(ConnectionPointType.In, {FieldName.ToString()})]");	}}