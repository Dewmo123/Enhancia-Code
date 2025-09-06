using _00.Work.CDH.Code.SkillSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills
{
	[CreateAssetMenu(fileName = "Skill", menuName = "SO/SkillSystem/SkillItem", order = 0)]
    public class SkillItemSO : ScriptableObject
    {
        [field : SerializeField] public Type SkillType { get; private set; }

        [Header("Skill UI Info")]
        public string skillName;
        public Sprite icon;

        public void SetSkillType(Type skillType)
        {
            SkillType = skillType;
        }
    }
}
