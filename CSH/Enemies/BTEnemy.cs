using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Events;
using Assets._00.Work.AKH.Scripts.Core.EventSystem;
using Assets._00.Work.YHB.Scripts.Core;
using Core.EventSystem;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;


namespace _00.Work.CSH._01.Scripts.Enemies
{
    public abstract class BTEnemy : Entity, IPoolable
    {
        [Header("Pool Set")]
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        [field: SerializeField] public GameObject GameObject { get; private set; }
        
        private Pool _myPool;

        protected StateChangeEvent _stateChannel;

        [Header("Enemy Set")]
        [field:SerializeField] public ObjectFinderSO Playerfinder { get; protected set; }
        public LayerMask whatIsPlayer;
        public LayerMask whatIsGround;
        [SerializeField] private EventChannelSO inGameEventChannel;

        public System.Action<BTEnemy> OnEnemyDead;

        protected BehaviorGraphAgent _btAgent;

        public event System.Action<IEnumerable<StatSO>, float> OnStatSettingEvent;

        protected override void AddComponentToDictionary()
        {
            base.AddComponentToDictionary();

            _btAgent = GetComponent<BehaviorGraphAgent>();
            Debug.Assert(_btAgent != null, $"{gameObject.name}에 BT가 없잖아");
        }

        public BlackboardVariable<T> GetBlackboardVariable<T>(string key)
        {
            if(_btAgent.GetVariable(key, out BlackboardVariable<T> result))
            {
                return result;
            }
            
            return default;
        }

        public void SetStat(float multiplyValue, params StatSO[] targetStats)
        {
            OnStatSettingEvent?.Invoke(targetStats, multiplyValue);
        }

        protected override void HandleDead()
        {
            isDead = true;
            OnEnemyDead?.Invoke(this);
        }
        public void PushToPool()
        {
            inGameEventChannel?.InvokeEvent(EnemySpawnEvent.EnemyCountChangeEvent.Initialize(-1));
            _myPool.Push(this);
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            _stateChannel?.SendEventMessage(BTEnemyState.PATROL);
            isDead = false;
            OnEntityResetEvent?.Invoke();
            // 체력 초기화, state초기화
        }
    }
}
