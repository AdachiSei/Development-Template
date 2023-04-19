using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(MultiPropertyBaseAttribute), true)]
public class MultiPropertyDrawerBase : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attributes = GetAttributes();
        var propertyDrawers = GetPropertyDrawers();

        // ”ñ•\Ž¦‚Ìê‡
        if (attributes.Any(attr => !attr.IsVisible(property))) return;

        // ‘Oˆ—
        foreach (var attr in attributes)
            attr.OnPreGUI(position, property);

        // •`‰æ
        using (var ccs = new EditorGUI.ChangeCheckScope())
        {
            if (propertyDrawers.Length == 0)
                EditorGUI.PropertyField(position, property, label);
            else
                propertyDrawers.LastOrDefault().OnGUI(position, property, label);

            // Œãˆ—
            foreach (var attr in attributes.Reverse())
                attr.OnPostGUI(position, property, ccs.changed);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attributes = GetAttributes();
        var propertyDrawers = GetPropertyDrawers();

        // ”ñ•\Ž¦‚Ìê‡
        if (attributes.Any(attr => !attr.IsVisible(property)))
            return -EditorGUIUtility.standardVerticalSpacing;

        var height = propertyDrawers.Length == 0
            ? base.GetPropertyHeight(property, label)
            : propertyDrawers.Last().GetPropertyHeight(property, label);

        return height;
    }

    private MultiPropertyBaseAttribute[] GetAttributes()
    {
        var attr = (MultiPropertyBaseAttribute)attribute;

        if (attr.Attributes == null)
        {
            attr.Attributes = fieldInfo
                .GetCustomAttributes(typeof(MultiPropertyBaseAttribute), false)
                .Cast<MultiPropertyBaseAttribute>()
                .OrderBy(x => x.order)
                .ToArray();
        }

        return attr.Attributes;
    }

    private IAttributePropertyDrawer[] GetPropertyDrawers()
    {
        var attr = (MultiPropertyBaseAttribute)attribute;

        if (attr.PropertyDrawers == null)
        {
            attr.PropertyDrawers = fieldInfo
                .GetCustomAttributes(typeof(MultiPropertyBaseAttribute), false)
                .OfType<IAttributePropertyDrawer>()
                .OrderBy(x => ((MultiPropertyBaseAttribute)x).order)
                .ToArray();
        }

        return attr.PropertyDrawers;
    }
}
