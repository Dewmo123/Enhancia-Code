using _00.Work.CDH.Code.Entities;
using _00.Work.CSH._01.Scripts.Enemies;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions;
using _00.Work.CSH._01.Scripts.Enemies.BTCommons.Events;
using Assets._00.Work.YHB.Scripts.Core;
using Unity.Behavior;
using UnityEngine;

namespace _00.Work.CIW._01.Scripts.Enemy
{
    public class Spitter : BTEnemy
    {
        //[SerializeField] StateChangeEvent _stateEvt;

        //[SerializeField] GameObject _burstPrefabs;
        [SerializeField] Transform _parentTrm;
        [SerializeField] Transform _target;
        [SerializeField] ObjectFinderSO _objFinder;

        BlackboardVariable<BTEnemyState> _state;
        public System.Action OnBurst; // BT action에서 호출해줄 것
        //public UnityEvent<Vector3> OnBurstEvent;
        public System.Action<Transform> OnBurstEvent;

        EntityMover _mover;

        protected override void AfterInitialize()
        {
            base.AfterInitialize();
            _mover = GetCompo<EntityMover>();
            GetCompo<EntityHealth>().OnKnockBack += HandleKnockback;
        }

        private void HandleKnockback(Vector2 vector)
        {
            float knockBackTime = 0.5f;
            _mover.KnockBack(vector, knockBackTime);
        }

        protected override void Awake()
        {
            base.Awake();

            // entity에서 설정해둔 리스너 사용하기
            OnBurst += HandleBurst;

            _target = _objFinder.Object.transform;
        }

        private void Start()
        {
            // _stateChannel 초기화 해주기
            BlackboardVariable<StateChangeEvent> stateChannelVariable = GetBlackboardVariable<StateChangeEvent>("StateChannel");
            _stateChannel = stateChannelVariable.Value;
            Debug.Assert(_stateChannel != null, $"StateChannel variable is null {gameObject.name}");

            _state = GetBlackboardVariable<BTEnemyState>("EnemyState");
        }

        private void HandleBurst()
        {
            OnBurstEvent?.Invoke(_target.transform); // 오로지 이벤트만을 호출

            // Obj Pooling 해서 총알 관리하기
            //if (_target == null)
            //{
            //    Debug.LogError("Spitter._target이 null입니다. ObjectFinderSO에 대상이 등록되어 있나요?");
            //}
            //else if (_target.transform == null)
            //{
            //    Debug.LogError("Spitter._target.transform이 null입니다.");
            //}

            ////Debug.Assert(targetTrm != null, $"{targetTrm} 이 없잖니");
            //var burstInstance = Instantiate(_burstPrefabs, _parentTrm.position, Quaternion.identity, _parentTrm);
            //var burst = burstInstance.GetComponent<Burst>();

            //if (burst == null)
            //{
            //    Debug.LogError("burstInstance에 Burst 컴포넌트가 없습니다.");
            //}

            //Debug.Log($"{_target.transform.position} 보내줌");
            
            //burst.LaunchToTarget(_target.transform.position, 5f); // error - nullargument exception
        }

        protected override void HandleHit()
        {
            if (isDead) return;

            // 스턴 상태에서는 무시
            // 여기서 Hit 구현?
            Debug.Log("Spitter : Damaged");
            _state.Value = BTEnemyState.HIT;
            _stateChannel.SendEventMessage(BTEnemyState.HIT);
        }

        protected override void HandleDead()
        {
            base.HandleDead();
            _state.Value = BTEnemyState.DEATH;
            _stateChannel.SendEventMessage(BTEnemyState.DEATH);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            // 중복 등록 방지를 위해 한번 더
            OnBurst -= HandleBurst;
            GetCompo<EntityHealth>().OnKnockBack -= HandleKnockback;
        }
    }
}

