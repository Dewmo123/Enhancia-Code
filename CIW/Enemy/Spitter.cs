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
        public System.Action OnBurst; // BT action���� ȣ������ ��
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

            // entity���� �����ص� ������ ����ϱ�
            OnBurst += HandleBurst;

            _target = _objFinder.Object.transform;
        }

        private void Start()
        {
            // _stateChannel �ʱ�ȭ ���ֱ�
            BlackboardVariable<StateChangeEvent> stateChannelVariable = GetBlackboardVariable<StateChangeEvent>("StateChannel");
            _stateChannel = stateChannelVariable.Value;
            Debug.Assert(_stateChannel != null, $"StateChannel variable is null {gameObject.name}");

            _state = GetBlackboardVariable<BTEnemyState>("EnemyState");
        }

        private void HandleBurst()
        {
            OnBurstEvent?.Invoke(_target.transform); // ������ �̺�Ʈ���� ȣ��

            // Obj Pooling �ؼ� �Ѿ� �����ϱ�
            //if (_target == null)
            //{
            //    Debug.LogError("Spitter._target�� null�Դϴ�. ObjectFinderSO�� ����� ��ϵǾ� �ֳ���?");
            //}
            //else if (_target.transform == null)
            //{
            //    Debug.LogError("Spitter._target.transform�� null�Դϴ�.");
            //}

            ////Debug.Assert(targetTrm != null, $"{targetTrm} �� ���ݴ�");
            //var burstInstance = Instantiate(_burstPrefabs, _parentTrm.position, Quaternion.identity, _parentTrm);
            //var burst = burstInstance.GetComponent<Burst>();

            //if (burst == null)
            //{
            //    Debug.LogError("burstInstance�� Burst ������Ʈ�� �����ϴ�.");
            //}

            //Debug.Log($"{_target.transform.position} ������");
            
            //burst.LaunchToTarget(_target.transform.position, 5f); // error - nullargument exception
        }

        protected override void HandleHit()
        {
            if (isDead) return;

            // ���� ���¿����� ����
            // ���⼭ Hit ����?
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
            // �ߺ� ��� ������ ���� �ѹ� ��
            OnBurst -= HandleBurst;
            GetCompo<EntityHealth>().OnKnockBack -= HandleKnockback;
        }
    }
}

