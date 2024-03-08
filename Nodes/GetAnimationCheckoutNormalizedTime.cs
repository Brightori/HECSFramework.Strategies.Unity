using Components;
using HECSFramework.Core;using HECSFramework.Unity;
using UnityEngine;

namespace Strategies{	[Documentation(Doc.Strategy, "this node provide normalized timing from AnimationCheckOutsHolderComponent")]	public class GetAnimationCheckoutNormalizedTime : GenericNode<float>	{
        [Connection(ConnectionPointType.In, "<Entity> Animator Owner")]
        public GenericNode<Entity> AnimatorOwner;

        [ExposeField]		public AnimationEventIdentifier AnimationEventID;				[Connection(ConnectionPointType.Out, "<float> Out")]		public BaseDecisionNode Out;				public override string TitleOfNode { get; } = "GetAnimationCheckoutNormalizedTime";				public override void Execute(Entity entity)		{		}				public override float Value(Entity entity)		{			var needed = AnimatorOwner != null ? AnimatorOwner.Value(entity) : entity;			if (needed.TryGetComponent(out AnimationCheckOutsHolderComponent animationCheckOutsHolderComponent))
			{
				if (animationCheckOutsHolderComponent.TryGetCheckoutInfo(AnimationEventID, out var checkoutInfo)) 
				{
					return checkoutInfo.TimingNormalized;
				}
			}			Debug.LogWarning("we dont have checkout holder on " + entity.ContainerID);			return 0;		}	}}