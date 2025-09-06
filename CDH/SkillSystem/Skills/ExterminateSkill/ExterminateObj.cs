using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.SkillSystem.Skills;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using UnityEngine;
using static _00.Work.CDH.Code.Combat.DamageCaster;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.ExterminateSkill
{
    public class ExterminateObj : SkillModule
    {
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private int _maxTime = 5;

        private Vector2 _force;
        private int _curExplosionCnt = 0;
        private int _animationHash;

        public void SetUp(
            GetCalculatedDamageHandler getCalculatedDamageFunc,
            SkillDataSO skillSo,
            Entity owner,
            PoolManagerMono poolManager,
            Vector2 force
            )
        {
            base.SetUp(getCalculatedDamageFunc, skillSo, owner, poolManager);

            _force = force;
            _curExplosionCnt = 0;

            DOVirtual.DelayedCall(_maxTime, EndExterminate);

            damageCaster.InitCaster(owner);

            Throw();
            Explosion();
        }

        private void Explosion()
        {
            _curExplosionCnt++;

            DOVirtual.DelayedCall(1f, () =>
            {
                damageCaster.CastDamage(
                    enemyHealth => GetCalculatedDamage?.Invoke(enemyHealth) ?? 0,
                    _skillSo.enemyKnockbackForce,
                    _skillSo.isPowerAttack
                    );
                if (_curExplosionCnt >= _maxTime) return;
                Explosion();
            });
        }

        private void Throw()
        {
            _rigid.AddForce(_force, ForceMode2D.Impulse);
        }

        private void EndExterminate()
        {
            _poolManager.Push(this);
        }
    }
}
