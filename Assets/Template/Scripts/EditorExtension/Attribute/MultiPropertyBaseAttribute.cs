using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class MultiPropertyBaseAttribute : PropertyAttribute
{
    public MultiPropertyBaseAttribute[] Attributes { get; private set; }
    public IAttributePropertyDrawer[] PropertyDrawers { get; private set; }

    #if UNITY_EDITOR

    public abstract void OnPreGUI(Rect position, SerializedProperty property);

    public abstract void OnPostGUI(Rect position, SerializedProperty property, bool changed);

    // アトリビュートのうち一つでもfalseだったらそのGUIは非表示になる
    public virtual bool IsVisible(SerializedProperty property)
    {
        return true;
    }

    public void SetAttributes(MultiPropertyBaseAttribute[] attributes)
    {
        Attributes = attributes;
    }

    public void SetPropertyDrawers(IAttributePropertyDrawer[] propertyDrawers)
    {
        PropertyDrawers = propertyDrawers;
    }

    #endif
}