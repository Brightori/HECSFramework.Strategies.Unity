using Components;
using HECSFramework.Core;

namespace Strategies
{
    public class GetCurrentAnimationNormalizedTimeNode : GenericNode<float>
    {
        public override string TitleOfNode { get; } = "GetCurrentAnimationNormalizedTimeNode";

        [Connection(ConnectionPointType.In, "<Entity> Animator Owner")]
        public GenericNode<Entity> AnimatorOwner;

        [Connection(ConnectionPointType.Out, "<float>")]
        public BaseDecisionNode Out;

        [ExposeField]
        public int AnimationLayer;

        public override void Execute(Entity entity)
        {
        }

        public override float Value(Entity entity)
        {
            var check = AnimatorOwner == null ? entity : AnimatorOwner.Value(entity);

            if (check.TryGetComponent(out AnimatorStateComponent animatorStateComponent))
            {
                var currentState = animatorStateComponent.Animator.GetCurrentAnimatorStateInfo(AnimationLayer);
                return currentState.normalizedTime;
            }
            else
            {
                HECSDebug.LogWarning($"{entity.ID} {entity.GUID} doent have animator state component");
                return 0;
            }
        }
    }
}
