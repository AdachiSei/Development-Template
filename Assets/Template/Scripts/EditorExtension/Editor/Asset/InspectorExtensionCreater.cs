using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Template.Constant;
using Template.Extension;
using UnityEditor;
using UnityEngine;

namespace TemplateEditor.Asset
{
	/// <summary>
	/// エディター拡張したいスクリプトを作成するエディター拡張
	/// </summary>
	public static class InspectorExtensionCreater
    {
		#region Constants

		// コマンド名
		private const string COMMAND_NAME = "Assets/Inspector Extension Script";

		#endregion

		#region Private Methods

		[MenuItem(COMMAND_NAME)]
        private static void CreateEditorExtension()
        {
            var filtered = Selection.GetFiltered
							(typeof(MonoScript), SelectionMode.Assets);

            foreach (var monoScript in filtered)
            {
                var path = AssetDatabase.GetAssetPath(monoScript);
				var directoryName = Path.GetDirectoryName(path);
				var fileName = Path.GetFileNameWithoutExtension(path);
				CreateScript(directoryName, fileName);
				Debug.Log($"{fileName}のエディター拡張を作成した");
			}

            Selection.activeObject = null;
        }

		/// <summary>
		/// シーンのファイル名を定数で管理するクラスを作成できるかどうかを取得します
		/// </summary>
		[MenuItem(COMMAND_NAME, true)]
		private static bool CanCreate()
		{
			var isPlayingEditor = !EditorApplication.isPlaying;
			var isPlaying = !Application.isPlaying;
			return isPlayingEditor && isPlaying;
		}

		private static void CreateScript(string directoryname, string fileName)
		{
			var scriptName = $"{directoryname}/Editor/{fileName}Inspector.cs";
			string directoryName = Path.GetDirectoryName(scriptName);

			if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

			File.WriteAllText(scriptName, CreateBuilder(fileName).ToString(), Encoding.UTF8);
			AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
		}


		private static StringBuilder CreateBuilder(string fileName)
        {
			var builder = new StringBuilder();

			//Script
			{
				//Using
				{
					builder.AppendLine("using System.Collections;");
					builder.AppendLine("using System.Collections.Generic;");
					builder.AppendLine("using UnityEngine;");
					builder.AppendLine("using UnityEditor;");

					var rootNameSpaceName = RootNameSpaceName.DEFAULT;
					if (rootNameSpaceName != "")
						builder.AppendLine($"using {RootNameSpaceName.DEFAULT};");
				}

				builder.AppendLine("\t");

				//NameSpace
				builder.AppendLine($"namespace {RootNameSpaceName.DEFAULT}Editor.Inspector");
				builder.AppendLine("{");

				//Class
				{
					builder.Append("\t").AppendLine("/// <summary>");
					builder.Append("\t").AppendLine($"/// {fileName}のエディター拡張");
					builder.Append("\t").AppendLine("/// </summary>");
					builder.Append("\t").AppendLine($"[CustomEditor(typeof({fileName}))]");
					builder.Append("\t").AppendFormat("public class {0} : Editor", $"{fileName}Inspector").AppendLine();
					builder.Append("\t").AppendLine("{");

					//Member Variables
					{
						builder.Append("\t").Append("\t").AppendLine("#region Member Variables");
						builder.AppendLine("\t");

						builder.Append("\t").Append("\t").AppendLine("private static bool _isOpening;");

						builder.AppendLine("\t");
						builder.Append("\t").Append("\t").AppendLine("#endregion");
					}

					builder.AppendLine("\t");

					//Unity Methods
					{
						builder.Append("\t").Append("\t").AppendLine("#region Unity Methods");
						builder.AppendLine("\t");

						builder.Append("\t").Append("\t").AppendLine("public override void OnInspectorGUI()");
						builder.Append("\t").Append("\t").AppendLine("{");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("EditorGUI.BeginDisabledGroup(true);");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("EditorGUILayout.ObjectField(\"Editor\", MonoScript.FromScriptableObject(this), typeof(MonoScript), false);");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("EditorGUI.EndDisabledGroup();");
						builder.Append("\t").AppendLine("\t");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("base.OnInspectorGUI();");
						builder.AppendLine("\t");
						builder.Append("\t").Append("\t").Append("\t").AppendLine($"var {fileName.ToLower()} = target as {fileName};");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("var style = new GUIStyle(EditorStyles.label);");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("style.richText = true;");
						builder.AppendLine("\t");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("EditorGUILayout.Space();");
						builder.AppendLine("\t");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("_isOpening = EditorGUILayout.Foldout(_isOpening, \"Settings\");");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("if (_isOpening)");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("{");
						builder.Append("\t").Append("\t").Append("\t").Append("\t").AppendLine("EditorGUILayout.Space();");
						builder.AppendLine("\t");
						builder.Append("\t").Append("\t").Append("\t").Append("\t").AppendLine("//if (GUILayout.Button())");
						builder.Append("\t").Append("\t").Append("\t").Append("\t").AppendLine("//{");
						builder.AppendLine("\t");
						builder.Append("\t").Append("\t").Append("\t").Append("\t").AppendLine("//}");
						builder.Append("\t").Append("\t").Append("\t").AppendLine("}");

						builder.Append("\t").Append("\t").AppendLine("}");

						builder.AppendLine("\t");
						builder.Append("\t").Append("\t").AppendLine("#endregion");
					}

					builder.Append("\t").AppendLine("}");
				}

				builder.AppendLine("}");
			}
			return builder;
		}

		#endregion
	}
}