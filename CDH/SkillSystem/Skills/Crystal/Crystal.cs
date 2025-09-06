using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.SkillSystem.Skills;
using Assets._00.Work.CDH.Code.SkillSystem.Effects;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static _00.Work.CDH.Code.Combat.DamageCaster;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.Crystal
{
    public class Crystal : SkillModule
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SkillEffectAnimationTrigger trigger;
        [SerializeField] private float maxLifeTime;

        private float _facingDirection;
        private SkillDataSO _crystalSkillSO;
        private float _speed;
        private int _animationHash;

        public override void ResetItem()
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        public void SetUp(
            GetCalculatedDamageHandler getCalculatedDamageFunc,
            Entity owner,
            SkillDataSO skillSO,    
            float facingDirection,
            float speed,
            PoolManagerMono poolManager
            )
        {
            base.SetUp(getCalculatedDamageFunc, skillSO, owner, poolManager);

            _animationHash = Animator.StringToHash("Trigger");

            trigger.OnAnimationEnd += () => _poolManager.Push(this);

            GetCalculatedDamage = getCalculatedDamageFunc;
            _owner = owner;
            _facingDirection = facingDirection;
            _crystalSkillSO = skillSO;
            _speed = speed;
            _poolManager = poolManager;

            transform.Rotate(0, facingDirection > 0 ? 0 : 180, 0);

            DOVirtual.DelayedCall(maxLifeTime, () => _poolManager.Push(this));

            Move();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer & _skillSo.canCollisionTargetLayer) != 0)
            {
                _rigid.linearVelocityX = 0;
                animator.SetTrigger(_animationHash);
                if (collision.TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(GetCalculatedDamage?.Invoke(damageable.GetMaxHealth()) ?? 0,
                        new Vector2(_facingDirection,0),
                        _crystalSkillSO.enemyKnockbackForce,
                        _crystalSkillSO.isPowerAttack,
                        _owner
                        );
                }
            }
        }


        private void Move()
        {
            if (_rigid == null) return;

            _rigid.linearVelocityX = _facingDirection * _speed;
        }
    }
}
