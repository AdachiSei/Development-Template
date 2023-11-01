using System.Collections;
using System.Collections.Generic;
using Template.Manager;
using UnityEngine;
using Zenject;

/// <summary>
/// スクリプト
/// </summary>
public class ILoadableSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<ILoadableScene>()
            .To<SceneLoader>()
            .AsSingle();
    }
}