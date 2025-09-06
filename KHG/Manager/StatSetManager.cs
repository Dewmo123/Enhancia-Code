using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Entities.FSM;
using _00.Work.CSH._01.Scripts.Enemies;
using UnityEngine;

namespace KHG_Manager
{
    public class StatSetManager : MonoBehaviour
    {
        [SerializeField] private EntityFinderSO playerFinder;

        private EntityStat playerStat;
        private void Initialze()
        {
            playerStat = playerFinder.target.GetCompo<EntityStat>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">damage와hp</param>
        /// <returns></returns>
        public StatSO GetStat(string name)
        {
            return playerStat.GetStat(name);
        }

        public void SetStat(StatSO stat,float value)
        {
            playerStat.GetStat(name).AddModifier(this, value);
            //빼줄떈 AddModifier가 아니라 RemoveModifier
        }
    }
}
