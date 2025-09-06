using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using UnityEngine;

namespace _00.Work.CDH.Code.SkillSystem.Skills.Backflow
{
    public class BackflowSkill : Skill
    {
        [Header("Backflow Info")]
        [SerializeField] private DamageCaster damageCaster;
        
        private Player _player;

        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            _player = entity as Player;
            damageCaster.InitCaster(_player);
        }

        protected override void UseSkill()
        {
            _player.ChangeState("BACKFLOW");
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            SkillData.attackTargetCount = (int)_player.GetCompo<EntityStat>().GetStat("attack_target_count").Value;

            damageCaster.ChangeMaxHitCount(SkillData.attackTargetCount);
            damageCaster.CastDamage(enemyHealth => CalculateDamage(_damage, enemyHealth), SkillData.enemyKnockbackForce, SkillData.isPowerAttack);
        }
    }
}