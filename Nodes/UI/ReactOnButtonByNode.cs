using HECSFramework.Core;
using UnityEngine.UI;

namespace Strategies
{
    [Documentation(Doc.Strategy, "SetReactOnButtonByNode")]
    public sealed class ReactOnButtonByNode : InterDecision
    {
        public override string TitleOfNode { get; } = "SetReactOnButtonByNode";

        [Connection(ConnectionPointType.In, "<Button> In")]
        public GenericNode<Button> Button;

        [Connection(ConnectionPointType.Out, "<Node> OnButtonReact")]
        public BaseDecisionNode ReactionNode;

        protected override void Run(Entity entity)
        {
            Button.Value(entity).onClick.AddListener(() => ReactionNode.Execute(entity));
            Next.Execute(entity);
        }
    }
}
