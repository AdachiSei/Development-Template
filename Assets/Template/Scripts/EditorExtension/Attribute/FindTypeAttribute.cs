using System;
using UnityEditor;
using UnityEngine;

[System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class FindTypeAttribute : MultiPropertyBaseAttribute
{
    public FindTypeAttribute()
    {

    }

    public override void OnPostGUI(Rect position, SerializedProperty property, bool changed)
    {
        throw new NotImplementedException();
    }

    public override void OnPreGUI(Rect position, SerializedProperty property)
    {
        throw new NotImplementedException();
    }
}