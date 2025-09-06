using _00.Work.CSH._01.Scripts.Enemies;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "FindTarget", story: "[Self] set [target] from finder", category: "Action", id: "73ae9bd00d86e71813ab4bd1b8dfdd84")]
    public partial class FindTargetAction : Action
    {
        [SerializeReference] public BlackboardVariable<BTEnemy> Self;
        [SerializeReference] public BlackboardVariable<Transform> Target;

        protected override Status OnStart()
        {
            Target.Value = Self.Value.Playerfinder.Object.transform;
            return Status.Success;
        }


    }
}

