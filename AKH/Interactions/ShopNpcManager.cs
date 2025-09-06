using Assets._00.Work.AKH.Scripts.Core.EventSystem;
using Core.EventSystem;
using System;
using UnityEngine;

namespace Scripts.Interactions
{
    public class ShopNpcManager : MonoBehaviour
    {
        [SerializeField]private GameObject shop;
        [SerializeField] private EventChannelSO generateChannel;
        [SerializeField] private EventChannelSO inStageChannel;
        private void Awake()
        {
            generateChannel.AddListener<GenerateCompleteEvent>(HandleGenerate);
            inStageChannel.AddListener<SkillChangeEvent>(HandleSkillChange);
        }

        private void HandleSkillChange(SkillChangeEvent @event)
        {
            shop.SetActive(false);
        }

        private void OnDestroy()
        {
            generateChannel.RemoveListener<GenerateCompleteEvent>(HandleGenerate);
            inStageChannel.RemoveListener<SkillChangeEvent>(HandleSkillChange);
        }
        private void HandleGenerate(GenerateCompleteEvent @event)
        {
            shop.SetActive(true);
        }
    }
}
