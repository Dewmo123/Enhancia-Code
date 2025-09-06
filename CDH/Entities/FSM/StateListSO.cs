using System.Collections.Generic;
using UnityEngine;

namespace _00.Work.CDH.Code.Entities.FSM
{
    [CreateAssetMenu(fileName = "StateList", menuName = "SO/FSM/StateList")]
    public class StateListSO : ScriptableObject
    {
        public List<StateSO> states;
    }
}
