using System;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine.UIElements;

namespace Strategies
{
    public class SearchTypeWindow : OdinEditorWindow
    {
        [OnValueChanged(nameof(SetName))]
        public Type drawChoices;

        [NonSerialized]
        public DrawConstructorNodeViewGraph node;

        [NonSerialized]
        public FieldInfo dropdownField;

        [NonSerialized]
        public TextField textField;

        public void Init(FieldInfo dropdownField, DrawConstructorNodeViewGraph drawConstructorNodeViewGraph, UnityEngine.UIElements.TextField textField)
        {
            this.dropdownField = dropdownField;
            this.node = drawConstructorNodeViewGraph;
            this.textField = textField;
        }

        public void SetName()
        {
            dropdownField.SetValue(node.InnerNode, drawChoices.Name);
            textField.value = drawChoices.Name;
        }
    }
}
