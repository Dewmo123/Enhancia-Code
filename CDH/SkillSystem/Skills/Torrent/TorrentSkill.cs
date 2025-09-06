using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using UnityEngine;

namespace _00.Work.CDH.Code.SkillSystem.Skills.Torrent
{
    public class TorrentSkill : Skill
    {
        [Header("Torrent Info")]
        [SerializeField] private DamageCaster damageCaster;
        
        private Player _player;
        private EntityMover _mover;

        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
            damageCaster.InitCaster(_player);
        }

        protected override void UseSkill()
        {
            _player.ChangeState("TORRENT");
            SkillData.attackTargetCount = 100;
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            damageCaster.ChangeMaxHitCount(SkillData.attackTargetCount);
            damageCaster.CastDamage(enemyHealth => CalculateDamage(_damage, enemyHealth), SkillData.enemyKnockbackForce, SkillData.isPowerAttack);
        }
    }
}