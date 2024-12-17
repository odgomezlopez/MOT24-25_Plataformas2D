using UnityEditor;
using UnityEngine;
using System.Reflection;

[CustomPropertyDrawer(typeof(ExpandableAttribute))]
public class ExpandableAttributeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PropertyField(position, property, label, true);

        if (property.objectReferenceValue != null)
        {
            var obj = property.objectReferenceValue as ScriptableObject;

            property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, GUIContent.none);
            if (property.isExpanded)
            {
                using (var changeCheck = new EditorGUI.ChangeCheckScope())
                {
                    Editor editor = Editor.CreateEditor(obj);
                    if (editor != null)
                    {
                        EditorGUI.indentLevel++;
                        editor.OnInspectorGUI();
                        EditorGUI.indentLevel--;

                        if (changeCheck.changed)
                        {
                            obj.name = obj.name; // Force Unity to detect a change in the ScriptableObject.

                            // Notify the MonoBehaviour of the change using reflection
                            var monoBehaviour = property.serializedObject.targetObject as MonoBehaviour;
                            if (monoBehaviour != null)
                            {
                                var onValidateMethod = monoBehaviour.GetType().GetMethod("OnValidate", BindingFlags.NonPublic | BindingFlags.Instance);
                                if (onValidateMethod != null)
                                {
                                    onValidateMethod.Invoke(monoBehaviour, null);
                                }

                                // Mark the MonoBehaviour as dirty to ensure changes are saved
                                EditorUtility.SetDirty(monoBehaviour);
                            }
                        }
                    }
                }
            }
        }
    }
}
