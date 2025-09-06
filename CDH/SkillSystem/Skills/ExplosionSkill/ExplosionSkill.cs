using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using _00.Work.CDH.Code.SkillSystem;
using Assets._00.Work.CDH.Code.SkillSystem.Skills.PenetrationSkill;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.ExplosionSkill
{
    public class ExplosionSkill : Skill
    {
        [Header("Explosion Info")]

        [SerializeField] private ObjectFinderSO poolMonoFinder;
        [SerializeField] private PoolItemSO prefabSO;
        [SerializeField] private Transform firingTrm;
        [SerializeField] private float speed;

        private PoolManagerMono _poolManager;
        private Vector2 _force;

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
            _player.ChangeState("EXPLOSION");
            SkillData.attackTargetCount = 100;
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            _force = transform.right * speed + transform.up * 3;
            Create();
        }

        private void Create()
        {
            ExplosionObj obj = _poolManager.Pop<ExplosionObj>(prefabSO);
            obj.transform.position = firingTrm.position;
            obj.SetUp(
                enemyHealth => CalculateDamage(_damage, enemyHealth),
                SkillData,
                _entity,
                _poolManager,
                _force
                );
        }
    }
}
