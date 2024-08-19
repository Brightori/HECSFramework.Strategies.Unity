using System;
using HECSFramework.Core;
using Strategies;

namespace Components
{
    [Serializable][Documentation(Doc.Strategy, Doc.HECS, "we use this component for holding many strategies and execute them by id")]
    public sealed class StrategiesToIdentifierComponent : BaseComponent
    {
        public StrategyToIdentifier[] StrategiesToIdentifiers;

        public void ExecuteStrategy(int id, Entity entity = null)
        {
            var target = entity != null ? entity : Owner; 

            foreach (var s  in StrategiesToIdentifiers) 
            { 
                if (s.StrategyIdentifier == id)
                    s.Strategy.Execute(target);
            }
        }
    }

    [Serializable]
    public struct StrategyToIdentifier
    {
        public StrategyIdentifier StrategyIdentifier;
        public BaseStrategy Strategy;
    }
}