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
    [NodeDescription(name: "Stop", story: "stop with [mover] on [YAxis]", category: "Action", id: "c44ed24b771ed6bc00a84c5922657c00")]
    public partial class StopAction : Action
    {

        [SerializeReference] public BlackboardVariable<EntityMover> Mover;
        [SerializeReference] public BlackboardVariable<bool> YAxis;

        protected override Status OnStart()
        {
            Mover.Value.StopImmediately(YAxis.Value);
            return Status.Success;
        }


    }
}

