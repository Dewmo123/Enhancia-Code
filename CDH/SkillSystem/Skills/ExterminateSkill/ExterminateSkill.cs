using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using _00.Work.CDH.Code.SkillSystem;
using Assets._00.Work.CDH.Code.SkillSystem.Skills.ExplosionSkill;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.ObjectPool.RunTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.ExterminateSkill
{
    public class ExterminateSkill : Skill
    {
        [Header("Exterminate Info")]

        [SerializeField] private ObjectFinderSO poolMonoFinder;
        [SerializeField] private PoolItemSO prefabSO;

        [SerializeField] private ExterminateObj obj;
        [SerializeField] private Transform firingTrm;
        [SerializeField] private float speed;

        private Vector2 _force;
        private PoolManagerMono _poolManager;

        private Player _player;


        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            _player = entity as Player;
            _trigger = entity.GetCompo<EntityAnimationTrigger>();

            bool settingPoolSuccess = poolMonoFinder.GetObject(out _poolManager);
            Debug.Assert(settingPoolSuccess, "can't set poolMono");
        }

        protected override void UseSkill()
        {
            _player.ChangeState("EXTERMINATE");
            SkillData.attackTargetCount = (int)_player.GetCompo<EntityStat>().GetStat("attack_target_count").Value * 2 + 3;
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            _force = transform.right * speed;
            Create();
        }

        private void Create()
        {
            ExterminateObj explosion = _poolManager.Pop<ExterminateObj>(prefabSO);
            explosion.transform.position = firingTrm.position;
            explosion.SetUp(
                enemyHealth => CalculateDamage(_damage, enemyHealth),
                SkillData,
                _entity,
                _poolManager,
                _force
                );
        }
    }
}
