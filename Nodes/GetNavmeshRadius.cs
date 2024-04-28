﻿using Components;
using HECSFramework.Core;namespace Strategies{
    [Documentation(Doc.Strategy, Doc.HECS, "this node return radius of navmesh agent")]	public class GetNavmeshRadius : GenericNode<float>	{		[Connection(ConnectionPointType.In, "<Entity> AdditionalEntity")]		public GenericNode<Entity> AdditionalEntity;		[Connection(ConnectionPointType.Out, "<float> Out")]		public BaseDecisionNode Out;		public override string TitleOfNode { get; } = "GetNavmeshRadius";		public override void Execute(Entity entity)		{		}		public override float Value(Entity entity)		{			var needed = AdditionalEntity != null ? AdditionalEntity.Value(entity) : entity;			return needed.GetComponent<NavMeshAgentComponent>().Radius;		}	}}