using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

/// <summary>
/// カレントディレクトリが取得できる
/// </summary>
public static class CurrentDirectory
{
    public static string GetCurrentDirectory()
    {
        var flag = BindingFlags.Public | BindingFlags.NonPublic 
                    | BindingFlags.Static | BindingFlags.Instance;
        var asm = Assembly.Load("UnityEditor.dll");
        var typeProjectBrowser = asm.GetType("UnityEditor.ProjectBrowser");
        var projectBrowserWindow = EditorWindow.GetWindow(typeProjectBrowser);
        var currentDirectory = typeProjectBrowser
                                .GetMethod("GetActiveFolderPath", flag)
                                .Invoke(projectBrowserWindow, null);
        return currentDirectory as string;
    }
}