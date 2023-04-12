using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace TemplateEditor.Tools
{
	/// <summary>
	/// ビルドシーン名を定数で管理する構造体を作成するエディター拡張
	/// </summary>
	public static class SceneNameCreator
	{
        #region Member Variables

        /// <summary>
		/// ファイル名
		/// </summary>
        private static readonly string FILENAME =
			Path.GetFileNameWithoutExtension(EXPORT_PATH);

        #endregion

        #region Constants

		/// <summary>
        /// コマンド名
		/// </summary>
        private const string COMMAND_NAME = "Tools/Create Constants/Scene Name";

		/// <summary>
		/// ショートカットキー
		/// </summary>
        private const string SHORTCUT_KEY = " &s";

		/// <summary>
		/// 作成したスクリプトを保存するパス
		/// </summary>
		private const string EXPORT_PATH = "Assets/Template/Scripts/Constants/SceneName.cs";

		#endregion

		#region MenuItem Methods

		/// <summary>
		/// シーンのファイル名を定数で管理する構造体を作成します
		/// </summary>
		[MenuItem(COMMAND_NAME + SHORTCUT_KEY)]
		private static void Create()
		{
			if (!CanCreate()) return;
			CreateSceneName();
			Debug.Log("SceneNameを作成完了");
		}

		/// <summary>
		/// シーンのファイル名を定数で管理する構造体を作成できるかどうかを取得します
		/// </summary>
		[MenuItem(COMMAND_NAME, true)]
		private static bool CanCreate()
		{
			var isPlayingEditor = !EditorApplication.isPlaying;
			var isPlaying = !Application.isPlaying;
			var isCompiling = !EditorApplication.isCompiling;
			return isPlayingEditor && isPlaying && isCompiling;
		}

        #endregion

        #region SceneName Methods

        /// <summary>
        /// スクリプトを作成します
        /// </summary>
        private static void CreateSceneName()
		{
			var directoryName = Path.GetDirectoryName(EXPORT_PATH);

			if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

			File.WriteAllText(EXPORT_PATH, BuildConstants(), Encoding.UTF8);
			AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
		}

        private static string BuildConstants()
        {
			var builder = new StringBuilder();

			//Script
			{
				//NameSpace
				builder.AppendLine("namespace Template.Constant");
				builder.AppendLine("{");

				//Struct
				{
					builder.Append("\t").AppendLine("/// <summary>");
					builder.Append("\t").AppendLine("/// シーン名を定数で管理する構造体");
					builder.Append("\t").AppendLine("/// </summary>");
					builder.Append("\t").AppendFormat("public struct {0}", FILENAME).AppendLine();
					builder.Append("\t").AppendLine("{");

					//Constants
					{
						builder.Append("\t").Append("\t").AppendLine("#region Constants");
						builder.AppendLine("\t");

						//BuildSetingsに入っているSceneの名前を全てとってくる
						foreach (var scene in EditorBuildSettings.scenes)
						{
							var name = Path.GetFileNameWithoutExtension(scene.path);
							builder
								.Append("\t")
								.Append("\t")
								.AppendFormat
									(@"  public const string {0} = ""{1}"";",
										name.Replace(" ", "_").ToUpper(),
										name)
								.AppendLine();
						}

						builder.AppendLine("\t");
						builder.Append("\t").Append("\t").AppendLine("#endregion");
					}

					builder.Append("\t").AppendLine("}");
				}

				builder.AppendLine("}");
			}

			return builder.ToString();
		}

		#endregion
	}
}
