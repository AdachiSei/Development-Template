using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Template.AudioData;
using UnityEditor;
using UnityEngine;

namespace TemplateEditor.Tools
{
	/// <summary>
	/// オーディオデータのニックネームを定数で管理する構造体を作成するエディター拡張
	/// </summary>
	public static class AudioNickNameCreator
	{
        #region Member Variables

		/// <summary>
        /// 音楽用ファイル名
		/// </summary>
        readonly private static string FILENAME_BGM =
			Path.GetFileNameWithoutExtension(EXPORT_PATH_BGM);

		/// <summary>
		/// 効果音用ファイル名
		/// </summary>
		readonly private static string FILENAME_SFX =
			Path.GetFileNameWithoutExtension(EXPORT_PATH_SFX);

        #endregion

        #region Constants

		/// <summary>
		/// コマンド名
		/// </summary>
        private const string COMMAND_NAME = "Tools/Create Constants/Audio NickName";

		/// <summary>
		/// ショートカットキー
		/// </summary>
		private const string SHORTCUT_KEY = " &a";

		/// <summary>
		/// 作成したスクリプトを保存するパス(BGM)
		/// </summary>
		private const string EXPORT_PATH_BGM = "Assets/Template/Scripts/Constants/BGMName.cs";

		/// <summary>
		/// 作成したスクリプトを保存するパス(SFX)
		/// </summary>
		private const string EXPORT_PATH_SFX = "Assets/Template/Scripts/Constants/SFXName.cs";

		#endregion

		#region MenuItem Methods

		/// <summary>
		/// オーディオデータのニックネームを定数で管理する構造体を作成します
		/// </summary>
		[MenuItem(COMMAND_NAME + SHORTCUT_KEY)]
		private static void Create()
		{			
			if (!CanCreate()) return;

			CreateBGMName();
			CreateSFXName();
			Debug.Log("AudioNameを作成完了");
		}

		/// <summary>
		/// オーディオデータのニックネームを定数で管理する構造体を作成できるかどうかを取得します
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

        #region BGMName Methods

        /// <summary>
        /// BGM用スクリプトを作成する関数
        /// </summary>
        private static void CreateBGMName()
		{
			var directoryName = Path.GetDirectoryName(EXPORT_PATH_BGM);

			if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

			File.WriteAllText(EXPORT_PATH_BGM, BuildBGMName(), Encoding.UTF8);
			AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
		}
		private static string BuildBGMName()
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
					builder.Append("\t").AppendLine("/// オーディオデータのニックネームを定数で管理する構造体");
					builder.Append("\t").AppendLine("/// </summary>");
					builder.Append("\t").AppendFormat("public static class {0}", FILENAME_BGM).AppendLine();
					builder.Append("\t").AppendLine("{");

					//Constants
					{
						builder.Append("\t").Append("\t").AppendLine("#region Constants");
						builder.AppendLine("\t");

						foreach (var guid in AssetDatabase.FindAssets("t:BGMData"))
						{
							var path = AssetDatabase.GUIDToAssetPath(guid);
							var asset = AssetDatabase.LoadMainAssetAtPath(path);
							var data = asset as BGMData;

							builder
								.Append("\t")
								.Append("\t")
								.AppendFormat
									(@"  public const string {0} = ""{1}"";",
										data.NickName.Replace(" ", "_").ToUpper(),
										data.NickName)
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

        #region SFXName Methods

        /// <summary>
        /// SFX用スクリプトを作成する関数
        /// </summary>
        private static void CreateSFXName()
		{
			var directoryName = Path.GetDirectoryName(EXPORT_PATH_SFX);

			if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

			File.WriteAllText(EXPORT_PATH_SFX, BuildSFXName().ToString(), Encoding.UTF8);
			AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
		}

		private static string BuildSFXName()
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
					builder.Append("\t").AppendLine("/// 効果音名を定数で管理するクラス");
					builder.Append("\t").AppendLine("/// </summary>");
					builder.Append("\t").AppendFormat("public static class {0}", FILENAME_SFX).AppendLine();
					builder.Append("\t").AppendLine("{");

					//Constants
					{
						builder.Append("\t").Append("\t").AppendLine("#region Constants");
						builder.AppendLine("\t");

						foreach (var guid in AssetDatabase.FindAssets("t:SFXData"))
						{
							var path = AssetDatabase.GUIDToAssetPath(guid);
							var asset = AssetDatabase.LoadMainAssetAtPath(path);
							var data = asset as SFXData;

							builder
								.Append("\t")
								.Append("\t")
								.AppendFormat
									(@"  public const string {0} = ""{1}"";",
										data.NickName.Replace(" ", "_").ToUpper(),
										data.NickName)
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