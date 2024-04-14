using HECSFramework.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Strategies
{
    [Documentation(Doc.Strategy, "EnableDisableGameObjectNode")]
    public class EnableDisableGameObjectNode : InterDecision
    {
        [Connection(ConnectionPointType.In, "<GameObject>")]
        public GenericNode<GameObject> In;

        [ExposeField]
        public bool Enable = true;

        public override string TitleOfNode { get; } = "EnableDisableGameObjectNode";

        protected override void Run(Entity entity)
        {
            In.Value(entity).gameObject.SetActive(Enable);
            Next.Execute(entity);
        }
    }
}
