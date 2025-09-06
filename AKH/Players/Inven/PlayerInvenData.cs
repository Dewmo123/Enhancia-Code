using _00.Work.CDH.Code.Entities;
using Core.EventSystem;
using Scripts.InvenSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _00.Work.CDH.Code.Players;
using AKH.Scripts.SO;
using Core.Network;
using Core.Managers;
using ServerCode.DTO;
using System;
using Newtonsoft.Json;
using System.Net.NetworkInformation;

//돈을 변화하는 경우
//장비강화, 옥션 물건 구매

//아이템 변화하는 경우
//장비 강화, 도감 강화, 옥션에 아이템 올리기, 캔슬하기 
namespace Players.Inven
{
    //얘의 존재 이유? -> 작업 처리 후 다시 서버에서 받아오지 않고 클라 내에서 같은 처리 & Update
    //얘는 로비씬에서만 사용하는 애니까 Add요청을 받을 필요없음
    public class PlayerInvenData : InvenData, IEntityComponent
    {
        [SerializeField] protected EventChannelSO networkChannel;

        [SerializeField] private ItemDictionarySO dicSO;
        private Player _player;
        private PlayerItemNetworkController _itemController;
        private PlayerDataNetworkController _dataController;
        public void Initialize(Entity entity)
        {
            _player = entity as Player;
            inventory = new Dictionary<ItemDataSO, InventoryItem>();
            _itemController = NetworkConnector.Instance.GetController<PlayerItemNetworkController>();
            _dataController = NetworkConnector.Instance.GetController<PlayerDataNetworkController>();
            var dic = dicSO.GetAllItems();
            dic.ToList().ForEach(item => CreateNewInventoryItem(item.Value, 0));//처음에 모든 아이템으로 인벤 만들어
            #region AddListener
            networkChannel.AddListener<UpgradeDictionaryCallbackEvent>(HandleDictionaryUpgrade);
            networkChannel.AddListener<PurchaseAuctionItemCallbackEvent>(HandlePurchaseAuctionItem);
            networkChannel.AddListener<PostAuctionItemCallbackEvent>(HandlePostAuctionItem);

            networkChannel.AddListener<GetMyItemsCallbackEvent>(HandleGetMyItems);

            #endregion

            _dataController.GetMyData();
            _itemController.GetMyItems();
        }


        private void OnDestroy()
        {
            #region RemoveListener
            networkChannel.RemoveListener<UpgradeDictionaryCallbackEvent>(HandleDictionaryUpgrade);
            networkChannel.RemoveListener<PurchaseAuctionItemCallbackEvent>(HandlePurchaseAuctionItem);
            networkChannel.RemoveListener<PostAuctionItemCallbackEvent>(HandlePostAuctionItem);

            networkChannel.RemoveListener<GetMyItemsCallbackEvent>(HandleGetMyItems);
            #endregion
        }
        #region RequestData Handler

        protected override void HandleReqInven(RequestInventoryEvent @event)
            => _itemController.GetMyItems();
        #endregion

        #region GameLogic
        private void HandlePurchaseAuctionItem(PurchaseAuctionItemCallbackEvent evt)//구매 처리
        {
            if (!evt.success)
                return;
            RemoveItem(dicSO.GetItemOrDefault(evt.buyerInfo.itemInfo.itemName), evt.buyerInfo.buyCount);
            UpdateInventory();
        }

        private void HandleDictionaryUpgrade(UpgradeDictionaryCallbackEvent evt)
        {
            if (!evt.success)
                return;
            var dto = evt.upgradeDic;
            RemoveItem(dicSO.GetItemOrDefault(dto.dictionaryKey), evt.level * 2);
            UpdateInventory();
        }
        private void HandlePostAuctionItem(PostAuctionItemCallbackEvent evt)
        {
            if (!evt.success)
                return;
            _itemController.GetMyItems();
        }
        #endregion

        #region Network Callback Handler
        private void HandleGetMyItems(GetMyItemsCallbackEvent evt)
        {
            //서버에서 값을 받아온거기 때문에 그냥 덮어씌우기
            evt.results.ForEach(item => UpdateItem(dicSO.GetItemOrDefault(item.itemName), item.quantity));
            UpdateInventory();
        }

        #endregion

        #region Update

        public void UpdateInventory()
        {
            var evt = InvenEvents.updateInventoryEvent;
            evt.inven = inventory.Values.ToList();
            inventoryChannel.InvokeEvent(evt);
        }
        #endregion

        #region ItemControl

        public override void AddItem(ItemDataSO itemData, int count = 1)
        {
            if (GetItem(itemData, out var item))
                item.AddStack(count);
            else
                Debug.LogWarning("It doesn't exist Item");
        }
        public override void UpdateItem(ItemDataSO itemData, int quantity)
        {
            if (GetItem(itemData, out var item))
                item.UpdateStack(quantity);
            else
                Debug.LogWarning("It doesn't exist Item");
        }
        private void CreateNewInventoryItem(ItemDataSO itemData, int count)
        {
            InventoryItem newItem = new InventoryItem(itemData, count);
            inventory.Add(itemData, newItem);
        }

        public override void RemoveItem(ItemDataSO itemData, int count)
        {
            if (GetItem(itemData, out var item))
                item.RemoveStack(count);
            else
                Debug.LogWarning("It doesn't exist Item");
        }
        #endregion
    }
}
