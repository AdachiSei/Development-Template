using System;
using UnityEngine;

[System.AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
public class FindTypeAttribute : PropertyAttribute
{
    public FindTypeAttribute()
    {

    }
}