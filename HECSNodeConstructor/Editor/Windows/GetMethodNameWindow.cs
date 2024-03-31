using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

internal class GetMethodNameWindow : OdinEditorWindow
{
    public Type Type;
    
    [ValueDropdown(nameof(Methods))]
    public MethodInfo Method;
    
    [ValueDropdown(nameof(Fields))]
    public FieldInfo Field;

    public IEnumerable<FieldInfo> Fields()
    {
        if (Type == null)
            return Enumerable.Empty<FieldInfo>();

        return Type.GetFields();
    }

    public IEnumerable<MethodInfo> Methods()
    {
        if (Type == null) 
            return Enumerable.Empty<MethodInfo>();

        return Type.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
    }

    [Button]
    public void CopyMethod()
    {
        GUIUtility.systemCopyBuffer = Method?.Name;
    }

    [Button]
    public void CopyField()
    {
        GUIUtility.systemCopyBuffer = Field?.Name;
    }
}