using System.Collections;
using System.Collections.Generic;
using Template.Manager;
using UnityEngine;
using Zenject;

/// <summary>
/// スクリプト
/// </summary>
public class IRegistablePauseInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container
            .Bind<IRegistablePause>()
            .To<PauseData>()
            .AsSingle();
    }
}