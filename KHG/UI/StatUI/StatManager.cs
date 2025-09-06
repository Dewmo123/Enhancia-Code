using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CSH._01.Scripts.Enemies;
using Assets._00.Work.YHB.Scripts.Core;
using Core.EventSystem;
using Core.Managers;
using Core.Network;
using ServerCode.DTO;
using System;
using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

namespace KHG.UI.Stat
{
    public class StatManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI playerId;
        [SerializeField] private TextMeshProUGUI currentGold;
        [Header("read")]
        [SerializeField] private ReadStat rDmgStat;
        [SerializeField] private ReadStat rHpStat;
        [Header("upgrade")]
        [SerializeField] private UpgradeStat uDmgStat;
        [SerializeField] private UpgradeStat uHpStat;
        [Header("etc")]
        [SerializeField] private ObjectFinderSO playerFinder;
        [SerializeField] private EventChannelSO NetworkChannel;
        [SerializeField] private float blenceLv = 10f;

        private PlayerDataDTO playerDataDTO;
        private EntityStat playerStat;
        private string curPlayer;
        private void Awake()
        {
            NetworkChannel.AddListener<GetMyDataCallbackEvent>(HandleMyData);
            playerStat = playerFinder.Object.GetComponentInChildren<EntityStat>();
        }

        private void OnDestroy()
        {
            NetworkChannel.RemoveListener<GetMyDataCallbackEvent>(HandleMyData);
        }

        private void OnEnable()
        {
            SetUI();
        }
        private void SetUI()
        {
            curPlayer = NetworkConnector.Instance.PlayerId;

            NetworkConnector.Instance.GetController<PlayerDataNetworkController>().GetMyData();
        }
        private void SetGold()
        {
            currentGold.text = $"보유 골드: {playerDataDTO.gold}G";
        }
        private void HandleMyData(GetMyDataCallbackEvent callback)
        {
            playerDataDTO = callback.result;
            SetText(playerId, $"이름 : {curPlayer}");
            SetStats();
        }
        private void SetStats()
        {
            StatSO dmgStat = playerStat.GetStat("damage");
            StatSO hpStat = playerStat.GetStat("hp");

            SetGold();

            float dmgLv = playerDataDTO.weaponLevel;
            float hpLv = playerDataDTO.armorLevel;//이거 맞나??..흠

            rDmgStat.SetUI(dmgStat.Icon, $"공격 피해 : {CalcStatCost(dmgLv) * 1.5f}", $"Lv.{dmgLv}");
            uDmgStat.SetUI(dmgStat.Icon, $"{CalcStatCost(dmgLv)}G", $"{dmgLv} -> {dmgLv + 1}");

            rHpStat.SetUI(hpStat.Icon, $"최대 체력 : {CalcStatCost(hpLv)*1.5f}", $"Lv.{hpLv}");
            uHpStat.SetUI(hpStat.Icon, $"{CalcStatCost(hpLv)}G", $"{hpLv} -> {hpLv + 1}");
        }

        private int CalcStatValue(float level)
        {
            return Mathf.RoundToInt(blenceLv * Mathf.Pow(1.5f, Mathf.RoundToInt(level-1)));
        }
        private int CalcStatCost(float level)
        {
            float a = 100f;
            float k = 0.05f;
            return Mathf.RoundToInt(a * -(1 - Mathf.Exp(k* Mathf.RoundToInt(level))));
        }

        private void SetText(TextMeshProUGUI tmp, string text)
        {
            tmp.text = text;
        }
        public void UpgradeStat(string name)
        {
            EquipType curType = name == EquipType.Weapon.ToString() ? EquipType.Weapon : EquipType.Armor;
            if(curType == EquipType.Weapon)
            {
                if (playerDataDTO.gold >= CalcStatCost(playerDataDTO.weaponLevel))
                {
                    playerDataDTO.gold -= CalcStatCost(playerDataDTO.weaponLevel);
                    playerDataDTO.weaponLevel++;
                }
            }
            else
            {
                if (playerDataDTO.gold >= CalcStatCost(playerDataDTO.armorLevel))
                {   
                    playerDataDTO.gold -= CalcStatCost(playerDataDTO.armorLevel);
                    playerDataDTO.armorLevel++; 
                }
            }
            NetworkConnector.Instance.GetController<PlayerDataNetworkController>().UpgradeEquip(curType);
            SetStats();
        }
    }
}
