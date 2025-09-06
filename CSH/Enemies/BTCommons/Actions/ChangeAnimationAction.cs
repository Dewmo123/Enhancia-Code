using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;


namespace _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions
{[Serializable, GeneratePropertyBag]
    [NodeDescription(name: "ChangeAnimation", story: "[animator] change [current] to [next]", category: "Action", id: "348a82dc4e7abb3431455ef475b86c10")]
    public partial class ChangeAnimationAction : Action
    {
        [SerializeReference] public BlackboardVariable<Animator> Animator;
        [SerializeReference] public BlackboardVariable<string> Current;
        [SerializeReference] public BlackboardVariable<string> Next;

        protected override Status OnStart()
        {
            Animator.Value.SetBool(Current.Value, false);
            Current.Value = Next.Value;
            Animator.Value.SetBool(Current.Value, true);

            return Status.Success;
        }


    }
}
