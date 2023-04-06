using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Template.Constant;
using TemplateEditor.Project;
using TemplateEditor.Window;
using UnityEditor;
using UnityEngine;

namespace TemplateEditor.Asset.Create
{
	public class MVPPatternCreater
	{
		#region Member Variables

		// ファイル名(拡張子あり、なし)
		private static readonly string FILENAME =
			Path.GetFileNameWithoutExtension(EXPORT_PATH);

		private static string _rootNameSpaceName = "";

		private static readonly Vector2 _windowPos = new(100, 500);

		#endregion

		#region Constants

		// コマンド名
		private const string COMMAND_NAME = "Assets/Create/Design Pattern/Game Programming/MVP";

		//作成したスクリプトを保存するパス
		private const string EXPORT_PATH = "NewScript.cs";

		#endregion

		private static Rect _dropDownButtonRect;

		#region Private Methods

		[MenuItem(COMMAND_NAME, priority = 70)]
		private static void CreateScript()
		{
			// クリックした位置を視点とするRectを作る
			// 本来のポップアップの用途として使う場合はボタンのRectを渡す
			var mouseRect = new Rect(_windowPos, Vector2.one);

			// PopupWindowContentを生成
			var content = new TextFieldWindow(CreateMVP);

			// 開く
			PopupWindow.Show(mouseRect, content);
		}

		private static void CreateMVP(string scriptName)
        {
			_rootNameSpaceName = RootNameSpaceName.DEFAULT;
			CreateModel(scriptName);
			CreateView(scriptName);
			CreatePresenter(scriptName);
        }

        #endregion

        #region Model Methods

        private static void CreateModel(string scriptName)
        {

			var path = $"{CurrentDirectory.GetCurrentDirectory()}/Model/{scriptName}Data.cs";
			var directoryName = Path.GetDirectoryName(path);

			if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

			File.WriteAllText(path, BuildModel().ToString(), Encoding.UTF8);
			AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
		}

		private static StringBuilder BuildModel()
        {
			var builder = new StringBuilder();

			//Script
			{
				builder.AppendLine($"using System.Collections;");
				builder.AppendLine($"using System.Collections.Generic;");
				builder.AppendLine($"using UnityEngine;");
				builder.AppendLine("\t");

				//NameSpace
				if (_rootNameSpaceName != "")
				{
					builder.AppendLine($"namespace {RootNameSpaceName.DEFAULT}");
					builder.AppendLine("{");
				}

				//Class
				{
					builder.Append("\t").AppendLine("/// <summary>");
					builder.Append("\t").AppendLine("/// Data");
					builder.Append("\t").AppendLine("/// </summary>");
					builder.Append("\t").AppendLine($"public class {FILENAME}Data");
					builder.Append("\t").AppendLine("{");
					{

						builder.Append("\t").Append("\t").AppendLine("#region Properties");
						builder.Append("\t").Append("\t").AppendLine("#endregion");

						builder.AppendLine("\t");

						builder.Append("\t").Append("\t").AppendLine("#region Inspector Variables");
						builder.Append("\t").Append("\t").AppendLine("#endregion");

						builder.AppendLine("\t");

						builder.Append("\t").Append("\t").AppendLine("#region Member Variables");
						builder.Append("\t").Append("\t").AppendLine("#endregion");

						builder.AppendLine("\t");

						builder.Append("\t").Append("\t").AppendLine("#region Public Methods");
						builder.Append("\t").Append("\t").AppendLine("#endregion");

						builder.AppendLine("\t");

						builder.Append("\t").Append("\t").AppendLine("#region Private Methods");
						builder.Append("\t").Append("\t").AppendLine("#endregion");

					}
					builder.Append("\t").AppendLine("}");
				}

				if (_rootNameSpaceName != "") builder.AppendLine("}");
			}

			return builder;
		}

		#endregion

		#region View Methods

		private static void CreateView(string scriptName)
		{
			var path = $"{CurrentDirectory.GetCurrentDirectory()}/View/{scriptName}View.cs";
			var directoryName = Path.GetDirectoryName(path);

			if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

			File.WriteAllText(path, BuildView().ToString(), Encoding.UTF8);
			AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
		}

		private static StringBuilder BuildView()
		{
			var builder = new StringBuilder();

			//Script
			{
				builder.AppendLine($"using System.Collections;");
				builder.AppendLine($"using System.Collections.Generic;");
				builder.AppendLine($"using UnityEngine;");
				builder.AppendLine("\t");

				//NameSpace
				if (_rootNameSpaceName != "")
				{
					builder.AppendLine($"namespace {RootNameSpaceName.DEFAULT}");
					builder.AppendLine("{");
				}

				//Class
				{
					builder.Append("\t").AppendLine("/// <summary>");
					builder.Append("\t").AppendLine("/// View");
					builder.Append("\t").AppendLine("/// </summary>");
					builder.Append("\t").AppendLine($"public class {FILENAME}View : MonoBehaviour");
					builder.Append("\t").AppendLine("{");

					builder.Append("\t").Append("\t").AppendLine("#region Inspector Variables");
					builder.Append("\t").Append("\t").AppendLine("#endregion");

					builder.AppendLine("\t");

					builder.Append("\t").Append("\t").AppendLine("#region Unity Methods");

					builder.AppendLine("\t");

					builder.Append("\t").Append("\t").AppendLine("private void Awake()");
					builder.Append("\t").Append("\t").AppendLine("{");

					builder.AppendLine("\t");

					builder.Append("\t").Append("\t").AppendLine("}");

					builder.AppendLine("\t");

					builder.Append("\t").Append("\t").AppendLine("#endregion");

					builder.AppendLine("\t");

					builder.Append("\t").Append("\t").AppendLine("#region Public Methods");
					builder.Append("\t").Append("\t").AppendLine("#endregion");

					builder.Append("\t").AppendLine("}");
				}

				if (_rootNameSpaceName != "") builder.AppendLine("}");
			}

			return builder;
		}

		#endregion

		#region Presenter Methods

		private static void CreatePresenter(string scriptName)
		{

			var path = $"{CurrentDirectory.GetCurrentDirectory()}/Presenter/{scriptName}Presenter.cs";
			var directoryName = Path.GetDirectoryName(path);

			if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

			File.WriteAllText(path, BuildPresenter().ToString(), Encoding.UTF8);
			AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
		}

		private static StringBuilder BuildPresenter()
		{
			var builder = new StringBuilder();

			//Script
			{
				builder.AppendLine($"using System.Collections;");
				builder.AppendLine($"using System.Collections.Generic;");
				builder.AppendLine($"using UnityEngine;");
				builder.AppendLine($"using UniRx;");
				builder.AppendLine("\t");

				//NameSpace
				if (_rootNameSpaceName != "")
				{
					builder.AppendLine($"namespace {RootNameSpaceName.DEFAULT}");
					builder.AppendLine("{");
				}

				//Class
				{
					builder.Append("\t").AppendLine("/// <summary>");
					builder.Append("\t").AppendLine("/// Presenter");
					builder.Append("\t").AppendLine("/// </summary>");
					builder.Append("\t").AppendLine($"public class {FILENAME}Presenter : MonoBehaviour");
					builder.Append("\t").AppendLine("{");
					{
						{
							builder.Append("\t").Append("\t").AppendLine("#region Properties");
							{
								builder.AppendLine("\t");

								builder.Append("\t").Append("\t").Append($"public {FILENAME}Data {FILENAME}Data");
								builder.AppendLine(" { get; private set; } = new();");

								builder.AppendLine("\t");
							}
							builder.Append("\t").Append("\t").AppendLine("#endregion");

							builder.AppendLine("\t");

							builder.Append("\t").Append("\t").AppendLine("#region Inspector Variables");
							{
								builder.AppendLine("\t");

								builder.Append("\t").Append("\t").AppendLine("[SerializeField]");
								builder.Append("\t").Append("\t").AppendLine($"private {FILENAME}View _{FILENAME.ToLower()}View = null;");

								builder.AppendLine("\t");
							}
							builder.Append("\t").Append("\t").AppendLine("#endregion");

							builder.AppendLine("\t");

							builder.Append("\t").Append("\t").AppendLine("#region Unity Methods");
							{
								builder.AppendLine("\t");

								builder.Append("\t").Append("\t").AppendLine("private void Awake()");
								builder.Append("\t").Append("\t").AppendLine("{");

								builder.AppendLine("\t");

								builder.Append("\t").Append("\t").AppendLine("}");

								builder.AppendLine("\t");
							}
							builder.Append("\t").Append("\t").AppendLine("#endregion");

						}
					}
					builder.Append("\t").AppendLine("}");
				}

				if (_rootNameSpaceName != "") builder.AppendLine("}");
			}

			return builder;
		}

		#endregion
	}
}
