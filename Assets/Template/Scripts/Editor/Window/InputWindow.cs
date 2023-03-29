using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 文字を入力するためのポップアップウィンドウ
/// </summary>
public class InputWindow : PopupWindowContent
{
    #region Properties

    public string _scriptName  = "NewScript";

    #endregion

    #region Events

    public event Action<string> OnCreate = null;

    #endregion

    #region Init Methods

    public InputWindow(Action<string> action)
    {
        OnCreate = null;
        OnCreate += action;
    }

    #endregion

    #region Unity Methods

    /// <summary>
    /// サイズを取得する
    /// </summary>
    public override Vector2 GetWindowSize()
    {
        return new (300f, 50f);
    }

    /// <summary>
    /// GUI描画
    /// </summary>
    public override void OnGUI(Rect rect)
    {
        EditorGUILayout.LabelField("Please Enter The Script Name");

        EditorGUILayout.Space();

        var scriptName = EditorGUILayout
                    .TextField("スクリプト名", _scriptName);
        _scriptName = scriptName;
    }

    /// <summary>
    /// 閉じたときの処理
    /// </summary>
    public override void OnClose()
    {
        OnCreate?.Invoke(_scriptName);
    }

    #endregion
}