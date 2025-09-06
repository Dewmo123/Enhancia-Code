using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.SkillSystem.Skills;
using Library.Normal;
using System;
using UnityEngine;

namespace _00.Work.CDH.Code.SkillSystem
{
    public abstract class Skill : MonoBehaviour
    {
        public delegate void CooldownInfo(float current, float totalTime);

        [field: SerializeField] public SkillDataSO SkillData { get; private set; }
        [SerializeField] protected StatSO damageStat;

        protected float _remainCooldown;
        protected float _damage;
        protected StatSO _damageStat;
        protected EntityAnimationTrigger _trigger;

        public event CooldownInfo OnCooldown;
        public bool IsCooldown
            => _remainCooldown > 0f;

        public NotifyValue<int> currentSkillChargeCount;
        public event Action<int> OnChangeSkillChargeCount;
        public int CurrentSkillChargeCount
        {
            get => currentSkillChargeCount.Value;
            set => currentSkillChargeCount.Value = Mathf.Clamp(value, 0, SkillData.MaxChargeCount);
        }

        protected Entity _entity;

        public virtual void InitializeSkill(Entity entity)
        {
            _entity = entity;
            currentSkillChargeCount = new NotifyValue<int>();
            currentSkillChargeCount.OnValueChanged += HandleChangeSkillChargeCount;
            _damageStat = _entity.GetCompo<EntityStat>().GetStat(damageStat);
            _damage = _damageStat.Value;
            _trigger = _entity.GetCompo<EntityAnimationTrigger>();

            _damageStat.OnValueChange += HandleDamageStatChanged;
        }

        protected virtual void OnDestroy()
        {
            currentSkillChargeCount.OnValueChanged -= HandleChangeSkillChargeCount;
            _damageStat.OnValueChange -= HandleDamageStatChanged;
        }

        public virtual void UpdateSkill()
        {
            if (CurrentSkillChargeCount < SkillData.MaxChargeCount)
            {
                if (_remainCooldown > 0f)
                {
                    OnCooldown?.Invoke(_remainCooldown, SkillData.cooldown);
                }
                else if (_remainCooldown <= 0f)
                {
                    // 여기서 스킬 차징
                    _remainCooldown = SkillData.cooldown;
                    ++CurrentSkillChargeCount;
                }
            }

            _remainCooldown -= Time.deltaTime;
        }

        public virtual void ActivedSkill()
        {
            CurrentSkillChargeCount = 0;
            _remainCooldown = 0f;
        }

        public virtual void UnActivedSkill() { }

        private void HandleChangeSkillChargeCount(int previousValue, int nextValue)
        {
            OnChangeSkillChargeCount?.Invoke(nextValue);
        }

        private void HandleDamageStatChanged(StatSO stat, float current, float previous)
        {
            _damage = current;
        }

        public bool AttemptUseSkill()
        {
            if (CurrentSkillChargeCount > 0)
            {
                --CurrentSkillChargeCount;
                _remainCooldown = SkillData.cooldown;
                UseSkill();
                return true;
            }
            return false;
        }

        protected virtual void UseSkill()
        {
            _trigger.OnSkillTrigger += StartSkill;
        }

        protected virtual void StartSkill()
        {
            _trigger.OnSkillTrigger -= StartSkill;
        }

        /// <summary>
        /// 데미지를 계산해서 가져옵니다.
        /// </summary>
        /// <param name="enemyMaxHealth">적의 최대 체력입니다. 0일 경우 체력비례 데미지를 계산하지 않습니다.</param>
        /// <returns>계산된 데미지</returns>
        public float CalculateDamage(float damage, float enemyMaxHealth = 0) // 곱연사 대미지, 추가 대미지, 체력 비례 대미지
            => ((damage * SkillData.MultiplyDamage) + SkillData.AdditionalDamage) + (enemyMaxHealth * SkillData.EnemyHpProportionProbability);
    }
}