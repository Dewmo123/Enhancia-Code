using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.SkillSystem.Skills;
using Assets._00.Work.CDH.Code.SkillSystem.Effects;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using static _00.Work.CDH.Code.Combat.DamageCaster;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.ExplosionSkill
{
    public class ExplosionObj : SkillModule
    {
        [SerializeField] private SkillEffectAnimationTrigger trigger;
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private Rigidbody2D rigid;
        [SerializeField] private Animator animator;
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private float explosionTimer;

        private Vector2 _force;
        private int _explosionAnimationTriggerHash;

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
            _rigid = GetComponent<Rigidbody2D>();

            Color color = renderer.color;
            color.a = 1;
            renderer.color = color;

            transform.localScale = Vector3.one;
            _explosionAnimationTriggerHash = Animator.StringToHash("Trigger");

            damageCaster.InitCaster(owner);

            Throw();
            // DOVirtual.DelayedCall(explosionTimer, Explosion);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((1 << collision.gameObject.layer & _skillSo.canCollisionTargetLayer) != 0)
                Explosion();
        }

        private void Explosion()
        {
            _rigid.linearVelocity = Vector3.zero;

            Color color = renderer.color;
            color.a = 0;
            renderer.color = color;

            animator.SetTrigger(_explosionAnimationTriggerHash);

            trigger.OnAnimationEnd += () => _poolManager.Push(this);

            damageCaster.CastDamage(
                enemyHealth => GetCalculatedDamage?.Invoke(enemyHealth) ?? 0,
                _skillSo.enemyKnockbackForce,
                _skillSo.isPowerAttack
                );
        }

        private void Throw()
        {
            _rigid.AddForce(_force, ForceMode2D.Impulse);
        }
    }
}
