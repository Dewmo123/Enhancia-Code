using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using _00.Work.CDH.Code.SkillSystem;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using System;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.Radiance
{
    public class RadianceSkill : Skill
    {
        [SerializeField] private ObjectFinderSO poolMonoFinder;
        [SerializeField] private PoolItemSO prefabSO;
        [SerializeField] private Transform skillPos;
        [SerializeField] private float maxRadiance;
        [SerializeField] private float duration;

        private PoolManagerMono _poolManager;
        private Collider2D[] colliders;

        private Player _player;

        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            colliders = new Collider2D[SkillData.attackTargetCount];

            _player = entity as Player;

            bool settingPoolSuccess = poolMonoFinder.GetObject(out _poolManager);
            Debug.Assert(settingPoolSuccess, "can't set poolMono");
        }

        protected override void UseSkill()
        {
            _player.ChangeState("RADIANCE");
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            Radiance newRadiance = _poolManager.Pop<Radiance>(prefabSO);
            newRadiance.transform.position = skillPos.position;
            newRadiance.SetUp(
                enemyHealth => CalculateDamage(_damage, enemyHealth),
                SkillData,
                _entity,
                _poolManager,
                maxRadiance,
                duration
                );
        }
    }
}
