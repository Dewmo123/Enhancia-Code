using System;
using System.Security.Cryptography;
using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Core.StatSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _00.Work.CDH.Code.Entities
{
    public class EntityHealth : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [SerializeField] private StatSO hpStat;
        public float maxHealth;
        private float _currentHealth;

        public event Action<Vector2> OnKnockBack;
        // 맞은거랑 데미지를 입은거랑 다름
        public UnityEvent<float> OnDamaged;
        public float HpPercent => _currentHealth / maxHealth;
        private Entity _entity;
        private EntityStat _statCompo;
        private EntityFeedbackData _feedbackData;

        #region Init

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _statCompo = entity.GetCompo<EntityStat>();
            _feedbackData = entity.GetCompo<EntityFeedbackData>();

            _entity.GetMaxHealthValue += GetMaxHealth;
            _entity.OnEntityResetEvent += ResetHealth;
        }
        
        public void AfterInit()
        {
            _statCompo.GetStat(hpStat).OnValueChange += HandleHpChange;
            _entity.OnDamage += ApplyDamage;

            _currentHealth = maxHealth = _statCompo.GetStat(hpStat).Value;
        }

        private void OnDestroy()
        {
            _statCompo.GetStat(hpStat).OnValueChange -= HandleHpChange;
            _entity.OnDamage -= ApplyDamage;

            _entity.GetMaxHealthValue -= GetMaxHealth;
            _entity.OnEntityResetEvent -= ResetHealth;
        }

        #endregion

        private void HandleHpChange(StatSO stat, float current, float previous)
        {
            maxHealth = current;
            _currentHealth = Mathf.Clamp(_currentHealth + current - previous, 1f, maxHealth);
            // 체력 변경으로 사망하는 일이 없도록 
        }
        public void ApplyDamage(float damage, Vector2 direction, Vector2 knockBackPower, bool isPowerAttack, Entity dealer)
        {
            if (_entity.isDead) return;
            
            _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, maxHealth);
            _feedbackData.LastAttackDirection = direction.normalized;
            _feedbackData.IsLastHitPowerAttack = isPowerAttack;
            _feedbackData.IsLastEntityWhoHit = dealer;
            
            AfterHitFeedbacks(knockBackPower);
            OnDamaged?.Invoke(damage);
        }
        private void AfterHitFeedbacks(Vector2 knockBackPower)
        {
            _entity.OnHit?.Invoke();
            OnKnockBack?.Invoke(knockBackPower);

            if (_currentHealth <= 0f)
            {
                _entity.OnDead?.Invoke();
            }
        }

        private float GetMaxHealth()
            => maxHealth;

        private void ResetHealth()
        {
            _currentHealth = maxHealth;
        }
    }
}