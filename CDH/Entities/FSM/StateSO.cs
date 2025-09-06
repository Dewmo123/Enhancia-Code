using System;
using _00.Work.CDH.Code.Animators;
using UnityEngine;

namespace _00.Work.CDH.Code.Entities.FSM
{
    [CreateAssetMenu(fileName = "StateSO", menuName = "SO/FSM/State")]
    public class StateSO : ScriptableObject
    {
        public string className;
        public AnimParamSO animParam;
    }
    
}
