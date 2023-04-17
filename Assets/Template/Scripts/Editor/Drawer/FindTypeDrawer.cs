using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(FindTypeAttribute))]
public class FindTypeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (true)
        {
            Rect rect_content = EditorGUI.PrefixLabel(position, label);
            //ボタンを描画
            if (GUI.Button(rect_content, "Find"))
            {
                var customAttribute = typeof(MonoBehaviour).GetField("sampleField").GetCustomAttribute<FindTypeAttribute>();
                Debug.Log(customAttribute);
                //FindTypeAttribute findTypeAttribute = (FindTypeAttribute)attribute;
            }
        }
        else
        {
            if (EditorGUI.PropertyField(position, property, label))
            {

            }
        }

        


        //var a = label.text.Replace(" ", "").Split('(');
        //var b = a[1].Replace(")", "");
        //Type type = Type.GetType(b);
        //Debug.Log(property.objectReferenceValue);


        //// プロパティ名のラベルを描画し、ラベル以外のrectを返す
        //Rect rect_content = EditorGUI.PrefixLabel(position, label);
    }
}
