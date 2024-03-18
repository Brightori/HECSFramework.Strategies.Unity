using HECSFramework.Core;
using HECSFramework.Unity.Helpers;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Linq;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace Strategies
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public abstract class BaseDecisionNode : ScriptableObject, IDecisionNode
    {
        public abstract string TitleOfNode { get; }
        [IgnoreDraw] public Vector2 coords;

        [IgnoreDraw]
        [HideInInspectorCrossPlatform]
        public List<ConnectionContext> ConnectionContexts = new List<ConnectionContext>();

        public abstract void Execute(Entity entity);

        [Button]
        public void OpenAndShowNode()
        {
            var strategies = new SOProvider<BaseStrategy>().GetCollection().ToArray();

            for (int i = 0; i < strategies.Length; i++)
            {
                if (strategies[i].nodes.Contains(this))
                {
#if UNITY_EDITOR
                    strategies[i].OpenStrategy(coords);
#endif
                }
            }
        }
    }
}