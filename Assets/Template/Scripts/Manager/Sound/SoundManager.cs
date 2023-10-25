using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using Template.AudioData;

namespace Template.Manager
{
    /// <summary>
    /// サウンドを管理するScript
    /// </summary>
    public class SoundManager : MonoBehaviour, IPauseable
    {
        #region Properties

        public float MasterVolume => _masterVolume;
        public float BGMVolume => _bgmVolume;
        public float SFXVolume => _sfxVolume;
        public int AudioSourceCount { get; private set; } = 0;
        public bool IsStopingToCreate { get; private set; } = false;
        public bool IsPausing { get; private set; } = false;

        #endregion

        #region Inspector Variables

        [SerializeField]
        [Header("最初に流すBGM")]
        private string _name = "";

        [SerializeField]
        [Header("マスター音量")]
        [Range(0, 1)]
        private float _masterVolume = 1;

        [SerializeField]
        [Header("音楽の音量")]
        [Range(0, 1)]
        private float _bgmVolume = 1;

        [SerializeField]
        [Header("効果音の音量")]
        [Range(0, 1)]
        private float _sfxVolume = 1;

        [SerializeField]
        [Header("音が消えるまでの時間")]
        private float _fadeTime = 2f;

        [SerializeField]
        [Header("音楽を格納するオブジェクト")]
        private Transform _bgmParent = null;

        [SerializeField]
        [Header("効果音を格納するオブジェクト")]
        private Transform _sfxParent = null;

        [SerializeField]
        [Header("オーディオソースがついているプレファブ")]
        private AudioSource _audioPrefab = null;

        [SerializeField]
        [Header("音楽のデータ")]
        private BGMData[] _bgmDatas;

        [SerializeField]
        [Header("効果音のデータ")]
        private SFXData[] _sfxDatas;

        [SerializeField]
        [Tooltip("BGM用のオーディオソース")]
        private List<AudioSource> _bgmAudioSources = new();

        [SerializeField]
        [Tooltip("SFX用のオーディオソース")]
        private List<AudioSource> _sfxAudioSources = new();

        #endregion

        #region Member Variables

        private int _newAudioSourceCount = 0;

        #endregion

        #region Unity Method

        private void Awake()
        {
            //オーディオソースのプレファブが無かったら
            if (_audioPrefab == null)
                CreateAudio();

            //BGMを格納する親オブジェクトが無かったら
            if (_bgmParent == null)
                CreateParent(out _bgmParent, "BGM");

            //SFXを格納する親オブジェクトが無かったら
            if (_sfxParent == null)
                CreateParent(out _sfxParent, "SFX");

            if (_audioPrefab.playOnAwake)
                _audioPrefab.playOnAwake = false;

#if UNITY_EDITOR

            this
                .ObserveEveryValueChanged
                    (soundManager => soundManager.MasterVolume)
                .Subscribe
                    (volume => ReflectMasterVolume());

            this
                .ObserveEveryValueChanged
                    (soundManager => soundManager.BGMVolume)
                .Subscribe
                    (volume => ReflectBGMVolume());

            this
                .ObserveEveryValueChanged
                    (soundManager => soundManager.SFXVolume)
                .Subscribe
                    (volume => ReflectSFXVolume());

#endif

            PlayBGM(_name);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 音楽(BGM)を再生する関数
        /// </summary>
        /// <param name="name">音楽(BGM)の名前</param>
        /// <param name="isLooping">ループにするか</param>
        /// <param name="volume">音の大きさ</param>
        public void PlayBGM(string name, bool isLooping = true, float volume = 1)
        {
            var bgmVolume = volume * _masterVolume * _bgmVolume;

            //BGMを止める
            foreach (var audioSource in _bgmAudioSources)
            {
                audioSource.Stop();
                audioSource.name = audioSource.name.Replace("♪ ", "");
            }

            if (name == "")
                return;

            AudioClip audioClip = null;

            //再生したい音を絞り込む
            foreach (var clip in _bgmDatas)
            {
                var clipName = clip.AudioClip.name == name;
                var nickName = clip.NickName == name;
                if (clipName || nickName)
                    audioClip = clip.AudioClip;
            }

            if (audioClip == null)
            {
                Debug.LogWarning("音楽が見つからなかった");
                return;
            }

            //再生したい音を格納しているオブジェクトから絞り込む
            foreach (var audioSource in _bgmAudioSources)
            {
                if (audioSource.clip != audioClip)
                    continue;

                audioSource.name = $"♪ {audioSource.name}";
                audioSource.volume = bgmVolume;
                audioSource.loop = isLooping;
                audioSource.Play();
            }

            //再生したい音をのAudioを生成
            var newAudioSource = Instantiate(_audioPrefab);
            newAudioSource.transform.SetParent(_bgmParent.transform);
            _bgmAudioSources.Add(newAudioSource);

            newAudioSource.volume = bgmVolume;
            newAudioSource.clip = audioClip;
            newAudioSource.name = $"New {audioClip.name}";
            newAudioSource.loop = isLooping;

            newAudioSource.Play();
        }

        /// <summary>
        /// 効果音(SFX)を再生する関数
        /// </summary>
        /// <param name="name">効果音(SFX)の名前</param>
        /// <param name="audioSource">効果音(SFX)の名前</param>
        /// <param name="volume">音の大きさ</param>
        public async void PlaySFX(string name, Transform parent = null, float volume = 1)
        {
            if (parent == null)
                parent = _sfxParent;

            var sfxVolume = volume * _masterVolume * _sfxVolume;

            //再生したい音を絞り込む
            foreach (var data in _sfxDatas)
            {
                var clipName = data.AudioClip.name != name;
                var nickName = data.NickName != name;
                if (clipName && nickName)
                    continue;

                //ClipがnullのAudioSourceを探す
                foreach (var audioSource in _sfxAudioSources)
                {
                    if (audioSource.clip != null)
                        continue;

                    audioSource.clip = data.AudioClip;
                    audioSource.volume = sfxVolume;
                    audioSource.gameObject.transform.SetParent(parent);
                    audioSource.transform.localPosition = Vector3.zero;

                    audioSource.Play();

                    //音を再生しているかを分かりやすくする
#if UNITY_EDITOR

                    var privName = audioSource.name;
                    audioSource.name = data.name;

                    await UniTask.WaitUntil(() => !audioSource.isPlaying && !IsPausing);

                    audioSource.name = privName;
                    audioSource.clip = null;
                    audioSource.gameObject.transform.SetParent(_sfxParent);
                    audioSource.transform.localPosition = Vector3.zero;

#endif

                    return;
                }

                //無かったら新しく作る
                var newAudioSource = Instantiate(_audioPrefab);
                newAudioSource.transform.SetParent(_sfxParent.transform);
                newAudioSource.transform.localPosition = Vector3.zero;

                newAudioSource.name = $"NewSFX {_newAudioSourceCount}";
                _newAudioSourceCount++;
                _sfxAudioSources.Add(newAudioSource);

                newAudioSource.clip = data.AudioClip;
                newAudioSource.volume = sfxVolume;

                if (parent != null)
                    newAudioSource.gameObject.transform.SetParent(parent);

                newAudioSource.Play();

                //音を再生しているかを分かりやすくする
#if UNITY_EDITOR

                var newPrivName = newAudioSource.name;
                newAudioSource.name = 
                    data.NickName != "" ? data.NickName : data.AudioClip.name;

                await UniTask.WaitUntil(() => !newAudioSource.isPlaying && !IsPausing);

                newAudioSource.name = newPrivName;
                newAudioSource.clip = null;
                newAudioSource.gameObject.transform.SetParent(_sfxParent);
                newAudioSource.transform.localPosition = Vector3.zero;

#endif

                return;
            }
            Debug.LogWarning("効果音が見つからなかった");
        }

        /// <summary>
        /// BGMをフェードする関数
        /// </summary>
        public async UniTask FadeBGM()
        {
            //BGMの音量を少しずつ下げる
            foreach (var audio in _bgmAudioSources)
                if (audio.isPlaying)
                    audio.DOFade(0, _fadeTime);

            await UniTask.Delay(TimeSpan.FromSeconds(_fadeTime));

            //BGMを止める
            foreach (var audioSource in _bgmAudioSources)
            {
                if (!audioSource.isPlaying)
                    continue;

                audioSource.Stop();
                audioSource.name = audioSource.name.Replace("♪ ", "");
                audioSource.volume = 1;
            }
        }

        /// <summary>
        /// マスター音量を変更する関数
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            _masterVolume = volume;
        }

        /// <summary>
        /// 音楽の音量を変更する関数
        /// </summary>
        public void SetBGMVolume(float volume)
        {
            _bgmVolume = volume;
        }

        /// <summary>
        /// 効果音の音量を変更する関数
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            _sfxVolume = volume;
        }

        /// <summary>
        /// 再生している全ての音の音量の変更を反映する関数
        /// </summary>
        public void ReflectMasterVolume()
        {
            ReflectBGMVolume();
            ReflectSFXVolume();
        }

        /// <summary>
        /// 再生している全ての音楽の音量を反映する関数
        /// </summary>
        public void ReflectBGMVolume()
        {
            foreach (var audioSource in _bgmAudioSources)
                if (audioSource.isPlaying)
                    audioSource.volume = _masterVolume * _bgmVolume;
        }

        /// <summary>
        /// 再生している全ての効果音の音量を変更を反映する関数
        /// </summary>
        public void ReflectSFXVolume()
        {
            foreach (var audioSource in _sfxAudioSources)
                if (audioSource.isPlaying)
                    audioSource.volume = _masterVolume * _sfxVolume;
        }

        /// <summary>
        /// ポーズ用の関数
        /// </summary>
        public void Pause()
        {
            IsPausing = true;

            foreach (var bgmAudio in _bgmAudioSources)
                if (bgmAudio.isPlaying)
                    bgmAudio.Pause();

            foreach (var sfxAudio in _sfxAudioSources)
                if (sfxAudio.isPlaying)
                    sfxAudio.Pause();
        }

        /// <summary>
        /// ポーズ解除用の関数
        /// </summary>
        public void Resume()
        {
            IsPausing = false;

            _bgmAudioSources.ForEach(bgm => bgm.UnPause());
            _sfxAudioSources.ForEach(sfx => sfx.UnPause());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// AudioSourceを格納するオブジェクトを生成する関数
        /// </summary>
        private void CreateParent(out Transform parent, string name)
        {
            var go = new GameObject();
            parent = go.transform;
            parent.name = name;
            parent.transform.SetParent(transform);
        }

        /// <summary>
        /// オーディオソースが付きオブジェクトを生成する関数
        /// </summary>
        private void CreateAudio()
        {
            var go = new GameObject();
            go.transform.SetParent(transform);
            _audioPrefab = go.AddComponent<AudioSource>();
            _audioPrefab.playOnAwake = false;
            _audioPrefab.name = "AudioSource";
        }

        /// <summary>
        /// 子オブジェクトのオーディオソースを全削除する関数
        /// </summary>
        /// <param name="parent"></param>
        private void InitAudioSource(ref Transform parent)
        {
            if (parent == null)
                return;

            while (parent.childCount != 0)
                DestroyImmediate(parent.GetChild(0).gameObject);
        }

        #endregion

#if UNITY_EDITOR
        #region Editor Methods

        /// <summary>
        /// BGM用のPrefabを生成する関数
        /// </summary>
        public void CreateBGM()
        {
            if (_audioPrefab == null) CreateAudio();
            if (_bgmParent == null) CreateParent(out _bgmParent, "BGM");

            IsStopingToCreate = false;

            _bgmAudioSources.Clear();

            InitAudioSource(ref _bgmParent);

            for (var i = 0; i < _bgmDatas.Length; i++)
            {
                var audio = Instantiate(_audioPrefab);
                audio.transform.SetParent(_bgmParent.transform);
                audio.transform.localPosition = Vector2.zero;
                _bgmAudioSources.Add(audio);

                audio.name = _bgmDatas[i].NickName;
                audio.clip = _bgmDatas[i].AudioClip;
                if (audio.clip != null) audio.clip.name = _bgmDatas[i].NickName;
                audio.loop = true;
            }

            _bgmAudioSources = new(_bgmAudioSources.Distinct());
        }

        /// <summary>
        /// SFX用のPrefabを生成する関数
        /// </summary>
        public void CreateSFX()
        {
            if (_audioPrefab == null) CreateAudio();
            if (_sfxParent == null) CreateParent(out _sfxParent, "SFX");

            IsStopingToCreate = false;

            _sfxAudioSources.Clear();

            InitAudioSource(ref _sfxParent);

            for (var i = 0; i < AudioSourceCount; i++)
            {
                var audio = Instantiate(_audioPrefab);
                audio.transform.SetParent(_sfxParent.transform);
                audio.transform.localPosition = Vector2.zero;

                audio.loop = false;
                audio.name = $"SFX {i.ToString("D3")}";

                _sfxAudioSources.Add(audio);
            }
        }

        /// <summary>
        /// BGM&SFX用のPrefabを全削除する関数
        /// </summary>
        public void Init()
        {
            IsStopingToCreate = true;

            _bgmAudioSources.Clear();
            _sfxAudioSources.Clear();

            InitAudioSource(ref _bgmParent);
            InitAudioSource(ref _sfxParent);
        }

        public void ResizeBGMClips(int length)
        {
            _bgmDatas = new BGMData[length];
        }

        public void ResizeSFXClips(int length)
        {
            _sfxDatas = new SFXData[length];
        }

        public void AddBGMClip(int index, BGMData clip)
        {
            _bgmDatas[index] = clip;
        }

        public void AddSFXClip(int index, SFXData clip)
        {
            _sfxDatas[index] = clip;
        }

        public void SetAudioSourceCount(int count)
        {
            AudioSourceCount = count;
        }

        #endregion
#endif
    }
}
