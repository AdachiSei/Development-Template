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
    /// �T�E���h���Ǘ�����Script
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
        [Header("���y���i�[����I�u�W�F�N�g")]
        private Transform _bgmParent = null;

        [SerializeField]
        [Header("���ʉ����i�[����I�u�W�F�N�g")]
        private Transform _sfxParent = null;

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

        #region Member Variables

        private int _newAudioSourceCount = 0;

        #endregion

        #region Unity Method

        private void Awake()
        {
            //�I�[�f�B�I�\�[�X�̃v���t�@�u������������
            if (_audioPrefab == null)
                CreateAudio();

            //BGM���i�[����e�I�u�W�F�N�g������������
            if (_bgmParent == null)
                CreateParent(out _bgmParent, "BGM");

            //SFX���i�[����e�I�u�W�F�N�g������������
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
        /// ���y(BGM)���Đ�����֐�
        /// </summary>
        /// <param name="name">���y(BGM)�̖��O</param>
        /// <param name="isLooping">���[�v�ɂ��邩</param>
        /// <param name="volume">���̑傫��</param>
        public void PlayBGM(string name, bool isLooping = true, float volume = 1)
        {
            var bgmVolume = volume * _masterVolume * _bgmVolume;

            //BGM���~�߂�
            foreach (var audioSource in _bgmAudioSources)
            {
                audioSource.Stop();
                audioSource.name = audioSource.name.Replace("�� ", "");
            }

            if (name == "")
                return;

            AudioClip audioClip = null;

            //�Đ������������i�荞��
            foreach (var clip in _bgmDatas)
            {
                var clipName = clip.AudioClip.name == name;
                var nickName = clip.NickName == name;
                if (clipName || nickName)
                    audioClip = clip.AudioClip;
            }

            if (audioClip == null)
            {
                Debug.LogWarning("���y��������Ȃ�����");
                return;
            }

            //�Đ������������i�[���Ă���I�u�W�F�N�g����i�荞��
            foreach (var audioSource in _bgmAudioSources)
            {
                if (audioSource.clip != audioClip)
                    continue;

                audioSource.name = $"�� {audioSource.name}";
                audioSource.volume = bgmVolume;
                audioSource.loop = isLooping;
                audioSource.Play();
            }

            //�Đ�������������Audio�𐶐�
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
        /// ���ʉ�(SFX)���Đ�����֐�
        /// </summary>
        /// <param name="name">���ʉ�(SFX)�̖��O</param>
        /// <param name="audioSource">���ʉ�(SFX)�̖��O</param>
        /// <param name="volume">���̑傫��</param>
        public async void PlaySFX(string name, Transform parent = null, float volume = 1)
        {
            if (parent == null)
                parent = _sfxParent;

            var sfxVolume = volume * _masterVolume * _sfxVolume;

            //�Đ������������i�荞��
            foreach (var data in _sfxDatas)
            {
                var clipName = data.AudioClip.name != name;
                var nickName = data.NickName != name;
                if (clipName && nickName)
                    continue;

                //Clip��null��AudioSource��T��
                foreach (var audioSource in _sfxAudioSources)
                {
                    if (audioSource.clip != null)
                        continue;

                    audioSource.clip = data.AudioClip;
                    audioSource.volume = sfxVolume;
                    audioSource.gameObject.transform.SetParent(parent);
                    audioSource.transform.localPosition = Vector3.zero;

                    audioSource.Play();

                    //�����Đ����Ă��邩�𕪂���₷������
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

                //����������V�������
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

                //�����Đ����Ă��邩�𕪂���₷������
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
            Debug.LogWarning("���ʉ���������Ȃ�����");
        }

        /// <summary>
        /// BGM���t�F�[�h����֐�
        /// </summary>
        public async UniTask FadeBGM()
        {
            //BGM�̉��ʂ�������������
            foreach (var audio in _bgmAudioSources)
                if (audio.isPlaying)
                    audio.DOFade(0, _fadeTime);

            await UniTask.Delay(TimeSpan.FromSeconds(_fadeTime));

            //BGM���~�߂�
            foreach (var audioSource in _bgmAudioSources)
            {
                if (!audioSource.isPlaying)
                    continue;

                audioSource.Stop();
                audioSource.name = audioSource.name.Replace("�� ", "");
                audioSource.volume = 1;
            }
        }

        /// <summary>
        /// �}�X�^�[���ʂ�ύX����֐�
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            _masterVolume = volume;
        }

        /// <summary>
        /// ���y�̉��ʂ�ύX����֐�
        /// </summary>
        public void SetBGMVolume(float volume)
        {
            _bgmVolume = volume;
        }

        /// <summary>
        /// ���ʉ��̉��ʂ�ύX����֐�
        /// </summary>
        public void SetSFXVolume(float volume)
        {
            _sfxVolume = volume;
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
                if (audioSource.isPlaying)
                    audioSource.volume = _masterVolume * _bgmVolume;
        }

        /// <summary>
        /// �Đ����Ă���S�Ă̌��ʉ��̉��ʂ�ύX�𔽉f����֐�
        /// </summary>
        public void ReflectSFXVolume()
        {
            foreach (var audioSource in _sfxAudioSources)
                if (audioSource.isPlaying)
                    audioSource.volume = _masterVolume * _sfxVolume;
        }

        /// <summary>
        /// �|�[�Y�p�̊֐�
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
        /// �|�[�Y�����p�̊֐�
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
        /// AudioSource���i�[����I�u�W�F�N�g�𐶐�����֐�
        /// </summary>
        private void CreateParent(out Transform parent, string name)
        {
            var go = new GameObject();
            parent = go.transform;
            parent.name = name;
            parent.transform.SetParent(transform);
        }

        /// <summary>
        /// �I�[�f�B�I�\�[�X���t���I�u�W�F�N�g�𐶐�����֐�
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
        /// �q�I�u�W�F�N�g�̃I�[�f�B�I�\�[�X��S�폜����֐�
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
        /// BGM�p��Prefab�𐶐�����֐�
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
        /// SFX�p��Prefab�𐶐�����֐�
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
        /// BGM&SFX�p��Prefab��S�폜����֐�
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
