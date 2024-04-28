using Components;
using HECSFramework.Core;namespace Strategies{
    [Documentation(Doc.Strategy, Doc.HECS, "SetNavmeshStopingDistance")]
    public class SetNavmeshStopingDistance : InterDecision
    {
        [Connection(ConnectionPointType.In, "<float> SetDistance")]
        public GenericNode<float> SetDistance;
        public override string TitleOfNode { get; } = "SetNavmeshStopingDistance";
        protected override void Run(Entity entity)
        {
            entity.GetComponent<NavMeshAgentComponent>().NavMeshAgent.stoppingDistance = SetDistance.Value(entity);
            Next.Execute(entity);
        }
    }}