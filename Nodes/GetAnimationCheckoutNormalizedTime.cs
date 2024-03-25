﻿using Components;
using HECSFramework.Core;
using UnityEngine;

namespace Strategies
        [Connection(ConnectionPointType.In, "<Entity> Animator Owner")]
        public GenericNode<Entity> AnimatorOwner;

        [ExposeField]
			{
				if (animationCheckOutsHolderComponent.TryGetCheckoutInfo(AnimationEventID, out var checkoutInfo)) 
				{
					return checkoutInfo.TimingNormalized;
				}
			}