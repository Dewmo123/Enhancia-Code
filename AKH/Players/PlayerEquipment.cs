using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using AYellowpaper.SerializedCollections;
using Core.EventSystem;
using Core.Managers;
using Core.Network;
using ServerCode.DTO;
using System;
using System.Linq;
using UnityEngine;

namespace Players
{
    [Serializable]
    public struct StatInfo
    {
        public StatSO stat;
        public int level;
    }
    public class PlayerEquipment : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private EventChannelSO networkChannel;
        public SerializedDictionary<EquipType, StatInfo> stats;

        private EntityStat _statCompo;
        public void Initialize(Entity entity)
        {
            _statCompo = entity.GetCompo<EntityStat>();
            networkChannel.AddListener<GetMyDataCallbackEvent>(HandleGetMyData);
            NetworkConnector.Instance.GetController<PlayerDataNetworkController>().GetMyData();
        }
        private void OnDestroy()
        {
            networkChannel.RemoveListener<GetMyDataCallbackEvent>(HandleGetMyData);
        }
        private void HandleGetMyData(GetMyDataCallbackEvent evt)
        {
            ChangeStatLevel(EquipType.Weapon, evt.result.weaponLevel);
            ChangeStatLevel(EquipType.Armor, evt.result.armorLevel);
        }
        private void ChangeStatLevel(EquipType type,int level)
        {
            if(stats.TryGetValue(type, out StatInfo info))
            {
                _statCompo.RemoveModifier(info.stat, info.level);
                info.level = level;
                _statCompo.AddModifier(info.stat, info.level, CalcStatCost(info.level)*1.5f);
                stats[type] = info;
                Debug.Log(CalcStatCost(info.level));
            }
        }
        private int CalcStatCost(float level)
        {
            float a = 100f;
            float k = 0.05f;
            return Mathf.RoundToInt(a * -(1 - Mathf.Exp(k * Mathf.RoundToInt(level))));
        }
    }
}
