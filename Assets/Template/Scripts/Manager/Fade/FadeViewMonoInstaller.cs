using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using Template.Scene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Template.Fade
{
    /// <summary>
    /// フェードのMonoInstaller
    /// </summary>
    public class FadeViewMonoInstaller : MonoInstaller<FadeViewMonoInstaller>
    {
        [SerializeField]
        [Header("ローディング時に回転する絵")]
        private Image _loadingImage = null;

        [SerializeField]
        [Header("ローディング時に表示するパネル")]
        private Image _loadingPanel = null;

        public override void InstallBindings()
        {
            Container
                .Bind<IFadable>()
                .To<FadeViewTemplate>()
                .FromInstance(new FadeViewTemplate(_loadingImage, _loadingPanel))
                .AsSingle();
        }
    }
}