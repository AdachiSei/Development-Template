using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using Template.Fade;
using Template.Scene;
using UnityEngine;
using Zenject;

/// <summary>
/// シリアライズブル
/// </summary>
public class SceneLoadManager
{
    ILoadableScene _sceneLoader = new SceneLoader();

    IFadable _fadeView;

    public async UniTask LoadScene(string sceneName)
    {
        await _fadeView.FadeOut();
        await _sceneLoader.LoadScene(sceneName);
        _fadeView.FadeIn().Forget();
    }
}