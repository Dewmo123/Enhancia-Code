using DG.Tweening;
using UnityEngine.Audio;
using UnityEngine;
using DewmoLib.ObjectPool.RunTime;

namespace Scripts.Core.Sound
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour, IPoolable
    {
        [SerializeField] private AudioMixerGroup _sfxGroup, _musicGroup;
        [SerializeField] private PoolItemSO _poolItem;
        private Pool _myPool;

        public GameObject GameObject => gameObject;

        public PoolItemSO PoolItem => _poolItem;

        private AudioSource _audioSource;
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }
        public void PlaySound(SoundSO data, Vector3 pos)
        {
            transform.position = pos;
            _audioSource.spatialBlend = data.spatialBlend;
            if (data.audioType == SoundSO.AudioType.SFX)
            {
                _audioSource.outputAudioMixerGroup = _sfxGroup;
            }
            else if (data.audioType == SoundSO.AudioType.Music)
            {
                _audioSource.outputAudioMixerGroup = _musicGroup;
            }
            _audioSource.volume = data.volume;
            _audioSource.pitch = data.basePitch;
            if (data.randomizePitch)
            {
                _audioSource.pitch += Random.Range(-data.randomPitchModifier, data.randomPitchModifier);
            }
            _audioSource.clip = data.clip;
            _audioSource.loop = data.loop;
            if (!data.loop)
            {
                float time = _audioSource.clip.length + 0.2f;
                DOVirtual.DelayedCall(time, () => _myPool.Push(this));
            }
            _audioSource.Play();
        }
        public void ResetItem()
        {

        }

        public void StopAndGoToPool()
        {
            _audioSource.Stop();
            _myPool.Push(this);
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }
    }

}