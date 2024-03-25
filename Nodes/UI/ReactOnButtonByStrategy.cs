using HECSFramework.Core;
using UnityEngine.UI;

namespace Strategies
{
    [Documentation(Doc.Strategy, "ReactOnButtonByStrategy")]
    public class ReactOnButtonByStrategy : InterDecision, IInitable
    {
        public override string TitleOfNode { get; } = "ReactOnButtonByStrategy";

        [Connection(ConnectionPointType.In, "<Button> In")]
        public GenericNode<Button> Button;

        [ExposeField]
        public BaseStrategy StrategyOnClick;

        protected override void Run(Entity entity)
        {
            Button.Value(entity).onClick.AddListener(() => StrategyOnClick.Execute(entity));
            Next.Execute(entity);
        }

        public void Init()
        {
            StrategyOnClick.Init();
        }
    }
}
