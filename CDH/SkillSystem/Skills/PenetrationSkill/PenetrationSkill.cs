using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using _00.Work.CDH.Code.SkillSystem;
using Assets._00.Work.CDH.Code.SkillSystem.Skills.Thunder;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.PenetrationSkill
{
    public class PenetrationSkill : Skill
    {
        [Header("Penetration Info")]
        [SerializeField] private ObjectFinderSO poolMonoFinder;
        [SerializeField] private PoolItemSO penetrationPrefabSO;
        [SerializeField] private Transform firingTrm;
        [SerializeField] private float power;
        [SerializeField] private float maxLifeTime;

        private PoolManagerMono _poolManager;
        private Vector2 _force;

        private int _curCreatingObj = 0;
        private Player _player;


        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            _player = entity as Player;
            _trigger = entity.GetCompo<EntityAnimationTrigger>();
            SkillData.attackTargetCount = ((int)_player.GetCompo<EntityStat>().GetStat("attack_target_count").Value) * 2;

            bool settingPoolSuccess = poolMonoFinder.GetObject(out _poolManager);
            Debug.Assert(settingPoolSuccess, "can't set poolMono");
        }
        protected override void UseSkill()
        {
            _player.ChangeState("PENETRATION");
            SkillData.attackTargetCount = (int)_player.GetCompo<EntityStat>().GetStat("attack_target_count").Value * 2;
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            _force = transform.right * power;
            _curCreatingObj = 0;
            CreatePenetrationObject();
        }

        private void CreatePenetrationObject()
        {
            // PenetrationObject obj = Instantiate(penetrationPrefab, firingTrm.position, Quaternion.identity);
            PenetrationObject obj = _poolManager.Pop<PenetrationObject>(penetrationPrefabSO);
            obj.SetUp(
                enemyHealth => CalculateDamage(_damage, enemyHealth),
                SkillData,
                _entity,
                _force,
                maxLifeTime,
                _poolManager,
                firingTrm.position,
                SkillData.attackTargetCount
                );
            _curCreatingObj++;

            int attackCount = SkillData.AttackCount;
            if (attackCount > 1 && _curCreatingObj < attackCount)
                DOVirtual.DelayedCall(0.1f, CreatePenetrationObject);
        }
    }
}
