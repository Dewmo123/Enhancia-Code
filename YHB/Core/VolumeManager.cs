using Assets._00.Work.AKH.Scripts.Core.EventSystem;
using Core.EventSystem;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets._00.Work.YHB.Scripts.Core
{
    public class VolumeManager : MonoBehaviour
    {
        [SerializeField] private EventChannelSO volumeChangeEventChannel;
        [SerializeField] private EventChannelSO generateEventChannel;
        [SerializeField] private EventChannelSO inStageEventChannel;
        [SerializeField] private Volume defaultVolume;
        [SerializeField] private float volumeChangeBlendTime = 1f;

        private Volume _previousVolume;
        private Volume _currentVolume;
        private DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> _previousTween;
        private DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> _currentTween;

        private void Awake()
        {
            _currentVolume = defaultVolume;

            volumeChangeEventChannel.AddListener<VolumeChangeEvent>(HandleVolumeChangeEvent);
            generateEventChannel.AddListener<GenerateCompleteEvent>(HandleGenerateCompleteEvent);
            inStageEventChannel.AddListener<StageEndEvent>(HandleStageEndEvent);
        }

        private void OnDestroy()
        {
            volumeChangeEventChannel.RemoveListener<VolumeChangeEvent>(HandleVolumeChangeEvent);
            generateEventChannel.RemoveListener<GenerateCompleteEvent>(HandleGenerateCompleteEvent);
            inStageEventChannel.RemoveListener<StageEndEvent>(HandleStageEndEvent);
        }

        private void ChangeVolume(Volume newVolume)
        {
            _previousVolume = _currentVolume;
            _currentVolume = newVolume;

            if (_previousVolume != null)
            {
                _previousTween?.Kill();
                _previousTween = DOTween.To(() => _previousVolume.weight,
                    value => _previousVolume.weight = value,
                    0f, volumeChangeBlendTime)
                    .SetEase(Ease.Linear)
                    .OnComplete(() => _previousVolume.gameObject.SetActive(false));
            }

            if (newVolume != null)
            {
                _currentVolume.gameObject.SetActive(true);

                _currentTween?.Kill();
                _currentTween = DOTween.To(() => _currentVolume.weight,
                    value => _currentVolume.weight = value,
                    1f, volumeChangeBlendTime)
                    .SetEase(Ease.Linear);
            }
        }

        private void HandleVolumeChangeEvent(VolumeChangeEvent volumeChangeEvent)
        {
            if (EqualityComparer<Volume>.Default.Equals(volumeChangeEvent.nextVolume ?? defaultVolume, _currentVolume))
                ChangeVolume(volumeChangeEvent.previousVolume ?? defaultVolume);
            else if (EqualityComparer<Volume>.Default.Equals(volumeChangeEvent.previousVolume ?? defaultVolume, _currentVolume))
                ChangeVolume(volumeChangeEvent.nextVolume ?? defaultVolume);
        }

        private void HandleGenerateCompleteEvent(GenerateCompleteEvent evt)
        {
            if (!evt.success)
                return;

            List<Volume> volumes = evt.volumes.ToList();

            volumes
                .Where(volume => volume != null)
                .ToList()
                .ForEach(volume =>
                {
                    volume.gameObject.SetActive(false);
                    volume.weight = 0f;
                });

            ChangeVolume(volumes[0] ?? defaultVolume);
        }

        private void HandleStageEndEvent(StageEndEvent evt)
        {
            _previousTween?.Kill();
            _currentTween?.Kill();
            _previousVolume = null;
            _currentVolume = null;
        }
    }
}
