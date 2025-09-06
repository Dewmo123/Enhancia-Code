using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using Core.EventSystem;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Core.Sound;
using System.Collections.Generic;
using UnityEngine;

namespace _00.Work.CDH.Code.Players
{
    public class PlayerAttackCompo : MonoBehaviour, IEntityComponent, IAfterInit
    {
        [Header("EventChannel")]
        [SerializeField] private EventChannelSO cameraEventChannel;

        [Header("AttackData")]
        [SerializeField] private DamageCaster damageCaster;
        [SerializeField] private List<AttackDataSO> attackDataList;

        [Header("Animation")]
        [SerializeField] private AnimParamSO attackSpeedParam;
        [field: SerializeField] public AnimParamSO ComboCounterParam { get; private set; }

        [Header("Stat")]
        [SerializeField] private StatSO jumpCountStat;
        [SerializeField] private StatSO attackSpeedStat, damageStat;

        [SerializeField] private SoundSO attackSO;
        [SerializeField] private PoolItemSO soundPlayer;

        [Inject] private PoolManagerMono _poolManager;

        private Player _player;
        private EntityStat _statCompo;
        private EntityRenderer _renderer;
        private EntityMover _mover;
        private EntityAnimationTrigger _triggerCompo;

        private StatSO _jumpCountStatComp;
        private StatSO _attackDamageStat;

        private bool _canJumpAttack;

        private Dictionary<string, AttackDataSO> _attackDataDictionary;
        private AttackDataSO _currentAttackData;

        #region Init Section

        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
            _statCompo = entity.GetCompo<EntityStat>();
            _renderer = entity.GetCompo<EntityRenderer>();
            _triggerCompo = entity.GetCompo<EntityAnimationTrigger>();
            damageCaster.InitCaster(entity);

            // 리스트를 딕셔너리로
            _attackDataDictionary = new Dictionary<string, AttackDataSO>();
            attackDataList.ForEach(attackData => _attackDataDictionary.Add(attackData.attackName, attackData));
        }

        public void AfterInit()
        {
            _statCompo.GetStat(attackSpeedStat).OnValueChange += HandleAttackSpeedChange;
            _renderer.SetParam(attackSpeedParam, _statCompo.GetStat(attackSpeedStat).Value);

            _triggerCompo.OnAttackTrigger += HandleAttackTrigger;

            _jumpCountStatComp = _statCompo.GetStat(jumpCountStat);
            _player.SetMaxJumpCount(Mathf.RoundToInt(_jumpCountStatComp.Value));
            _player.ResetJumpCount();

            _attackDamageStat = _statCompo.GetStat(damageStat);

            _jumpCountStatComp.OnValueChange += HandleJumpCountChange;
        }

        private void OnDestroy()
        {
            _statCompo.GetStat(attackSpeedStat).OnValueChange -= HandleAttackSpeedChange;

            _triggerCompo.OnAttackTrigger -= HandleAttackTrigger;

            _jumpCountStatComp.OnValueChange -= HandleJumpCountChange;
        }

        #endregion

        private void HandleAttackSpeedChange(StatSO stat, float current, float previous)
        {
            _renderer.SetParam(attackSpeedParam, current);
        }

        private void HandleAttackTrigger()
        {
            float damage = (_attackDamageStat.Value * _currentAttackData.damageMultiplier) + _currentAttackData.damageIncrease;
            Vector2 knockback = _currentAttackData.knockBackForce;
            bool isHit = damageCaster.CastDamage(damage, knockback, _currentAttackData.isPowerAttack); // isPowerAttack
            var sp = _poolManager.Pop<SoundPlayer>(soundPlayer);
            sp.PlaySound(attackSO, _player.transform.position);

            cameraEventChannel.InvokeEvent(
                CameraEvent.CameraShakeEvent.Initialize(
                    _currentAttackData.cameraShakePower,
                    _currentAttackData.cameraShakeDuration,
                    !isHit));
        }

        public bool CanJumpAttack()
        {
            bool returnValue = _canJumpAttack;
            if (_canJumpAttack)
                _canJumpAttack = false;
            return returnValue;
        }

        private void HandleJumpCountChange(StatSO stat, float current, float previous)
            => _player.SetMaxJumpCount(Mathf.RoundToInt(current));

        private void FixedUpdate()
        {
            if (_canJumpAttack == false && _mover.IsGroundDetected())
                _canJumpAttack = true;
        }

        public AttackDataSO GetAttackData(string attackName)
        {
            AttackDataSO data = _attackDataDictionary.GetValueOrDefault(attackName);
            Debug.Assert(data != null, $"request attack data is not exist : {attackName}");
            return data;
        }

        public void SetAttackData(AttackDataSO attackData)
        {
            _currentAttackData = attackData;
        }
    }
}