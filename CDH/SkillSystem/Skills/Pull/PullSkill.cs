using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using _00.Work.CSH._01.Scripts.Enemies;
using UnityEngine;

namespace _00.Work.CDH.Code.SkillSystem.Skills.Pull
{
    public class PullSkill : Skill
    {
        [Header("Pull Info")]
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
            _player.ChangeState("PULL");
            SkillData.attackTargetCount = (int)_player.GetCompo<EntityStat>().GetStat("attack_target_count").Value * 2;
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            float fDirection = _player.GetCompo<EntityRenderer>().FacingDirection;

            damageCaster.ChangeMaxHitCount(SkillData.attackTargetCount);
            damageCaster.CastDamage(enemyHealth => CalculateDamage(0, enemyHealth), SkillData.enemyKnockbackForce, SkillData.isPowerAttack);

            // Collider2D[] hits = damageCaster.HitResults;
            // for (int i = 0; i < hits.Length; i++)
            // {
            //     if (hits[i]!=null&&hits[i].TryGetComponent(out EntityMover mover))
            //     {
            //         Debug.Log("knockback");
            //         Vector2 distance = hits[i].transform.position - _player.transform.position;
            //         Vector2 direction = distance * -fDirection;
            //         Debug.Log(direction);
            //         mover.KnockBack(direction, 0.2f);
            //     }
            // }
        }
    }
}
