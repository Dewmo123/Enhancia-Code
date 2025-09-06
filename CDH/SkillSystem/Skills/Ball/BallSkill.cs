using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace _00.Work.CDH.Code.SkillSystem.Skills.Ball
{
    public class BallSkill : Skill
    {
        [Header("Ball Info")]

        [SerializeField] private ObjectFinderSO poolMonoFinder;
        [SerializeField] private PoolItemSO ballPrefabSO;
        [SerializeField, Range(1f, 6f)] private float radius;
        [SerializeField] private ContactFilter2D enemyFilter;
        [SerializeField] private LayerMask whatIsObstruct;
        [SerializeField] private Transform ballPos;

        private Player _player;
        private PoolManagerMono _poolManager;

        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            _player = entity as Player;

            bool settingPoolSuccess = poolMonoFinder.GetObject(out _poolManager);
            Debug.Assert(settingPoolSuccess, "can't set poolMono");
        }

        protected override void UseSkill()
        {
            _player.ChangeState("BALL");
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            Ball newBall = _poolManager.Pop<Ball>(ballPrefabSO);
            newBall.SetUp(
                enemyHealth => CalculateDamage(_damage, enemyHealth),
                SkillData,
                _entity,
                _poolManager,
                ballPos.position
                );
        }
    }
}