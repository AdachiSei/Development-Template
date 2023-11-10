using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Template.Constant;
using Template.Extension;
using UnityEditor;
using UnityEngine;

namespace TemplateEditor.Asset
{
    /// <summary>
    /// �G�f�B�^�[�g���������X�N���v�g���쐬����G�f�B�^�[�g��
    /// </summary>
    public static class InstallerCreater
    {
        #region Constants

        // �R�}���h��
        private const string COMMAND_NAME = "Assets/Create/Select And Semiautomatic/Installer Script";

        #endregion

        #region Private Methods

        [MenuItem(COMMAND_NAME, priority = 170)]
        private static void CreateInstaller()
        {
            var filtered = Selection.GetFiltered
                            (typeof(MonoScript), SelectionMode.Assets);

            foreach (var monoScript in filtered)
            {
                var path = AssetDatabase.GetAssetPath(monoScript);
                var deta = monoScript as MonoScript;
                var baseClassName = deta.text.GetExtractedData(":", "{");
                var directoryName = Path.GetDirectoryName(path);
                var fileName = Path.GetFileNameWithoutExtension(path);
                CreateScript(directoryName, fileName, baseClassName);
                Debug.Log($"{fileName}�̃C���X�g�[���[���쐬����");
            }

            Selection.activeObject = null;
        }

        /// <summary>
        /// �V�[���̃t�@�C������萔�ŊǗ�����N���X���쐬�ł��邩�ǂ������擾���܂�
        /// </summary>
        [MenuItem(COMMAND_NAME, true)]
        private static bool CanCreate()
        {
            return Selection.GetFiltered(typeof(MonoScript), SelectionMode.Assets).Any();
        }

        private static void CreateScript(string directoryname, string fileName, string BaseFileName)
        {
            var scriptName = $"{directoryname}/Installer/{fileName}Installer.cs";
            string directoryName = Path.GetDirectoryName(scriptName);

            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

            File.WriteAllText(scriptName, CreateBuilder(fileName, BaseFileName).ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
        }


        private static StringBuilder CreateBuilder(string fileName, string BaseFileName)
        {
            var builder = new StringBuilder();
            var rootNameSpaceName = RootNameSpaceName.DEFAULT;

            //Script
            {
                //Using
                {
                    builder.AppendLine("using Zenject;");

                    if (rootNameSpaceName != "")
                        builder.AppendLine($"using {RootNameSpaceName.DEFAULT};");
                }

                builder.AppendLine("\t");

                //NameSpace
                {
                    if (rootNameSpaceName != "")
                    {
                        builder.AppendLine($"namespace {RootNameSpaceName.DEFAULT}.Installer");
                        builder.AppendLine("{");
                    }

                    //Class
                    {
                        builder.Append("\t").AppendLine
                            ("/// <summary>");
                        builder.Append("\t").AppendLine
                            ($"/// {fileName}�̃C���X�g�[���[");
                        builder.Append("\t").AppendLine
                            ("/// </summary>");
                        builder.Append("\t").AppendFormat
                            ("public class {0} : Installer<{0}>", $"{fileName}Installer")
                            .AppendLine();
                        builder.Append("\t").AppendLine("{");

                        //Methods
                        {
                            builder.Append("\t").Append("\t").AppendLine
                                ("public override void InstallBindings()");
                            builder.Append("\t").Append("\t").AppendLine
                                ("{");
                            builder.Append("\t").Append("\t").Append("\t").AppendLine
                                ("Container");
                            builder.Append("\t").Append("\t").Append("\t").AppendFormat
                                (".Bind<{0}>()", BaseFileName).AppendLine();
                            builder.Append("\t").Append("\t").Append("\t").AppendFormat
                                (".To<{0}>()", fileName).AppendLine();
                            builder.Append("\t").Append("\t").Append("\t").AppendLine
                                (".AsSingle();");
                            builder.Append("\t").Append("\t").AppendLine
                                ("}");
                        }

                        builder.Append("\t").AppendLine("}");
                    }

                    if (rootNameSpaceName != "")
                        builder.AppendLine("}");
                }
            }
            return builder;
        }

        #endregion
    }
}