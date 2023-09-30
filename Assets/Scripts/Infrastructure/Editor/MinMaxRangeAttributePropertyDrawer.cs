using Infrastructure;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MinMaxRangeAttribute))]
public class MinMaxRangeAttributePropertyDrawer : PropertyDrawer
{
    private const float Indent = 10;
    private const int LabelWidth = 60;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 2 * EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var rangeAttribute = attribute as MinMaxRangeAttribute;

        var objectType = property.boxedValue?.GetType();

        var isIntObject = objectType == typeof(MinMaxIntRange);
        var isSuitableType = isIntObject || objectType == typeof(MinMaxFloatRange);
        if (!isSuitableType)
        {
            GUI.Label(position, $"{property.displayName}: Compatible with MinMaxIntRange or MinMaxFloatRange only!");
            return;
        }

        MinMaxSliderVariant(position, property, label, isIntObject, rangeAttribute);
        //SeparateSliderVariant(position, property, label, isIntObject, rangeAttribute);
    }

    private static void MinMaxSliderVariant(Rect position, SerializedProperty property, GUIContent label, bool isIntObject, MinMaxRangeAttribute range)
    {
        var linePosition = position;
        linePosition.height = EditorGUIUtility.singleLineHeight;

        var minProperty = property.FindPropertyRelative("Min");
        var maxProperty = property.FindPropertyRelative("Max");

        var sliderPosition = position;
        sliderPosition.height = EditorGUIUtility.singleLineHeight;

        float min, max;
        if (isIntObject)
        {
            min = minProperty.intValue;
            max = maxProperty.intValue;
            EditorGUI.MinMaxSlider(sliderPosition, label, ref min, ref max, Mathf.CeilToInt(range.Min), Mathf.FloorToInt(range.Max));
            // for sure, because editor sometimes not set correct data if slider not moved
            min = min.ClampMin(range.Min);
            max = max.ClampMax(range.Max);
            minProperty.intValue = (int) min;
            maxProperty.intValue = (int) max;
        }
        else
        {
            min = minProperty.floatValue;
            max = maxProperty.floatValue;
            EditorGUI.MinMaxSlider(sliderPosition, label, ref min, ref max, range.Min, range.Max);
            // for sure, because editor sometimes not set correct data if slider not moved
            min = min.ClampMin(range.Min);
            max = max.ClampMax(range.Max);
            minProperty.floatValue = min;
            maxProperty.floatValue = max;
        }

        linePosition.y += EditorGUIUtility.singleLineHeight;

        var indent = EditorGUIUtility.labelWidth;
        linePosition.x += indent;
        linePosition.width -= indent;
        if (isIntObject)
        {
            GUI.Label(linePosition, $"Min: {minProperty.intValue}, Max: {maxProperty.intValue}");
        }
        else
        {
            GUI.Label(linePosition, $"Min: {minProperty.floatValue:0.##}, Max: {maxProperty.floatValue:0.##}");
        }
    }

    

    #region Separate slider variant
    private static void SeparateSliderVariant(Rect position, SerializedProperty property, GUIContent label, bool isIntObject, MinMaxRangeAttribute rangeAttribute)
    {
        throw new System.NotImplementedException();
    }

    private static SerializedProperty DrawSubProperty(Rect linePosition, SerializedProperty property,
        string displayName, string fieldName,
        MinMaxRangeAttribute range)
    {
        var labelPosition = linePosition;
        labelPosition.width = LabelWidth;
        GUI.Label(labelPosition, displayName);

        var sliderPosition = linePosition;
        sliderPosition.x += LabelWidth;
        sliderPosition.width -= LabelWidth;

        var subProperty = property.FindPropertyRelative(fieldName);
        switch (subProperty.propertyType)
        {
            case SerializedPropertyType.Integer:
                float min = property.FindPropertyRelative("Min").intValue;
                float max = property.FindPropertyRelative("Max").intValue;
                EditorGUI.MinMaxSlider(sliderPosition, displayName, ref min, ref max, Mathf.CeilToInt(range.Min), Mathf.FloorToInt(range.Max));
                property.FindPropertyRelative("Min").intValue = (int) min;
                property.FindPropertyRelative("Max").intValue = (int) max;
                //subProperty.intValue = EditorGUI.IntSlider(sliderPosition, subProperty.intValue, Mathf.CeilToInt(range.Min), Mathf.FloorToInt(range.Max));
                break;
            case SerializedPropertyType.Float:
                subProperty.floatValue = EditorGUI.Slider(sliderPosition, subProperty.floatValue,
                    range.Min, range.Max);
                break;
            default:
                GUI.Label(sliderPosition, "Not supported");
                break;
        }

        return subProperty;
    }
    #endregion
}