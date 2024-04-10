using System;
using Components;
using HECSFramework.Core;
using HECSFramework.Unity;
using UnityEngine;

namespace Systems
{
    [Serializable][Documentation(Doc.HECS, Doc.Strategy, "this system execute strategies from StrategiesForCoroutineUpdateHolder by custom update functionality")]
    public sealed class ExecuteStrategiesByCoroutineSystem : BaseSystem, ICustomUpdatable
    {
        [Required]
        public StrategiesForCoroutineUpdateHolder StrategiesForCoroutineUpdateHolder;

        public YieldInstruction Interval { get; private set; }

        public override void InitSystem()
        {
            Interval = new WaitForSeconds(StrategiesForCoroutineUpdateHolder.UpdatePeriod);

            foreach (var strategy in StrategiesForCoroutineUpdateHolder.Strategies)
                strategy.Init();
        }

        public void UpdateCustom()
        {
            foreach (var strategy in StrategiesForCoroutineUpdateHolder.Strategies)
                strategy.Execute(Owner);
        }
    }
}