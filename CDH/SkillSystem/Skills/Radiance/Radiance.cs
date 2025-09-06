using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.SkillSystem.Skills;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.Radiance
{
    public class Radiance : SkillModule
    {
        public void SetUp(
            DamageCaster.GetCalculatedDamageHandler getCalculatedDamageFunc,
            SkillDataSO skillSo,
            Entity owner,
            PoolManagerMono poolManager,
            float maxRadiance,
            float duration
            )
        {
            base.SetUp(getCalculatedDamageFunc, skillSo, owner, poolManager);

            StartRadiance(maxRadiance, duration);
        }

        public override void ResetItem()
        {
            base.ResetItem();
            transform.localScale = Vector3.one;
        }

        private void StartRadiance(float maxRadiance, float duration)
        {
            transform.DOScale(maxRadiance, duration);
            float destroyTime = duration + 0.05f;

            DOVirtual.DelayedCall(destroyTime, () => _poolManager.Push(this));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _skillSo.canCollisionTargetLayer) != 0)
            {
                if (!collision.TryGetComponent(out Entity entity)) return;
                Vector2 direction = entity.transform.position - _owner.transform.position;
                entity.ApplyDamage(GetCalculatedDamage.Invoke(entity.GetMaxHealth()), direction, _skillSo.enemyKnockbackForce, _skillSo.isPowerAttack, _owner);
            }
        }
    }
}
