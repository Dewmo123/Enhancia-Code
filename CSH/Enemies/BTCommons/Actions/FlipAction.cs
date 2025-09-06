using _00.Work.CDH.Code.Entities;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

namespace _00.Work.CSH._01.Scripts.Enemies.BTCommons.Actions
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "Flip", story: "Filp [renderer]", category: "Action", id: "753ee30bb6e44a50fda574557c7c1bde")]
    public partial class FlipAction : Action
    {
        [SerializeReference] public BlackboardVariable<EntityRenderer> Renderer;

        protected override Status OnStart()
        {
            Renderer.Value.Filp();
            return Status.Success;
        }

    }    
}

