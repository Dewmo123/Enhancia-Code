using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using _00.Work.CDH.Code.SkillSystem;
using _00.Work.CDH.Code.SkillSystem.Skills.Ball;
using _00.Work.CSH._01.Scripts.Enemies;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.Thunder
{
    enum EnemyDirection
    {
        Left,
        Right
    }
    public class ThunderSkill : Skill
    {
        [Header("Thunder Info")]
        [SerializeField] private ObjectFinderSO poolMonoFinder;
        [SerializeField] private PoolItemSO prefabSO;
        [SerializeField] private ContactFilter2D whatIsEnemy;
        [SerializeField] private GameObject thunderPrefab;
        [SerializeField] private float thunderDelay;

        private Collider2D[] _colliders;
        private float _facingDirection;
        private Entity _enemy = null;
        private int _curThunder;
        private int _maxThunder;
        private PoolManagerMono _poolManager;
        private Player _player;

        
        public override void InitializeSkill(Entity entity)
        {
            base.InitializeSkill(entity);
            _colliders = new Collider2D[50];
            _player = entity as Player;
            _maxThunder = SkillData.AttackCount;
            bool settingPoolSuccess = poolMonoFinder.GetObject(out _poolManager);
            Debug.Assert(settingPoolSuccess, "can't set poolMono");
        }
        protected override void UseSkill()
        {
            _player.ChangeState("THUNDER");
            base.UseSkill();
        }

        protected override void StartSkill()
        {
            base.StartSkill();
            _enemy = null;
            _curThunder = 0;
            _facingDirection = _player.GetCompo<EntityRenderer>().FacingDirection;
            float range = float.MaxValue;
            _enemy = FindClosestEnemy(transform.position, range, _facingDirection < 0 ? EnemyDirection.Left : EnemyDirection.Right);

            CreateThunder();
        }

        private void CreateThunder()
        {
            Debug.Log("CreateThunder");
            if (_enemy == null || _curThunder >= _maxThunder) return;

            // 번개 생성
            Thunder thunder = _poolManager.Pop<Thunder>(prefabSO);
            thunder.transform.position = _enemy.transform.position;
            thunder.SetUp(
                enemyHealth => CalculateDamage(_damage, enemyHealth),
                SkillData,
                _player,
                _poolManager
                );
            // 공격
            thunder.Attack();

            _curThunder++;

            DOVirtual.DelayedCall(thunderDelay, CreateThunder);
        }


        private Entity FindClosestEnemy(Vector3 checkPosition, float range, EnemyDirection direction)
        {
            Entity closestOne = null;

            int cnt = Physics2D.OverlapCircle(checkPosition, range, whatIsEnemy, _colliders);

            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < cnt; i++)
            {
                if (_colliders[i].TryGetComponent(out Entity enemy))
                {
                    if (enemy.isDead) continue;

                    float distanceToEnemy = Vector2.Distance(checkPosition, _colliders[i].transform.position);

                    if (distanceToEnemy < closestDistance && enemy != _entity && IsRightDirection(enemy.transform, direction))
                    {
                        closestDistance = distanceToEnemy;
                        closestOne = enemy;
                    }
                }
            }
            return closestOne;
        }

        private bool IsRightDirection(Transform enemyTrm, EnemyDirection direction)
        {
            float diff = enemyTrm.position.x - transform.position.x;

            return (diff < 0 ? EnemyDirection.Left : EnemyDirection.Right) == direction;
        }
    }
}
