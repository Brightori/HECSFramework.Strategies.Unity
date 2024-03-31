using HECSFramework.Core;
using UnityEngine;

namespace Strategies
{
    [Documentation(Doc.Strategy, Doc.HECS, "SpriteNode")]
    public class SpriteNode : GenericNode<Sprite>
    {
        public override string TitleOfNode { get; } = "SpriteNode";

        [Connection(ConnectionPointType.Out, " <Sprite> Out")]
        public BaseDecisionNode Out;

        [ExposeField]
        public Sprite Sprite;

        public override void Execute(Entity entity)
        {
            throw new System.NotImplementedException();
        }

        public override Sprite Value(Entity entity)
        {
            return Sprite;
        }
    }
}
