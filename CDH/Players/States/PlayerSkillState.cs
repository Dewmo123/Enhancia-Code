using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Entities.FSM;
using _00.Work.CDH.Code.SkillSystem;
using UnityEngine;

namespace _00.Work.CDH.Code.Players.States
{
    public class PlayerSkillState : EntityState
    {
        protected Player _player;
        protected EntityMover _mover;


        public PlayerSkillState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
        {
            _player = entity as Player;
            _mover = entity.GetCompo<EntityMover>();
        }

        public override void Enter()
        {
            base.Enter();
            _mover.CanManualMove = _canMove;
            _mover.StopImmediately(_isMove);
        }

        public override void Update()
        {
            base.Update();
            if (_isTriggerCall)
                _player.ChangeState("IDLE");
        }

        public override void Exit()
        {
            _mover.CanManualMove = true;
            base.Exit();
        }
    }
}