using _00.Work.CDH.Code.Entities;
using _00.Work.CSH._01.Scripts.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Patrol", story: "[Renderer] Patrol with [mover] in [sec]", category: "Action", id: "127fee74e51fb8aba3036bafd2567178")]
    public partial class PatrolAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;
        [SerializeReference] public BlackboardVariable<EntityMover> Mover;
        [SerializeReference] public BlackboardVariable<float> Sec;

        private float _startTime;

        protected override Status OnStart()
        {
            Mover.Value.SetMovementX(Renderer.Value.FacingDirection);
            _startTime = Time.time;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            bool isOverTime = (Sec.Value + _startTime) < Time.time;
            bool isGround = Mover.Value.IsGroundDetected();
            bool isWall = Mover.Value.IsWallDetected(Renderer.Value.FacingDirection);
            if (isOverTime || isGround == false || isWall == true)
            {
                return Status.Success;
            }
            return Status.Running;
        }

        protected override void OnEnd()
        {

        }
    }
}
