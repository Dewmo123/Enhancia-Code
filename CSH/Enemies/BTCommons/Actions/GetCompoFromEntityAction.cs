using _00.Work.CDH.Code.Entities;
using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using Action = Unity.Behavior.Action;

namespace _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions

{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "GetCompoFromEntity", story: "Get Compnents from [BTEnemy]", category: "Action", id: "f23352f191b370e39790545e1fb2b531")]
    public partial class GetCompoFromEntityAction : Action
    {
        [SerializeReference] public BlackboardVariable<BTEnemy> BTEnemy;

        protected override Status OnStart()
        {

            BTEnemy enemy = BTEnemy.Value;

            EntityRenderer rendererCompo = enemy.GetCompo<EntityRenderer>();


            SetVariableToBT(enemy, "Mover", enemy.GetCompo<EntityMover>());
            SetVariableToBT(enemy, "Renderer", enemy.GetCompo<EntityRenderer>());
            SetVariableToBT(enemy, "MainAnimator", enemy.GetCompo<EntityRenderer>().GetComponent<Animator>());
            SetVariableToBT(enemy, "AnimationTrigger", enemy.GetCompo<EntityAnimationTrigger>());


            return Status.Success;
        }

        private void SetVariableToBT<T>(BTEnemy enemy, string variableName, T component)
        {
            BlackboardVariable<T> variable = enemy.GetBlackboardVariable<T>(variableName);
            variable.Value = component;
        }
    }
}