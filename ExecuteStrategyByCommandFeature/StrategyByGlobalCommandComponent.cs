using HECSFramework.Core;
using Strategies;
using UnityEngine;

namespace Components
{
    public abstract class StrategyByGlobalCommandComponent<T> : BaseComponent, IStrategyGlobalExecutor<T> where T : struct, IGlobalCommand
    {
        [SerializeField] BaseStrategy strategy;

        public BaseStrategy BaseStrategy => strategy;

        public T Value { get; set; }

        public override void Init()
        {
            strategy.Init();
        }
    }
}