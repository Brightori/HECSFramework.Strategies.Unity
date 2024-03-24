using HECSFramework.Core;
using Strategies;
using UnityEngine;

namespace Components
{
    public abstract class ExecuteStrategyByLocalCommand<T> : BaseComponent, IStrategyLocalExecutor<T> where T : struct, ICommand
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