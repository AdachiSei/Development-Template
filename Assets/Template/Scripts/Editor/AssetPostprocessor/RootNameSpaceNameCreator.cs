using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace TemplateEditor.Processor
{
    /// <summary>
    /// 名前空間名を定数で管理する構造体を作成するエディター拡張
    /// </summary>
    public class RootNameSpaceNameCreator : AssetPostprocessor
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
        /// 作成したスクリプトを保存するパス
        /// </summary>
        private const string EXPORT_PATH = "Assets/Scripts/Constants/RootNameSpaceName.cs";

        #endregion

        #region Unity Methods

        private static void OnPostprocessAllAssets
            (string[] importedAssets, string[] deletedAssets,
                string[] movedAssets, string[] movedFromPath) =>
            CreateScript(importedAssets);

        #endregion

        #region Private Methods

        private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
        {
            SerializedProperty child = parent.Copy();
            child.Next(true);
            do if (child.name == name) return child;
            while (child.Next(false));
            return null;
        }

        /// <summary>
        /// インプット名を定数で管理する構造体を作成する関数
        /// </summary>
        /// <param name="importedAssets"></param>
        private static void CreateScript(string[] importedAssets)
        {
            // EditorSettingsの変更チェック
            var EditorSettingsPath =
                Array
                    .Find(importedAssets,
                        path => Path.GetFileName(path) == "EditorSettings.asset");

            if (EditorSettingsPath == null) return;

            // EditorSettingsの設定情報読み込み
            var serializedObjects =
                AssetDatabase
                    .LoadAllAssetsAtPath(EditorSettingsPath);

            var newSerialize = new SerializedObject(serializedObjects[0]);
            var rootNameSpaceProperty = newSerialize.FindProperty("m_ProjectGenerationRootNamespace");
            var name = rootNameSpaceProperty.stringValue;


            var builder = new StringBuilder();

            //Script
            {
                //NameSpace
                builder.AppendLine("namespace Template.Constant");
                builder.AppendLine("{");

                //Struct
                {
                    builder.Append("\t").AppendLine("/// <summary>");
                    builder.Append("\t").AppendLine("/// 名前空間名を定数で管理する構造体");
                    builder.Append("\t").AppendLine("/// </summary>");
                    builder.Append("\t").AppendFormat("public struct {0}", FILENAME).AppendLine();
                    builder.Append("\t").AppendLine("{");

                    //Constants
                    {
                        builder.Append("\t").Append("\t").AppendLine("#region Constants");
                        builder.AppendLine("\t");

                        builder
                            .Append("\t")
                            .Append("\t")
                            .AppendFormat(@"  public const string {0} = ""{1}"";", "DEFAULT", name)
                            .AppendLine();

                        builder.AppendLine("\t");
                        builder.Append("\t").Append("\t").AppendLine("#endregion");
                    }

                    builder.Append("\t").AppendLine("}");
                }

                builder.AppendLine("}");
            }

            var directoryName = Path.GetDirectoryName(EXPORT_PATH);

            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

            File.WriteAllText(EXPORT_PATH, builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            Debug.Log("InputNameを作成完了");
        }

        #endregion
    }
}