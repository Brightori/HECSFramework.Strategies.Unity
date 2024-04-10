using System;
using HECSFramework.Core;
using Strategies;
using Systems;

namespace Components
{
    [RequiredAtContainer(typeof(ExecuteStrategiesByCoroutineSystem))]
    [Serializable][Documentation(Doc.HECS, Doc.Strategy, Doc.Holder, "this component holds stragies for ExecuteStrategiesByCoroutineSystem")]
    public sealed class StrategiesForCoroutineUpdateHolder : BaseComponent
    {
        public BaseStrategy[] Strategies;
        public float UpdatePeriod;
    }
}