using Assets._00.Work.AKH.Scripts.Core.EventSystem;
using Core.EventSystem;
using System;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Stages
{
    public class StageStartBarrier : MonoBehaviour
    {
        [SerializeField] private EventChannelSO generateStageNodeEventChannel;

        private void Awake()
        {
            generateStageNodeEventChannel.AddListener<GenerateStartEvent>(HandleStartGenerateEvent);
            generateStageNodeEventChannel.AddListener<GenerateCompleteEvent>(HandleGenerateCompleteEvent);
        }

        private void OnDestroy()
        {
            generateStageNodeEventChannel.RemoveListener<GenerateStartEvent>(HandleStartGenerateEvent);
            generateStageNodeEventChannel.RemoveListener<GenerateCompleteEvent>(HandleGenerateCompleteEvent);
        }

        private void HandleStartGenerateEvent(GenerateStartEvent evt)
        {
            gameObject.SetActive(true);
        }

        private void HandleGenerateCompleteEvent(GenerateCompleteEvent evt)
        {
            gameObject.SetActive(false);
        }
    }
}
