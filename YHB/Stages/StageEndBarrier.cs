using Assets._00.Work.AKH.Scripts.Core.EventSystem;
using Core.EventSystem;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Stages
{
    public class StageEndBarrier : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private EventChannelSO generateEventChannel;
        [SerializeField] private EventChannelSO inStageEventChannel;
        [SerializeField] private TextMeshPro canSkipText;
        [SerializeField] private GameObject barrier;
        [SerializeField] private Vector2 skipTextOffset;
        [SerializeField] private float clearPercent;

        private Transform _target;

        private int _enemyCount;
        private int _currentEnemyCount;

        private bool _complete;
        private bool _active;
        private bool _clear;

        private float _clearPercent;

        private void Awake()
        {
            _clearPercent = clearPercent / 100f;

            inStageEventChannel.AddListener<EnemyCountChangeEvent>(HandleEnemyCountChange);
            generateEventChannel.AddListener<GenerateCompleteEvent>(HandleGenerateCompleteEvent);
        }

        private void OnDestroy()
        {
            inStageEventChannel.RemoveListener<EnemyCountChangeEvent>(HandleEnemyCountChange);
            generateEventChannel.RemoveListener<GenerateCompleteEvent>(HandleGenerateCompleteEvent);
        }

        private void OnEnable()
        {
            barrier.SetActive(true);

            _complete = false;
            _active = false;
            _clear = false;
            _enemyCount = 0;
            _currentEnemyCount = 0;
        }

        private async void HandleGenerateCompleteEvent(GenerateCompleteEvent @event)
        {
            await Task.Yield();
            _complete = @event.success;
            if (_enemyCount <= 0)
            {
                ClaerStage();
            }
        }

        private void HandleEnemyCountChange(EnemyCountChangeEvent @event)
        {
            if (_complete)
            {
                _currentEnemyCount += @event.count;
                if (((float)_currentEnemyCount) / _enemyCount < _clearPercent)
                    ClaerStage();
            }
            else
                _currentEnemyCount = (_enemyCount += @event.count);
        }

        private void ClaerStage()
        {
            barrier.SetActive(false);
            _clear = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_clear && (1 << collision.gameObject.layer & playerLayer) != 0)
            {
                _target = collision.transform;
                canSkipText.gameObject.SetActive(_active = true);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!_clear && (1 << collision.gameObject.layer & playerLayer) != 0)
                canSkipText.gameObject.SetActive(_active = false);
        }

        private void Update()
        {
            if (!_clear && _active && _target != null)
            {
                canSkipText.transform.position = (Vector2)_target.transform.position + skipTextOffset;
            }
        }
    }
}
