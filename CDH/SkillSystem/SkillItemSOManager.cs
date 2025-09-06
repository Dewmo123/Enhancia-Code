using _00.Work.CDH.Code.SkillSystem;
using Assets._00.Work.CDH.Code.SkillSystem.Skills;
using Core.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem
{
    [DefaultExecutionOrder(-5)]
    public class SkillItemSOManager : MonoBehaviour
    {
        [Serializable]
        public class SkillInfoModel
        {
            public Skill skill;
            public SkillItemSO skillItem;
        }

        [SerializeField] private EventChannelSO inStageEventChannel;
        [SerializeField] private PlayerSkillCompo playerSkillComponent;
        [SerializeField] private List<SkillInfoModel> skillItemList;

        private void Start()
        {
            SetSkillItemSOes();
            inStageEventChannel.InvokeEvent(InStageEvent.ProclaimSkillCompEvent.Initialize(
                playerSkillComponent,
                skillItemList.Select(skillInfoModel => skillInfoModel.skillItem)
                ));
            Debug.Log("D");
        }

        private void SetSkillItemSOes()
        {
            if (skillItemList == null)
                return;

            foreach (SkillInfoModel item in skillItemList)
            {
                if (item.skillItem == null || item.skill == null)
                    continue;

                item.skillItem.SetSkillType(item.skill.GetType());
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            SetSkillItemSOes();
        }

        [ContextMenu("Show skill item's skill type")]
        private void ShowSkillItemSkillType()
        {
            foreach (SkillInfoModel item in skillItemList)
            {
                if (item.skillItem == null)
                    continue;

                string[] skillType = item.skillItem.SkillType.ToString().Split('.');
                Debug.Log($"{item.skillItem.name} : {skillType[skillType.Length - 1]}");
            }
        }
#endif
    }
}
