using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Linq;

namespace AKH.Scripts.SO
{
    [CreateAssetMenu(fileName = "SO", menuName = "SO/ItemDic", order = 0)]
    public class ItemDictionarySO : ScriptableObject
    {
        [SerializeField] private ItemDataSO[] items;
        private Dictionary<string, ItemDataSO> itemDatas;
        public ItemDataSO GetItemOrDefault(string key)
            => itemDatas.GetValueOrDefault(key);
        public bool ContainsKey(string key)
            => itemDatas.ContainsKey(key);
        public Dictionary<string,ItemDataSO> GetAllItems()
        {
            Dictionary<string, ItemDataSO> newDic = new(itemDatas);
            return newDic;
        }
        private void OnEnable()
        {
            itemDatas = new();
            items.ToList().ForEach(item => itemDatas.Add(item.Name, item));
        }
    }
}
