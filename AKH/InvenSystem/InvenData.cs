using Core.EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.InvenSystem
{
    public abstract class InvenData : MonoBehaviour
    {
        protected Dictionary<ItemDataSO, InventoryItem> inventory;
        [SerializeField] protected EventChannelSO inventoryChannel;
        private void Awake()
        {
            inventory = new Dictionary<ItemDataSO, InventoryItem>();
            inventoryChannel.AddListener<RequestInventoryEvent>(HandleReqInven);
        }
        private void OnDestroy()
        {
            inventoryChannel.RemoveListener<RequestInventoryEvent>(HandleReqInven);
        }

        protected abstract void HandleReqInven(RequestInventoryEvent evt);


        public virtual bool GetItem(ItemDataSO itemData, out InventoryItem item) => inventory.TryGetValue(itemData, out item);

        public abstract void AddItem(ItemDataSO itemData, int count = 1);
        public abstract void RemoveItem(ItemDataSO itemData, int count);
        public abstract void UpdateItem(ItemDataSO itemData, int quantity);


    }
}