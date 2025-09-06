using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.Events;


namespace _00.Work.CSH._01.Scripts.Enemies
{
    public class EnemyAttackCompo : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [Header("Atk Setting")]
        public float attackDistance;
        public float detectDistance;
        public bool isPowerAttack;
        [SerializeField] private StatSO attackDamageSO;

        [SerializeField] private float attackCooldown, cooldownRandomness;

        [SerializeField] private Vector2[] attackMoveForce;

        [Header("Reference")]
        [SerializeField] private DamageCaster damageCaster;

        [Header("BlackBoard")]
        [SerializeField] private string attackRangeName;
        [SerializeField] private string detectRangeName, attackCooldownName;
        
        [Header("?")]
        public UnityEvent OnAttack;

        private BTEnemy _enemy;
       
        private EntityAnimationTrigger _triggerCompo;
        private EntityStat _statComp;
        private EntityMover _moverComp;
        private EntityRenderer _rendererComp;

        private StatSO _attackDamageStat;

        private BlackboardVariable<float> _attackCooldownVariable;

        // 사용이 필요한 경우 상속받아서 쓰세요. 셋팅은 Start에서
        protected int _attackCombo = 0;

        private float Damage
            => _attackDamageStat.Value;

        protected virtual void Start()
        {
            Debug.Assert(_enemy != null, $"why bt enemy is null in enemy attack compo?");

            _enemy.GetBlackboardVariable<float>(attackRangeName).Value = attackDistance;
            _enemy.GetBlackboardVariable<float>(detectRangeName).Value = detectDistance;
            _enemy.GetBlackboardVariable<float>(attackCooldownName).Value = attackCooldown;
        }

        public void Initialize(Entity entity)
        {
            _enemy = entity as BTEnemy;
            _triggerCompo = entity.GetCompo<EntityAnimationTrigger>();
            _moverComp = entity.GetCompo<EntityMover>();
            _rendererComp = entity.GetCompo<EntityRenderer>();

            // 수정 - 04.22
            damageCaster.InitCaster(entity);

            _statComp = _enemy.GetCompo<EntityStat>();
            _attackDamageStat = _statComp.GetStat(attackDamageSO);
        }

        public void AfterInit()
        {
            _triggerCompo.OnAttackTrigger += HandleAttackTrigger;
            _enemy.OnStatSettingEvent += ApplyStatSettings;
        }

        private void OnDestroy()
        {
            _triggerCompo.OnAttackTrigger -= HandleAttackTrigger;
            _enemy.OnStatSettingEvent -= ApplyStatSettings;
        }

        private void HandleAttackTrigger()
        {
            _triggerCompo.OnAnimationEnd += HandleAnimationEnd;
            _moverComp.CanManualMove = false;

            Vector2 knockback = Vector2.zero;
            bool success = damageCaster.CastDamage(Damage, knockback, isPowerAttack);

            Vector2 movement = attackMoveForce[_attackCombo];
            movement.x *= _rendererComp.FacingDirection;
            _moverComp.AddForceToEntity(movement);
            OnAttack?.Invoke();
        }

        private void HandleAnimationEnd()
        {
            _triggerCompo.OnAnimationEnd -= HandleAnimationEnd;
            _moverComp.CanManualMove = true;
        }

        private void ApplyStatSettings(IEnumerable<StatSO> stats, float multipltValue)
        {
            foreach (StatSO stat in stats)
            {
                if (_statComp.TryGetStat(stat, out StatSO gotStat))
                {
                    gotStat.ClearAllModifier();
                    gotStat.AddModifier(gotStat, gotStat.Value * (multipltValue - 1));
                }
            }
        }



#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectDistance);
        }
#endif
    }


}
