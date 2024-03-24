using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HECSFramework.Core;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

public class ReactiveStrategiesHelper : OdinEditorWindow
{
    [ValueDropdown(nameof(GetFilteredTypeList))]
    public Type CommandType;

    [MenuItem("HECS Options/Helpers/Strategies/ReactiveStrategiesHelper" , priority = 10)]
    public static void GetReactiveStrategiesHelperWindow()
    {
        GetWindow<ReactiveStrategiesHelper>();
    }

    public IEnumerable<Type> GetFilteredTypeList()
    {
        var q = typeof(ICommand).Assembly.GetTypes().Where(x=> x.IsCastableTo(typeof(ICommand)));
        return q;
    }

    [Button]
    public void GenerateLocalListeners()
    {
    }

    [Button]
    public void GenerateGlobalListeners()
    {

    }
}
