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
    /// �T�E���h���Ǘ�����Script
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
        [Header("�ŏ��ɗ���BGM")]
        private string _name = "";

        [SerializeField]
        [Header("�}�X�^�[����")]
        [Range(0, 1)]
        private float _masterVolume = 1;

        [SerializeField]
        [Header("���y�̉���")]
        [Range(0, 1)]
        private float _bgmVolume = 1;

        [SerializeField]
        [Header("���ʉ��̉���")]
        [Range(0, 1)]
        private float _sfxVolume = 1;

        [SerializeField]
        [Header("����������܂ł̎���")]
        private float _fadeTime = 2f;

        [SerializeField]
        [Header("�|�[�Y�}�l�[�W���[")]
        PauseManager _pauseManager;

        [SerializeField]
        [Header("���y���i�[����I�u�W�F�N�g")]
        private Transform _bGMParent = null;

        [SerializeField]
        [Header("���ʉ����i�[����I�u�W�F�N�g")]
        private Transform _sFXParent = null;

        [SerializeField]
        [Header("�I�[�f�B�I�\�[�X�����Ă���v���t�@�u")]
        private AudioSource _audioPrefab = null;

        [SerializeField]
        [Header("���y�̃f�[�^")]
        private BGMData[] _bgmDatas;

        [SerializeField]
        [Header("���ʉ��̃f�[�^")]
        private SFXData[] _sfxDatas;

        [SerializeField]
        [Tooltip("BGM�p�̃I�[�f�B�I�\�[�X")]
        private List<AudioSource> _bgmAudioSources = new();

        [SerializeField]
        [Tooltip("SFX�p�̃I�[�f�B�I�\�[�X")]
        private List<AudioSource> _sfxAudioSources = new();

        #endregion

        #region Private Variables

        private int _newAudioSourceCount = 0;
        private bool _isPausing = false;

        #endregion

        #region Unity Method

        private void Awake()
        {
            //�I�[�f�B�I�\�[�X�̃v���t�@�u������������
            if (_audioPrefab == null) CreateAudio();

            //BGM���i�[����e�I�u�W�F�N�g������������
            if (_bGMParent == null) CreateBGMParent();

            //SFX���i�[����e�I�u�W�F�N�g������������
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
        /// ���y(BGM)���Đ�����֐�
        /// </summary>
        /// <param name="name">���y(BGM)�̖��O</param>
        /// <param name="isLooping"></param>
        /// <param name="volume">���̑傫��</param>
        public void PlayBGM(string name, bool isLooping = true, float volume = 1)
        {
            var bgmVolume = volume * _masterVolume * _bgmVolume;

            //BGM���~�߂�
            foreach (var audioSource in _bgmAudioSources)
            {
                audioSource.Stop();
                audioSource.name = audioSource.clip.name;
            }

            if (name == "") return;

            //�Đ������������i�[���Ă���I�u�W�F�N�g����i�荞��
            foreach (var audioSource in _bgmAudioSources)
            {
                if (audioSource.clip.name == name)
                {
                    audioSource.name = $"�� {audioSource.name}";
                    audioSource.volume = bgmVolume;
                    audioSource.loop = isLooping;

                    audioSource.Play();

                    return;
                }
            }

            //�Đ������������i�[���Ă���I�u�W�F�N�g������������
            //�Đ������������i�荞��
            foreach (var clip in _bgmDatas)
            {
                if (clip.name == name || clip.Name == name)
                {
                    //�Đ�������������Audio�𐶐�
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
            Debug.LogWarning("���y��������Ȃ�����");
        }

        /// <summary>
        /// ���ʉ�(SFX)���Đ�����֐�
        /// </summary>
        /// <param name="name">���ʉ�(SFX)�̖��O</param>
        /// <param name="volume">���̑傫��</param>
        async public void PlaySFX(string name, float volume = 1)
        {
            var sfxVolume = volume * _masterVolume * _sfxVolume;

            //�Đ������������i�荞��
            foreach (var data in _sfxDatas)
            {
                if (data.name == name || data.Name == name)
                {
                    //Clip��null��AudioSource��T��
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

                    //����������V�������
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
            Debug.LogWarning("���ʉ���������Ȃ�����");
        }

        /// <summary>
        /// ���ʉ�(SFX)���Đ�����֐�
        /// </summary>
        /// <param name="name">���ʉ�(SFX)�̖��O</param>
        /// <param name="audioSource">���ʉ�(SFX)�̖��O</param>
        /// <param name="volume">���̑傫��</param>
        async public void PlaySFX(string name, Transform parent, float volume = 1)
        {
            var sfxVolume = volume * _masterVolume * _sfxVolume;

            //�Đ������������i�荞��
            foreach (var data in _sfxDatas)
            {
                if (data.name == name || data.Name == name)
                {
                    //Clip��null��AudioSource��T��
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

                    //����������V�������
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
            Debug.LogWarning("���ʉ���������Ȃ�����");
        }


        /// <summary>
        /// BGM���t�F�[�h����֐�
        /// </summary>
        async public UniTask FadeBGM()
        {
            //BGM�̉��ʂ�������������
            foreach (var audio in _bgmAudioSources)
            {
                if (audio.isPlaying) audio.DOFade(0, _fadeTime);
            }
            await UniTask.Delay(TimeSpan.FromSeconds(_fadeTime));

            //BGM���~�߂�
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
        /// �}�X�^�[���ʂ�ύX����֐�
        /// </summary>
        /// <param name="masterVolume">�}�X�^�[����</param>
        public void SetMasterVolume(float masterVolume)
        {
            _masterVolume = masterVolume;
        }

        /// <summary>
        /// ���y�̉��ʂ�ύX����֐�
        /// </summary>
        /// <param name="bgmVolume">���y�̉���</param>
        public void SetBGMVolume(float bgmVolume)
        {
            _bgmVolume = bgmVolume;
        }

        /// <summary>
        /// ���ʉ��̉��ʂ�ύX����֐�
        /// </summary>
        /// <param name="sfxVolume">���ʉ��̉���</param>
        public void SetSFXVolume(float sfxVolume)
        {
            _sfxVolume = sfxVolume;
        }

        /// <summary>
        /// �Đ����Ă���S�Ẳ��̉��ʂ̕ύX�𔽉f����֐�
        /// </summary>
        public void ReflectMasterVolume()
        {
            ReflectBGMVolume();
            ReflectSFXVolume();
        }

        /// <summary>
        /// �Đ����Ă���S�Ẳ��y�̉��ʂ𔽉f����֐�
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
        /// �Đ����Ă���S�Ă̌��ʉ��̉��ʂ�ύX�𔽉f����֐�
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
        /// ��������SFX�pAudio�̐���ύX����֐�
        /// </summary>
        /// <param name="count">��������Audio�̐�</param>
        public void SetAudioCount(int count)
        {
            AudioSourceCount = count;
        }

        /// <summary>
        /// BGM�p��Prefab�𐶐�����֐�
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
        /// SFX�p��Prefab�𐶐�����֐�
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
        /// BGM&SFX�p��Prefab��S�폜����֐�
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
        /// ���y���i�[����I�u�W�F�N�g�𐶐�����֐�
        /// </summary>
        private void CreateBGMParent()
        {
            var go = new GameObject();
            _bGMParent = go.transform;
            _bGMParent.name = "BGM";
            _bGMParent.transform.parent = transform;
        }

        /// <summary>
        /// ���ʉ����i�[����I�u�W�F�N�g�𐶐�����֐�
        /// </summary>
        private void CreateSFXParent()
        {
            var go = new GameObject();
            _sFXParent = go.transform;
            _sFXParent.name = "SFX";
            _sFXParent.transform.parent = transform;
        }

        /// <summary>
        /// �I�[�f�B�I�\�[�X���t���I�u�W�F�N�g�𐶐�����֐�
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
        /// BGM�p��Prefab��S�폜����֐�
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
        /// SFX�p��Prefab��S�폜����֐�
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
        /// �|�[�Y�p�̊֐�
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
        /// �|�[�Y�����p�̊֐�
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
