using HECSFramework.Core;
using TMPro;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "SetTextMeshPro")]
    public class SetTextMeshPro : InterDecision
    {
        [Connection(ConnectionPointType.In, "<string> Value")]
        public GenericNode<string> Text;

        [Connection(ConnectionPointType.In, "<TextMeshProGui> In")]
        public GenericNode<TextMeshProUGUI> TextMeshPro;

        public override string TitleOfNode { get; } = "SetTextMeshPro";

        protected override void Run(Entity entity)
        {
            TextMeshPro.Value(entity).text = Text.Value(entity);
            Next.Execute(entity);
        }
    }
}
