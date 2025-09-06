using _00.Work.CDH.Code.Entities;
using Core.EventSystem;
using Core.Managers;
using Core.Network;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

namespace Players.Inven
{
    public class PlayerDataStorage : MonoBehaviour, IEntityComponent
    {
        [SerializeField] protected EventChannelSO networkChannel;
        [SerializeField] protected EventChannelSO inventoryChannel;

        public Dictionary<string, int> Dictionary { get; private set; }
        public int Gold { get; private set; }

        public void Initialize(Entity entity)
        {
            networkChannel.AddListener<GetMyDataCallbackEvent>(HandleGetMyData);
            inventoryChannel.AddListener<RequestPlayerDataEvent>(HandleReqData);
            networkChannel.AddListener<UpgradeDictionaryCallbackEvent>(HandleDictionaryUpgrade);
            networkChannel.AddListener<PurchaseAuctionItemCallbackEvent>(HandlePurchaseAuctionItem);

        }
        private void OnDestroy()
        {
            inventoryChannel.RemoveListener<RequestPlayerDataEvent>(HandleReqData);
            networkChannel.RemoveListener<GetMyDataCallbackEvent>(HandleGetMyData);
            networkChannel.RemoveListener<UpgradeDictionaryCallbackEvent>(HandleDictionaryUpgrade);
            networkChannel.RemoveListener<PurchaseAuctionItemCallbackEvent>(HandlePurchaseAuctionItem);
        }
        private void HandlePurchaseAuctionItem(PurchaseAuctionItemCallbackEvent evt)//구매 처리
        {
            if (!evt.success)
                return;
            ChangeGold(-evt.buyerInfo.NeededMoney);
            UpdatePlayerData();
        }

        private void HandleDictionaryUpgrade(UpgradeDictionaryCallbackEvent evt)
        {
            if (!evt.success)
                return;
            var dto = evt.upgradeDic;
            Dictionary[dto.dictionaryKey]++;
            UpdatePlayerData();
        }
        private void HandleGetMyData(GetMyDataCallbackEvent @event)
        {
            //얘도 마찬가지
            Dictionary = JsonConvert.DeserializeObject<Dictionary<string, int>>(@event.result.dictionary);
            Gold = @event.result.gold;
            UpdatePlayerData();
        }
        public void UpdatePlayerData()
        {
            var evt = InvenEvents.SendPlayerDataEvent;
            evt.dictionary = Dictionary;
            evt.gold = Gold;
            inventoryChannel.InvokeEvent(evt);
        }
        public void ChangeGold(int value)
        {
            Gold += value;
        }
        protected void HandleReqData(RequestPlayerDataEvent @event)
    => NetworkConnector.Instance.GetController<PlayerDataNetworkController>().GetMyData();
    }
}
