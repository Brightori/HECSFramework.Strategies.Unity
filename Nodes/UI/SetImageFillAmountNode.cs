using HECSFramework.Core;
using UnityEngine.UI;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "SetImageFillAmountNode")]
    public class SetImageFillAmountNode : InterDecision
    {
        [Connection(ConnectionPointType.In, "<float> Progress")]
        public GenericNode<float> Progress;
        [Connection(ConnectionPointType.In, "<Image> Image")]
        public GenericNode<Image> Image;
        public override string TitleOfNode { get; } = "Set Image Fill Amount";
        protected override void Run(Entity entity)
        {
            Image.Value(entity).fillAmount = Progress.Value(entity);
            Next.Execute(entity);
        }
    }
}
