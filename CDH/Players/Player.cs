
using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Entities.FSM;
using _00.Work.CDH.Code.SkillSystem;
using UnityEngine;
using UnityEngine.Events;
using StateMachine = _00.Work.CDH.Code.Entities.FSM.StateMachine;

namespace _00.Work.CDH.Code.Players
{
    public class Player : Entity
    {
        [field: SerializeField] public PlayerInputSO InputSO { get; private set; }
        [SerializeField] private StateListSO playerFSM;

        private StateMachine _stateMachine;
        public UnityEvent OnJump;

        private int _maxJumpCount;
        private int _currentJumpCount;

        public int JumpCount
        {
            get => _currentJumpCount;
            set => _currentJumpCount = Mathf.Clamp(value, 0, _maxJumpCount);
        }
        public bool CanJump
            => _currentJumpCount > 0;

        //private int _prevSkillId;
        //private bool _isSkillActive = false;

        protected override void Awake()
        {
            base.Awake();

            _stateMachine = new StateMachine(this, playerFSM);
        }

        protected override void OnDestroy()
        {
            _stateMachine.CurrentState?.Exit();

            base.OnDestroy();
        }

        private void Start()
        {
            _stateMachine.ChangeState("IDLE");
        }
        private void Update()
        {
            _stateMachine.UpdateStateMachine();
        }

        protected override void HandleHit()
        {

        }

        protected override void HandleDead()
        {

        }

        public void SetMaxJumpCount(int value)
            => _maxJumpCount = value;

        public void ResetJumpCount()
        {
            _currentJumpCount = _maxJumpCount;
        }

        public void ChangeState(string newStateName)
            => _stateMachine.ChangeState(newStateName);

        //Exit Skill�� ���ʿ���?
        //public void ExitSkill()
        //{
        //    _isSkillActive = false;
        //    _skillCompo.ExitActiveSkill(_prevSkillId);
        //}
    }
}

