using HECSFramework.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "SetSpriteToImage")]
    public class SetImageSprite : InterDecision
    {
        [Connection(ConnectionPointType.In, "<Sprite> Sprite")]
        public GenericNode<Sprite> Sprite;

        [Connection(ConnectionPointType.In, "<Image> In")]
        public GenericNode<Image> Image;

        public override string TitleOfNode { get; } = "SetImageSprite";

        protected override void Run(Entity entity)
        {
            Image.Value(entity).sprite = Sprite.Value(entity);
            Next.Execute(entity);
        }
    }
}

