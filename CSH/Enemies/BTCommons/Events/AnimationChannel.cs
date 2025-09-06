using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;


namespace _00.Work.CSH._01.Scripts.Enemies.BTCommons.Events
{
#if UNITY_EDITOR
    [CreateAssetMenu(menuName = "Behavior/Event Channels/AnimationChannel")]
#endif
    [Serializable, GeneratePropertyBag]
    [EventChannelDescription(name: "AnimationChannel", message: "change animation to [next]", category: "Events", id: "b6ec2e88b906b5ee12af0a8aae11b581")]
    public partial class AnimationChannel : EventChannelBase
    {
        public delegate void AnimationChannelEventHandler(string next);
        public event AnimationChannelEventHandler Event;

        public void SendEventMessage(string next)
        {
            Event?.Invoke(next);
        }

        public override void SendEventMessage(BlackboardVariable[] messageData)
        {
            BlackboardVariable<string> nextBlackboardVariable = messageData[0] as BlackboardVariable<string>;
            var next = nextBlackboardVariable != null ? nextBlackboardVariable.Value : default(string);

            Event?.Invoke(next);
        }

        public override Delegate CreateEventHandler(BlackboardVariable[] vars, System.Action callback)
        {
            AnimationChannelEventHandler del = (next) =>
            {
                BlackboardVariable<string> var0 = vars[0] as BlackboardVariable<string>;
                if (var0 != null)
                    var0.Value = next;

                callback();
            };
            return del;
        }

        public override void RegisterListener(Delegate del)
        {
            Event += del as AnimationChannelEventHandler;
        }

        public override void UnregisterListener(Delegate del)
        {
            Event -= del as AnimationChannelEventHandler;
        }
    }

}