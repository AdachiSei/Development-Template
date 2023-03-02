using System;
using System.IO;
using System.Collections;
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
    public class SoundManager : MonoBehaviour
    {
        #region Properties

        public float MasterVolume => _masterVolume;
        public float BGMVolume => _bgmVolume;
        public float SFXVolume => _sfxVolume;
        public int AudioSourceCount { get; private set; }
        public bool IsStopingToCreate { get; private set; }

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
        [Header("ポーズマネージャー")]
        PauseManager _pauseManager;

        [SerializeField]
        [Header("音楽を格納するオブジェクト")]
        private Transform _bGMParent = null;

        [SerializeField]
        [Header("効果音を格納するオブジェクト")]
        private Transform _sFXParent = null;

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

        #region Private Variables

        private int _newAudioSourceCount = 0;
        private bool _isPausing = false;

        #endregion

        #region Unity Method

        private void Awake()
        {
            //オーディオソースのプレファブが無かったら
            if (_audioPrefab == null) CreateAudio();

            //BGMを格納する親オブジェクトが無かったら
            if (_bGMParent == null) CreateBGMParent();

            //SFXを格納する親オブジェクトが無かったら
            if (_sFXParent == null) CreateSFXParent();

            if (_audioPrefab.playOnAwake) _audioPrefab.playOnAwake = false;

            var soundManager = this;

            soundManager
                .ObserveEveryValueChanged
                    (soundManager => soundManager.MasterVolume)
                .Subscribe
                    (volume => ReflectMasterVolume());

            soundManager
                .ObserveEveryValueChanged
                    (soundManager => soundManager.BGMVolume)
                .Subscribe
                    (volume => ReflectBGMVolume());

            soundManager
                .ObserveEveryValueChanged
                    (soundManager => soundManager.SFXVolume)
                .Subscribe
                    (volume => ReflectSFXVolume());

            PlayBGM(_name);

            if(_pauseManager)
            {
                _pauseManager.OnPause += Pause;
                _pauseManager.OnResume += Resume;
            }
        }

        //private void OnDestroy()
        //{
        //    if (PauseManager.Instance.IsDontDestroy)
        //    {
        //        PauseManager.Instance.OnPause -= Pause;
        //        PauseManager.Instance.OnResume -= Resume;
        //    }
        //}

        #endregion

        #region Public Methods

        /// <summary>
        /// 音楽(BGM)を再生する関数
        /// </summary>
        /// <param name="name">音楽(BGM)の名前</param>
        /// <param name="isLooping"></param>
        /// <param name="volume">音の大きさ</param>
        public void PlayBGM(string name, bool isLooping = true, float volume = 1)
        {
            var bgmVolume = volume * _masterVolume * _bgmVolume;

            //BGMを止める
            foreach (var audioSource in _bgmAudioSources)
            {
                audioSource.Stop();
                audioSource.name = audioSource.clip.name;
            }

            if (name == "") return;

            //再生したい音を格納しているオブジェクトから絞り込む
            foreach (var audioSource in _bgmAudioSources)
            {
                if (audioSource.clip.name == name)
                {
                    audioSource.name = $"♪ {audioSource.name}";
                    audioSource.volume = bgmVolume;
                    audioSource.loop = isLooping;

                    audioSource.Play();

                    return;
                }
            }

            //再生したい音を格納しているオブジェクトが無かったら
            //再生したい音を絞り込む
            foreach (var clip in _bgmDatas)
            {
                if (clip.name == name || clip.Name == name)
                {
                    //再生したい音をのAudioを生成
                    var newAudioSource = Instantiate(_audioPrefab);
                    newAudioSource.transform.SetParent(_bGMParent.transform);
                    _bgmAudioSources.Add(newAudioSource);

                    newAudioSource.volume = bgmVolume;
                    newAudioSource.clip = clip.BGMClip;
                    newAudioSource.name = $"New {clip.name}";
                    newAudioSource.loop = isLooping;

                    newAudioSource.Play();

                    return;
                }
            }
            Debug.LogWarning("音楽が見つからなかった");
        }

        /// <summary>
        /// 効果音(SFX)を再生する関数
        /// </summary>
        /// <param name="name">効果音(SFX)の名前</param>
        /// <param name="volume">音の大きさ</param>
        async public void PlaySFX(string name, float volume = 1)
        {
            var sfxVolume = volume * _masterVolume * _sfxVolume;

            //再生したい音を絞り込む
            foreach (var data in _sfxDatas)
            {
                if (data.name == name || data.Name == name)
                {
                    //ClipがnullのAudioSourceを探す
                    foreach (var audioSource in _sfxAudioSources)
                    {
                        if (audioSource.clip == null)
                        {
                            audioSource.clip = data.SFXClip;
                            audioSource.volume = sfxVolume;

                            var privName = audioSource.name;
                            audioSource.name = data.name;
                            audioSource.Play();

                            await UniTask.WaitUntil(() => !audioSource.isPlaying && !_isPausing);
                            
                            audioSource.name = privName;
                            audioSource.clip = null;
                            return;
                        }
                    }

                    //無かったら新しく作る
                    var newAudioSource = Instantiate(_audioPrefab);
                    newAudioSource.transform.SetParent(_sFXParent.transform);

                    newAudioSource.name = $"NewSFX {_newAudioSourceCount}";
                    _newAudioSourceCount++;
                    _sfxAudioSources.Add(newAudioSource);

                    newAudioSource.clip = data.SFXClip;
                    newAudioSource.volume = sfxVolume;

                    var newPrivName = newAudioSource.name;
                    newAudioSource.name = data.name;

                    newAudioSource.Play();

                    await UniTask.WaitUntil(() => !newAudioSource.isPlaying && !_isPausing);

                    newAudioSource.name = newPrivName;
                    newAudioSource.clip = null;
                    return;
                }
            }
            Debug.LogWarning("効果音が見つからなかった");
        }

        /// <summary>
        /// 効果音(SFX)を再生する関数
        /// </summary>
        /// <param name="name">効果音(SFX)の名前</param>
        /// <param name="audioSource">効果音(SFX)の名前</param>
        /// <param name="volume">音の大きさ</param>
        async public void PlaySFX(string name, Transform parent, float volume = 1)
        {
            var sfxVolume = volume * _masterVolume * _sfxVolume;

            //再生したい音を絞り込む
            foreach (var data in _sfxDatas)
            {
                if (data.name == name || data.Name == name)
                {
                    //ClipがnullのAudioSourceを探す
                    foreach (var audioSource in _sfxAudioSources)
                    {
                        if (audioSource.clip == null)
                        {
                            audioSource.clip = data.SFXClip;
                            audioSource.volume = sfxVolume;
                            audioSource.gameObject.transform.SetParent(parent);

                            var privName = audioSource.name;
                            audioSource.name = data.name;
                            audioSource.Play();

                            await UniTask.WaitUntil(() => !audioSource.isPlaying && !_isPausing);
                            
                            audioSource.name = privName;
                            audioSource.clip = null;
                            audioSource.gameObject.transform.SetParent(_sFXParent);
                            return;
                        }
                    }

                    //無かったら新しく作る
                    var newAudioSource = Instantiate(_audioPrefab);
                    newAudioSource.transform.SetParent(_sFXParent.transform);

                    newAudioSource.name = $"NewSFX {_newAudioSourceCount}";
                    _newAudioSourceCount++;
                    _sfxAudioSources.Add(newAudioSource);

                    newAudioSource.clip = data.SFXClip;
                    newAudioSource.volume = sfxVolume;
                    newAudioSource.gameObject.transform.SetParent(parent);

                    var newPrivName = newAudioSource.name;
                    newAudioSource.name = data.name;

                    newAudioSource.Play();

                    await UniTask.WaitUntil(() => !newAudioSource.isPlaying && !_isPausing);

                    newAudioSource.name = newPrivName;
                    newAudioSource.clip = null;
                    newAudioSource.gameObject.transform.SetParent(_sFXParent);
                    return;
                }
            }
            Debug.LogWarning("効果音が見つからなかった");
        }


        /// <summary>
        /// BGMをフェードする関数
        /// </summary>
        async public UniTask FadeBGM()
        {
            //BGMの音量を少しずつ下げる
            foreach (var audio in _bgmAudioSources)
            {
                if (audio.isPlaying) audio.DOFade(0, _fadeTime);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(_fadeTime));

            //BGMを止める
            foreach (var audio in _bgmAudioSources)
            {
                if (audio.isPlaying)
                {
                    audio.Stop();
                    audio.name = audio.clip.name;
                    audio.volume = 1;
                }
            }
        }

        /// <summary>
        /// マスター音量を変更する関数
        /// </summary>
        /// <param name="masterVolume">マスター音量</param>
        public void SetMasterVolume(float masterVolume)
        {
            _masterVolume = masterVolume;
        }

        /// <summary>
        /// 音楽の音量を変更する関数
        /// </summary>
        /// <param name="bgmVolume">音楽の音量</param>
        public void SetBGMVolume(float bgmVolume)
        {
            _bgmVolume = bgmVolume;
        }

        /// <summary>
        /// 効果音の音量を変更する関数
        /// </summary>
        /// <param name="sfxVolume">効果音の音量</param>
        public void SetSFXVolume(float sfxVolume)
        {
            _sfxVolume = sfxVolume;
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
            {
                if (audioSource.isPlaying)
                {
                    audioSource.volume = _masterVolume * _bgmVolume;
                }
            }
        }

        /// <summary>
        /// 再生している全ての効果音の音量を変更を反映する関数
        /// </summary>
        public void ReflectSFXVolume()
        {
            foreach (var audioSource in _sfxAudioSources)
            {
                if (audioSource.isPlaying)
                {
                    audioSource.volume = _masterVolume * _sfxVolume;
                }
            }
        }

        #region Inspector Methods

        /// <summary>
        /// 生成するSFX用Audioの数を変更する関数
        /// </summary>
        /// <param name="count">生成するAudioの数</param>
        public void SetAudioCount(int count)
        {
            AudioSourceCount = count;
        }

        /// <summary>
        /// BGM用のPrefabを生成する関数
        /// </summary>
        public void CreateBGM()
        {
            if (_audioPrefab == null) CreateAudio();
            if (_bGMParent == null) CreateBGMParent();

            IsStopingToCreate = false;

            _bgmAudioSources.Clear();

            InitBGM();

            for (var i = 0; i < _bgmDatas.Length; i++)
            {
                var audio = Instantiate(_audioPrefab);
                audio.transform.SetParent(_bGMParent.transform);
                _bgmAudioSources.Add(audio);

                audio.name = _bgmDatas[i].Name;
                audio.clip = _bgmDatas[i].BGMClip;
                if (audio.clip != null) audio.clip.name = _bgmDatas[i].Name;
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
            if (_sFXParent == null) CreateSFXParent();

            IsStopingToCreate = false;

            _sfxAudioSources.Clear();

            InitSFX();

            for (var i = 0; i < AudioSourceCount; i++)
            {
                var audio = Instantiate(_audioPrefab);
                audio.transform.SetParent(_sFXParent.transform);

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

            InitBGM();
            InitSFX();
        }

        #endregion

        #region Editor Methods

        public void ResizeBGMClips(int length) =>
            Array.Resize(ref _bgmDatas, length);

        public void ResizeSFXClips(int length) =>
            Array.Resize(ref _sfxDatas, length);

        public void AddBGMClip(int index, BGMData clip) =>
            _bgmDatas[index] = clip;

        public void AddSFXClip(int index, SFXData clip) =>
            _sfxDatas[index] = clip;

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// 音楽を格納するオブジェクトを生成する関数
        /// </summary>
        private void CreateBGMParent()
        {
            var go = new GameObject();
            _bGMParent = go.transform;
            _bGMParent.name = "BGM";
            _bGMParent.transform.parent = transform;
        }

        /// <summary>
        /// 効果音を格納するオブジェクトを生成する関数
        /// </summary>
        private void CreateSFXParent()
        {
            var go = new GameObject();
            _sFXParent = go.transform;
            _sFXParent.name = "SFX";
            _sFXParent.transform.parent = transform;
        }

        /// <summary>
        /// オーディオソースが付きオブジェクトを生成する関数
        /// </summary>
        private void CreateAudio()
        {
            var go = new GameObject();
            go.transform.parent = transform;
            _audioPrefab = go.AddComponent<AudioSource>();
            _audioPrefab.playOnAwake = false;
            _audioPrefab.name = "XD";
        }

        /// <summary>
        /// BGM用のPrefabを全削除する関数
        /// </summary>
        private void InitBGM()
        {
            if (_bGMParent == null) return;

            while (true)
            {
                var children = _bGMParent.transform;
                var empty = children.childCount == 0;
                if (empty) break;
                var go = children.GetChild(0).gameObject;
                DestroyImmediate(go);
            }
        }

        /// <summary>
        /// SFX用のPrefabを全削除する関数
        /// </summary>
        private void InitSFX()
        {
            if (_sFXParent == null) return;

            while (true)
            {
                var children = _sFXParent.transform;
                var empty = children.childCount == 0;
                if (empty) break;
                var go = children.GetChild(0).gameObject;
                DestroyImmediate(go);
            }
        }

        /// <summary>
        /// ポーズ用の関数
        /// </summary>
        private void Pause()
        {
            _isPausing = true;

            foreach (var bGMAudio in _bgmAudioSources)
            {
                if (bGMAudio.isPlaying) bGMAudio.Pause();
            }
            foreach (var sFXAudio in _sfxAudioSources)
            {
                if (sFXAudio.isPlaying) sFXAudio.Pause();
            }
        }

        /// <summary>
        /// ポーズ解除用の関数
        /// </summary>
        private void Resume()
        {
            _isPausing = false;

            foreach (var bgm in _bgmAudioSources) bgm.UnPause();
            foreach (var sfx in _sfxAudioSources) sfx.UnPause();
        }

        #endregion
    }
}
