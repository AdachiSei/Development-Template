using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// GUI描画をコントロールしたいMultiPropertyAttributeにはこれを付ける
public interface IAttributePropertyDrawer
{
    #if UNITY_EDITOR

    void OnGUI(Rect position, SerializedProperty property, GUIContent label);

    float GetPropertyHeight(SerializedProperty property, GUIContent label);

    #endif
}

