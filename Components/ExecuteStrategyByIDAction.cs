using System;
using HECSFramework.Core;

namespace Components
{
    [Serializable]
    [Documentation(Doc.Action, Doc.Strategy, Doc.HECS,  "this action execute strategy by id, and use " + nameof(StrategiesToIdentifierComponent))]
    public sealed class ExecuteStrategyByIDAction : IAction
    {
        public StrategyIdentifier StrategyIdentifier;

        public void Action(Entity entity)
        {
            if (entity.TryGetComponent(out StrategiesToIdentifierComponent strategiesToIdentifierComponent))
            {
                strategiesToIdentifierComponent.ExecuteStrategy(StrategyIdentifier);
            }
            else
                HECSDebug.LogError($"we dont have need component {nameof(StrategiesToIdentifierComponent)} on {entity.ID}");
        }
    }
}