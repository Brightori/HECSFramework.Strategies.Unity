using HECSFramework.Core;
using HECSFramework.Unity;

namespace Strategies
{
    [Documentation(Doc.Strategy, "InitActorNode")]
    public class InitActorNode : InterDecision
    {
        [Connection(ConnectionPointType.In, "<Entity> Entity of Actor")]
        public GenericNode<Actor> Actor;

        [ExposeField]
        public bool InitWithContainer = true;

        public override string TitleOfNode { get; } = "InitActorNode";

        protected override void Run(Entity entity)
        {
            var needed = Actor.Value(entity);

            if (needed != null)
            {
                if (!needed.IsInited)
                {
                    if (InitWithContainer)
                        needed.InitWithContainer();
                    else
                        needed.Init();
                }
            }

            Next.Execute(entity);
        }
    }
}
