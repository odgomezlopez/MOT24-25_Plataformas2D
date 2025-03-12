using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalHidePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);

        if (sourcePropertyValue != null && sourcePropertyValue.boolValue)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute condHAtt = (ConditionalHideAttribute)attribute;
        SerializedProperty sourcePropertyValue = property.serializedObject.FindProperty(condHAtt.ConditionalSourceField);

        return (sourcePropertyValue != null && sourcePropertyValue.boolValue) ? EditorGUI.GetPropertyHeight(property, label) : 0f;
    }
}