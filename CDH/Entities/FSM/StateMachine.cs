
using System;
using System.Collections.Generic;
using _00.Work.CDH.Code.Players.States;
using UnityEngine;

namespace _00.Work.CDH.Code.Entities.FSM
{
    public class StateMachine
    {
        public EntityState CurrentState { get; private set; }

        private Dictionary<string, EntityState> _states;

        public StateMachine(Entity entity, StateListSO stateList)
        {
            _states = new Dictionary<string, EntityState>();
            foreach(StateSO state in stateList.states)
            {
                Type type = Type.GetType(state.className);
                Debug.Assert(type != null, $"Finding type is null {state.className}");
                EntityState entityState = Activator.CreateInstance(type, entity, state.animParam) as EntityState;
                _states.Add(state.animParam.parameterName, entityState);
            }
        }
        public void ChangeState(string newStateName)
        {
            CurrentState?.Exit();

            EntityState newState = _states.GetValueOrDefault(newStateName);
            Debug.Assert(newState != null, $"State is null. {newStateName}");

            CurrentState = newState;
            CurrentState.Enter();
        }
        public void UpdateStateMachine()
        {
            CurrentState.Update();
        }
    }
}
