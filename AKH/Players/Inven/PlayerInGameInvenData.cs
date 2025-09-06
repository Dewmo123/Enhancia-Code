using _00.Work.CDH.Code.Entities;
using Core.EventSystem;
using Core.Managers;
using Core.Network;
using Scripts.Combat;
using Scripts.InvenSystem;
using ServerCode.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Players.Inven
{
    public class PlayerInGameInvenData : InvenData, IEntityComponent
    {
        public void Initialize(Entity entity)
        {
            inventoryChannel.AddListener<RequestAddItemsEvent>(HandleAddItemsEvents);
        }
        private void OnDestroy()
        {
            inventoryChannel.RemoveListener<RequestAddItemsEvent>(HandleAddItemsEvents);
        }
        private void HandleAddItemsEvents(RequestAddItemsEvent evt)
        {
            while (evt.dropInfos.Count > 0)
            {
                DropInfo info = evt.dropInfos.Dequeue();
                AddItem(info.item, info.quantity);
                Debug.Log(info.item.Name);
            }
        }
        public void SendInvenToServer()
        {
            List<PlayerItemDTO> items = new List<PlayerItemDTO>(inventory.Count);
            inventory.Values.ToList().ForEach((item) => 
            items.Add(
                new PlayerItemDTO { itemName = item.data.Name, quantity = item.stackSize }
            ));
            NetworkConnector.Instance.GetController<PlayerItemNetworkController>().AddMyItems(items);
            inventory.Clear();
        }
        protected override void HandleReqInven(RequestInventoryEvent evt)
        {
            UpdateInven();
        }
        private void UpdateInven()
        {
            var evt = InvenEvents.updateInventoryEvent;
            evt.inven = inventory.Values.ToList();
            inventoryChannel.InvokeEvent(evt);
        }
        #region InvenUtiles
        public override void AddItem(ItemDataSO itemData, int count = 1)
        {
            if(GetItem(itemData,out var item))
                item.AddStack(count);
            else
                CreateNewInventoryItem(itemData, count);
        }
        private void CreateNewInventoryItem(ItemDataSO itemData, int count)
        {
            InventoryItem newItem = new InventoryItem(itemData, count);
            inventory.Add(itemData,newItem);
        }
        public override void RemoveItem(ItemDataSO itemData, int count)
        {
        }
        public override void UpdateItem(ItemDataSO itemData, int quantity)
        {
        }
        #endregion
    }
}
