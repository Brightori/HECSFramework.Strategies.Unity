using Components;
using HECSFramework.Core;
using HECSFramework.Unity;
using Strategies;
using UnityEngine.UI;

public sealed class ButtonStrategyActor : Actor
{
    public BaseStrategy strategy;

    private Button button;

    protected override void Start()
    {
        Init();
        Entity.GetOrAddComponent<UIAccessProviderComponent>();
        strategy.Init();
        button = GetComponent<Button>();
        button.onClick.AddListener(()=> strategy.Execute(Entity));
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        button?.onClick.RemoveAllListeners();
    }
}
