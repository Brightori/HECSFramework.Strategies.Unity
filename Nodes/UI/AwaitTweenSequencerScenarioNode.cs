using HECSFramework.Core;
using HECSFramework.Unity;


namespace Strategies
{
    [Documentation(Doc.Strategy, "AwaitTweenSequencerScenarioNode")]
    public class AwaitTweenSequencerScenarioNode : InterDecision
    {
        [Connection(ConnectionPointType.In, "<TweenSequencer>")]
        public GenericNode<TweenSequencer> TweenSequencer;

        [ExposeField]
        public ActionIdentifier ActionIdentifier;


        public override string TitleOfNode { get; } = "AwaitTweenSequencerScenarioNode";

        protected override async void Run(Entity entity)
        {
            var generation = 0;
            var aliveEntity = entity.GetAliveEntity();

            if (entity.TryGetComponent(out Components.StateContextComponent stateContextComponent))
            {
                generation = stateContextComponent.CurrentIteration;
                stateContextComponent.StrategyState = StrategyState.Pause;
            }

            await TweenSequencer.Value(entity).PlayAsync(ActionIdentifier);

            if (!aliveEntity.IsAlive)
                return;

            if (stateContextComponent != null)
            {
                if (generation != stateContextComponent.CurrentIteration)
                    return;

                stateContextComponent.StrategyState = StrategyState.Run;
            }

            Next.Execute(entity);
        }
    }
}
