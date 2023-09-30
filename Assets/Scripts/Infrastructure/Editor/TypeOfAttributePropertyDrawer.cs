using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TypeOfAttribute))]
public class TypeOfAttributePropertyDrawer : PropertyDrawer
{
    private List<Type> _knownDerivedType;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 2 * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        var typeOfAttribute = attribute as TypeOfAttribute;
        CollectSuitableTypes(typeOfAttribute.BaseType);

        var assemblyQualified = _knownDerivedType.Select(t => t.AssemblyQualifiedName).ToList();
        if (!assemblyQualified.Contains(property.stringValue)) assemblyQualified.Insert(1, property.stringValue);
        assemblyQualified.Insert(0, "Undefined");

        var nameIndex = assemblyQualified.IndexOf(property.stringValue);
        if (nameIndex < 0) nameIndex = 0;

        rect.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(rect, $"Select implementation of type: {typeOfAttribute.BaseType.AssemblyQualifiedName}");
        rect.y += EditorGUIUtility.singleLineHeight;

        //var shortNames = assemblyQualified.Select(SimplifyAssemblyQualifiedName).ToList();
        var changedNameIndex = EditorGUI.Popup(rect, nameIndex, assemblyQualified.ToArray());

        if (changedNameIndex != nameIndex)
        {
            property.stringValue = changedNameIndex > 0 ? assemblyQualified[changedNameIndex] : string.Empty;
        }
    }

    private void CollectSuitableTypes(Type baseType)
    {
        if (_knownDerivedType != null) return;
        
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        var candidates = assemblies
                .Where(assembly => !assembly.IsDynamic)
                .SelectMany(assembly => assembly.ExportedTypes, (assembly, type) => type)
                .Where(type => type.IsClass && !type.IsAbstract && baseType.IsAssignableFrom(type));

        _knownDerivedType = candidates.ToList();
    }

    private string SimplifyAssemblyQualifiedName(string assemblyQualifiedName)
    {
        var commaIndex = assemblyQualifiedName.IndexOf(',');
        if (commaIndex == -1) return assemblyQualifiedName;

        return assemblyQualifiedName.Substring(0, commaIndex);
    }
}