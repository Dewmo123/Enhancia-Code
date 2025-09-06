using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using UnityEngine;

namespace _00.Work.CDH.Code.SkillSystem.Skills.Push
{
    public class PushSkill : Skill
    {
        [Header("Push Info")]
        [SerializeField] private StatSO attackTargetCountStat;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private float pushPower;
        
        private Player _player;
        private int _skillCharge;
        
        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            _skillCharge = SkillData.MaxChargeCount;
            _player = entity as Player;
            damageCaster.InitCaster(_player);
            SkillData.attackTargetCount = (int)attackTargetCountStat.Value * 2;
            attackTargetCountStat.OnValueChange += OnChangeAttackTargetCount;
        }

        private void OnChangeAttackTargetCount(StatSO stat, float current, float previous)
        {
            SkillData.attackTargetCount = (int)current * 2;
        }

        // public override bool AttemptUseSkill()
        // {
        //     if (_remainCooldown <= 0f || _skillCharge > 0)
        //     {
        //         _remainCooldown = SkillData.cooldown;
        //         UseSkill();
        //         return true;
        //     }
        //     return false;
        // }

        protected override void UseSkill()
        {
            _player.ChangeState("PUSH");
            base.UseSkill();

            
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            float fDirection = _player.GetCompo<EntityRenderer>().FacingDirection;

            damageCaster.ChangeMaxHitCount(SkillData.attackTargetCount);
            damageCaster.CastDamage(enemyHealth => CalculateDamage(_damage, enemyHealth), SkillData.enemyKnockbackForce, SkillData.isPowerAttack);

            Collider2D[] hits = damageCaster.HitResults;
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i]!=null&&hits[i].TryGetComponent(out EntityMover mover))
                {
                    Vector2 direction = new Vector2(pushPower * fDirection, 0);
                    mover.KnockBack(direction, 0.15f);
                }
            }

            _skillCharge--;
        }
    }
}
