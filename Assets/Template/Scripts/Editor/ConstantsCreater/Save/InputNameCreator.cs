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
    /// インプット名を定数で管理する構造体を作成するエディター拡張
    /// </summary>
    public class InputNameCreator : AssetPostprocessor
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
        private const string EXPORT_PATH = "Assets/Template/Scripts/Constants/InputName.cs";

        #endregion

        #region Unity Methods

        private static void OnPostprocessAllAssets
            (string[] importedAssets, string[] deletedAssets,
                string[] movedAssets, string[] movedFromPath) =>
            CreateScriptInputName(importedAssets);

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
        private static void CreateScriptInputName(string[] importedAssets)
        {
            // InputManagerの変更チェック
            var inputManagerPath =
                Array
                    .Find(importedAssets,
                        path => Path.GetFileName(path) == "InputManager.asset");

            if (inputManagerPath == null) return;

            // InputManagerの設定情報読み込み
            var serializedObjects =
                AssetDatabase
                    .LoadAllAssetsAtPath(inputManagerPath);

            var axesSize =
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(inputManagerPath)[0])
                        .FindProperty("m_Axes")
                        .arraySize;

            var inputNames = new List<string>();

            //全部取ってくる
            for (int i = 0; i < axesSize; ++i)
            {
                foreach (var serialized in serializedObjects)
                {
                    var newSerialize = new SerializedObject(serialized);
                    var newAxesProperty = newSerialize.FindProperty("m_Axes");
                    var newAxisProperty = newAxesProperty.GetArrayElementAtIndex(i);
                    var inputName = GetChildProperty(newAxisProperty, "m_Name").stringValue;
                    inputNames.Add(inputName);
                }
            }

            var builder = new StringBuilder();

            //Script
            {
                //NameSpace
                builder.AppendLine("namespace Template.Constant");
                builder.AppendLine("{");
                //Struct
                {
                    builder.Append("\t").AppendLine("/// <summary>");
                    builder.Append("\t").AppendLine("/// インプット名を定数で管理する構造体");
                    builder.Append("\t").AppendLine("/// </summary>");
                    builder.Append("\t").AppendFormat("public static class {0}", FILENAME).AppendLine();
                    builder.Append("\t").AppendLine("{");

                    //Constants
                    {
                        builder.Append("\t").Append("\t").AppendLine("#region Constants");
                        builder.AppendLine("\t");

                        //重複する要素を消す
                        inputNames = new(inputNames.Distinct());
                        foreach (var name in inputNames)
                        {
                            var constantName = 
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

            var directoryName = Path.GetDirectoryName(EXPORT_PATH);

            if (!Directory.Exists(directoryName)) Directory.CreateDirectory(directoryName);

            File.WriteAllText(EXPORT_PATH, builder.ToString(), Encoding.UTF8);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            Debug.Log("InputNameを作成完了");
        }

        #endregion
    }
}
