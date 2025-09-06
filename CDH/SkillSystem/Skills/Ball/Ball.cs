using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.SkillSystem.Skills.Pull;
using Assets._00.Work.CDH.Code.SkillSystem;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using System;
using System.Linq;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.AI;
using static _00.Work.CDH.Code.Combat.DamageCaster;

namespace _00.Work.CDH.Code.SkillSystem.Skills.Ball
{
    public class Ball : SkillModule
    {
        [SerializeField] private ContactFilter2D _whatIsEnemy;
        [SerializeField] private float _radius;

        private Entity[] _enemies;
        private Collider2D[] _colliders;
        private int _curBounce;

        public void SetUp(
            GetCalculatedDamageHandler getCalculatedDamageFunc,
            SkillDataSO skillSo,
            Entity owner,
            PoolManagerMono poolManager,
            Vector3 startPos
            )
        {
            base.SetUp(getCalculatedDamageFunc, skillSo, owner, poolManager);

            _owner = owner;
            GetCalculatedDamage = getCalculatedDamageFunc;
            transform.position = startPos;
            _colliders = new Collider2D[50];
            _enemies = new Entity[_skillSo.AttackCount];
            _curBounce = 0;

            BounceBall();
        }

        private void BounceBall()
        {
            if (_curBounce >= _skillSo.AttackCount) return;

            _enemies[_curBounce] = FindClosestEnemy(transform.position, _radius);
            if (_enemies[_curBounce] == null)
            {
                _poolManager.Push(this);
                return;
            }

            MoveToTarget(_enemies[_curBounce].transform);

            AttackEnemy(_enemies[_curBounce]);

            _curBounce++;
            DOVirtual.DelayedCall(0.4f, BounceBall);
        }

        private void AttackEnemy(Entity enemy)
        {
            if (enemy == null) return;

            if (enemy.TryGetComponent(out IDamageable damageable))
            {
                Vector2 direction = transform.position.x < enemy.transform.position.x ? -transform.right : transform.right;

                damageable.ApplyDamage(
                    GetCalculatedDamage?.Invoke(damageable.GetMaxHealth()) ?? 0,
                    direction,
                    _skillSo.enemyKnockbackForce,
                    _skillSo.isPowerAttack,
                    _owner
                    );
            }
        }

        private void MoveToTarget(Transform target)
        {
            transform.DOMove(target.position, 0.3f);
            // transform.DOBlendableMoveBy(target.position, 0.3f);
        }

        private Entity FindClosestEnemy(Vector3 checkPosition, float range)
        {
            Entity closestOne = null;

            int cnt = Physics2D.OverlapCircle(checkPosition, range, _whatIsEnemy, _colliders);

            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < cnt; i++)
            {
                if (_colliders[i].TryGetComponent(out Entity enemy))
                {
                    if (enemy.isDead) continue;

                    float distanceToEnemy = Vector2.Distance(checkPosition, _colliders[i].transform.position);

                    if (distanceToEnemy < closestDistance && !_enemies.Contains(enemy) && enemy != _owner)
                    {
                        closestDistance = distanceToEnemy;
                        closestOne = enemy;
                        _enemies[_curBounce] = enemy;
                    }
                }
            }
            return closestOne;
        }
    }
}