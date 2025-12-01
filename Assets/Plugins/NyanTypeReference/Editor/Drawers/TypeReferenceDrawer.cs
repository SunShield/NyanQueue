using System;
using System.Reflection;
using NyanTypeReference.Attribs;
using NyanTypeReference.Editor.Windows;
using UnityEditor;
using UnityEngine;

namespace NyanTypeReference.Editor.Drawers
{
    [CustomPropertyDrawer(typeof(TypeReference))]
    public class TypeReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty typeNameProp = property.FindPropertyRelative("_typeName");

            Type currentType = null;
            if (!string.IsNullOrEmpty(typeNameProp.stringValue))
                currentType = System.Type.GetType(typeNameProp.stringValue);

            var options = fieldInfo.GetCustomAttribute<TypeOptionsAttribute>();

            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var display = currentType != null ? currentType.FullName : "(None)";

            if (GUI.Button(position, display, EditorStyles.popup))
            {
                TypePickerWindow.Show(
                    position,
                    currentType,
                    t =>
                    {
                        typeNameProp.stringValue = t?.AssemblyQualifiedName;
                        property.serializedObject.ApplyModifiedProperties();
                    },
                    options
                );
            }

            EditorGUI.EndProperty();
        }
    }
}