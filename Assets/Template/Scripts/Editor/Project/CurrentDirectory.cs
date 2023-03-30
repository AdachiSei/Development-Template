using System.Reflection;
using UnityEditor;

namespace TemplateEditor.Project
{
    /// <summary>
    /// 開いているディレクトリを取得できるスクリプト
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
}