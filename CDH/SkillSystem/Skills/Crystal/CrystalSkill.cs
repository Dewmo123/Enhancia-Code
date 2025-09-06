using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using _00.Work.CDH.Code.SkillSystem;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using System;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.Crystal
{
    public class CrystalSkill : Skill
    {
        [Header("Crystal Info")]

        [SerializeField] private ObjectFinderSO poolMonoFinder;
        [SerializeField] private PoolItemSO prefabSO;
        [SerializeField, Range(1f, 20f)] private float speed;
        [SerializeField, Range(0.1f, 3f)] private float createSpeed;
        [SerializeField] private Crystal crystal;
        [SerializeField] private Transform crystalPos;

        private Player _player;
        private int _curCreateCount;
        private int _maxCreateCount;
        private PoolManagerMono _poolManager;

        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            _player = entity as Player;
            _trigger = entity.GetCompo<EntityAnimationTrigger>();


            bool settingPoolSuccess = poolMonoFinder.GetObject(out _poolManager);
            Debug.Assert(settingPoolSuccess, "can't set poolMono");
        }


        private void CreateCrystal()
        {
            _curCreateCount++;

            Crystal newCrystal = _poolManager.Pop<Crystal>(prefabSO);
            newCrystal.transform.position = crystalPos.position + new Vector3(0, -2.2f);
            newCrystal.SetUp(
                enemyHealth => CalculateDamage(_damage, enemyHealth),
                _player,
                SkillData,
                _player.GetCompo<EntityRenderer>().FacingDirection,
                speed,
                _poolManager
                );

            if (_curCreateCount >= _maxCreateCount) return;
            DOVirtual.DelayedCall(createSpeed, CreateCrystal);
        }

        protected override void UseSkill()
        {
            _player.ChangeState("CRYSTAL");
            SkillData.attackTargetCount = (int)_player.GetCompo<EntityStat>().GetStat("attack_target_count").Value;
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            _maxCreateCount = SkillData.AttackCount;
            _curCreateCount = 0;
            CreateCrystal();
        }
    }
}
