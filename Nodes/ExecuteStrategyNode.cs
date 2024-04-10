using HECSFramework.Core;


namespace Strategies
{
    [Documentation(Doc.Strategy, "here we execute strategy, be carefull, child strategy can change behaviour, u should check side effects manualy")]
    public sealed class ExecuteStrategyNode : InterDecision, IInitable
    {
        public override string TitleOfNode { get; } = "ExecuteStrategyNode";

        [ExposeField]
        public BaseStrategy BaseStrategy;

        protected override void Run(Entity entity)
        {
            BaseStrategy.Execute(entity);
            Next.Execute(entity);
        }

        public void Init()
        {
            BaseStrategy.Init();
        }
    }
}
