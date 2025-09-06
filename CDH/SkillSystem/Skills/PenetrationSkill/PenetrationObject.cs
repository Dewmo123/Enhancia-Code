using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.SkillSystem.Skills;
using _00.Work.CDH.Code.SkillSystem.Skills.Ball;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static _00.Work.CDH.Code.Combat.DamageCaster;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.PenetrationSkill
{
    public class PenetrationObject : SkillModule
    {
        private Vector2 _force;
        private int _maxPenetration;
        private int _currentPenetration = 0;

        public void SetUp(
            GetCalculatedDamageHandler getCalculatedDamageFunc,
            SkillDataSO skillSo,
            Entity owner,
            Vector2 force,
            float maxLifeTime,
            PoolManagerMono poolManager,
            Vector2 pos,
            int maxPenetration
            )
        {
            base.SetUp(getCalculatedDamageFunc, skillSo, owner, poolManager);

            _force = force;
            transform.position = pos;

            transform.Rotate(0, _owner.GetCompo<EntityRenderer>().FacingDirection > 0 ? 180 : 0, 0);

            _maxPenetration = maxPenetration;

            ThrowPenetrationObject();
            DOVirtual.DelayedCall(maxLifeTime, () => { poolManager.Push(this); });
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer & _skillSo.canCollisionTargetLayer) == 0) return;
            Debug.Log("attack");

            if(collision.TryGetComponent(out Entity enemy))
                AttackEnemy(enemy);
        }

        private void AttackEnemy(Entity enemy)
        {
            if (_currentPenetration >= _maxPenetration)
                _poolManager.Push(this);

            _currentPenetration++;
            if(enemy.TryGetComponent(out IDamageable damageable))
            {
                damageable.ApplyDamage(
                    GetCalculatedDamage?.Invoke(damageable.GetMaxHealth()) ?? 0,
                    _force.normalized,
                    _skillSo.enemyKnockbackForce,
                    _skillSo.isPowerAttack,
                    _owner
                    );
            }
        }

        private void ThrowPenetrationObject()
        {
            _rigid.AddForce(_force, ForceMode2D.Impulse);
        }
    }
}
