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
    [NodeDescription(name: "Move", story: "[Self] move with [mover]", category: "Action", id: "93a79a04934f84b1fb2da5d5fcc8e0ec")]
    public partial class MoveAction : Action
    {
        [SerializeReference] public BlackboardVariable<BTEnemy> Self;
        [SerializeReference] public BlackboardVariable<EntityMover> Mover;

        float speed = 0.5f;
        protected override Status OnStart()
        {
            Mover.Value.SetMovementX(speed);
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            EntityRenderer _renderer = Self.Value.GetCompo<EntityRenderer>();
            if (Mover.Value.IsWallDetected(_renderer.FacingDirection))
            {
                Mover.Value.SetMovementX(speed*-1);

                _renderer.Filp();
                
            }
            else if(!Mover.Value.IsGroundDetected())
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
