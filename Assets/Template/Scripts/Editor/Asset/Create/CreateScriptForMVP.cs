using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Template.Constant;
using UnityEditor;
using UnityEngine;

namespace TemplateEditor.Create
{
	public static class CreateScriptForMVP
	{
		#region Member Variables

		// �t�@�C����(�g���q����A�Ȃ�)
		private static readonly string FILENAME =
			Path.GetFileNameWithoutExtension(EXPORT_PATH);

		private static string _rootNameSpaceName = "";

		#endregion

		#region Constants

		private const string PLUGIN_PATH = "Assets/ScriptTemplates/";

		// �R�}���h��
		private const string COMMAND_NAME = "Assets/Create/Design Pattern/Game Programming/MVP";

		//�쐬�����X�N���v�g��ۑ�����p�X
		private const string EXPORT_PATH = "NewScript.cs";

		#endregion

		[MenuItem(COMMAND_NAME, priority = 70)]
		private static void Create()
		{
			if (!CanCreate()) return;
			CreateScript();
			Debug.Log("�쐬����");
		}

		/// <summary>
		/// �쐬�ł��邩�ǂ������擾���܂�
		/// </summary>
		[MenuItem(COMMAND_NAME, true)]
		private static bool CanCreate()
		{
			var isPlayingEditor = !EditorApplication.isPlaying;
			var isPlaying = !Application.isPlaying;
			var isCompiling = !EditorApplication.isCompiling;
			return isPlayingEditor && isPlaying && isCompiling;
		}

		private static void CreateScript()
		{
			// �N���b�N�����ʒu�����_�Ƃ���Rect�����
			// �{���̃|�b�v�A�b�v�̗p�r�Ƃ��Ďg���ꍇ�̓{�^����Rect��n��
			var mouseRect = new Rect(new Vector2(100f, 100f), Vector2.one);

			// PopupWindowContent�𐶐�
			var content = new InputWindow(CreateMVP);

			// �J��
			PopupWindow.Show(mouseRect, content);
		}

		private static void CreateMVP(string scriptName)
        {
			_rootNameSpaceName = RootNameSpaceName.DEFAULT;
			CreateModel(scriptName);
			CreateView(scriptName);
			CreatePresenter(scriptName);
        }

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
					builder.Append("\t").AppendFormat("public class {0}", $"{FILENAME}Data").AppendLine();
					builder.Append("\t").AppendLine("{");

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
					builder.Append("\t").AppendFormat("public class {0}", $"{FILENAME}View : MonoBehaviour").AppendLine();
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
					builder.Append("\t").AppendFormat("public class {0}", $"{FILENAME}Presenter : MonoBehaviour").AppendLine();
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

					builder.Append("\t").AppendLine("}");
				}

				if (_rootNameSpaceName != "") builder.AppendLine("}");
			}

			return builder;
		}

		#endregion
	}
}
