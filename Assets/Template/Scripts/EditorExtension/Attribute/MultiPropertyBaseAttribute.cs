using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public abstract class MultiPropertyBaseAttribute : PropertyAttribute
{
    public MultiPropertyBaseAttribute[] Attributes;
    public IAttributePropertyDrawer[] PropertyDrawers;

    #if UNITY_EDITOR

    public abstract void OnPreGUI(Rect position, SerializedProperty property);

    public abstract void OnPostGUI(Rect position, SerializedProperty property, bool changed);

    // アトリビュートのうち一つでもfalseだったらそのGUIは非表示になる
    public virtual bool IsVisible(SerializedProperty property)
    {
        return true;
    }

    #endif
}