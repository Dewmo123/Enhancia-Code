using System;
using _00.Work.CDH.Code.Entities;
using UnityEngine;

namespace _00.Work.CDH.Code.Combat 
{
    // 나중에 추가 될 수 있음
    public enum AttackType
    {
        CircleCast, BoxCast
    }

    public class DamageCaster : MonoBehaviour
    {
        [Header("Close Range")]
        public float damageRadius;

        [Header("Long Range")]
        public Vector2 damageSize;

        public AttackType attackType;
        [SerializeField] private int maxHitCount = 1;
        [SerializeField] private ContactFilter2D contactFilter;
        private Collider2D[] _hitResults;

        private Entity _owner;
        public delegate float GetCalculatedDamageHandler(float enemyHealth);
        
        public Collider2D[] HitResults => _hitResults;

        public void InitCaster(Entity owner)
        {
            ChangeMaxHitCount(maxHitCount);
            _owner = owner;
        }

        public void ChangeMaxHitCount(int newMaxHitCount)
        {
            _hitResults = new Collider2D[newMaxHitCount];
        }
        
        public bool CastDamage(float damage, Vector2 knockBack, bool isPowerAttack)
        {
            // 그냥 대미지만 계산하게 대미지만 반환하는 것
            return CastDamage(enemyHealth => damage, knockBack, isPowerAttack);
        }

        public bool CastDamage(GetCalculatedDamageHandler calulateDamage, Vector2 knockBack, bool isPowerAttack)
        {
            int cnt;
            switch (attackType)
            {
                // 근거리 공격일 때
                case AttackType.CircleCast:
                    cnt = Physics2D.OverlapCircle(transform.position, damageRadius, contactFilter, _hitResults);
                    break;

                // 원거리 공격일 때
                case AttackType.BoxCast:
                    cnt = Physics2D.OverlapBox(transform.position, damageSize, 0, contactFilter, _hitResults);
                    break;

                // 임시
                default:
                    cnt = Physics2D.OverlapCircle(transform.position, damageRadius, contactFilter, _hitResults);
                    break;
            }

            for (int i = 0; i < cnt; i++)
            {
                Vector2 direction = (_hitResults[i].transform.position - _owner.transform.position).normalized;
                knockBack.x *= Mathf.Sign(direction.x);
                
                if (_hitResults[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.ApplyDamage(calulateDamage?.Invoke(damageable.GetMaxHealth()) ?? 0, direction, knockBack, isPowerAttack, _owner);
                }
            }

            return cnt > 0;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.8f, 0.7f, 0, 0.7f);
            switch (attackType)
            {
                case AttackType.CircleCast:
                    Gizmos.DrawWireSphere(transform.position, damageRadius);
                    break;
                case AttackType.BoxCast:
                    Gizmos.DrawWireCube(transform.position, damageSize);
                    break;
            }
            

        }
#endif
    }
}
