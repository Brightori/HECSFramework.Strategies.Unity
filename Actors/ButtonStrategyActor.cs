using Components;
using HECSFramework.Core;
using HECSFramework.Unity;
using Strategies;
using UnityEngine.UI;

public sealed class ButtonStrategyActor : Actor
{
    public BaseStrategy InitStrategy;
    public BaseStrategy OnClickStrategy;

    private Button button;

    protected override void Start()
    {
        Init();
        Entity.GetOrAddComponent<UIAccessProviderComponent>();
        OnClickStrategy.Init();
        InitStrategy?.Init();
        button = GetComponent<Button>();
        button.onClick.AddListener(()=> OnClickStrategy.Execute(Entity));

        InitStrategy?.Execute(Entity);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        button?.onClick.RemoveAllListeners();
    }
}
