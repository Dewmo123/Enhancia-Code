using AKH.Scripts.SO;
using Core.EventSystem;
using Core.Managers;
using Core.Network;
using Scripts.InvenSystem;
using ServerCode.DTO;
using System.Collections.Generic;
using UnityEngine;

public class LibraryItemManager : MonoBehaviour
{
    [SerializeField] private ItemDictionarySO itemDictionary;
    [SerializeField] private GameObject libraryItemUIPrefab;
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private EventChannelSO inventoryChannel;
    [SerializeField] private LibraryProfileUI libraryProfileUI;
    [SerializeField] private int poolSize = 30; // 미리 만들어둘 UI 개수

    private List<InventoryItem> _inventoryItems = new();
    private List<LibraryItem> _libraryItems = new();
    private Queue<LibraryItem> _itemPool = new();

    private ItemDataSO _curSelectedItem;

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var ui = Instantiate(libraryItemUIPrefab, transform);
            ui.SetActive(false);
            var libItem = ui.GetComponent<LibraryItem>();
            _itemPool.Enqueue(libItem);
        }
    }

    private void OnEnable()
    {
        inventoryChannel.AddListener<UpdatePlayerDataEvent>(OnPlayerDataUpdated);
        inventoryChannel.AddListener<UpdateInventoryEvent>(OnInventoryUpdated);

        inventoryChannel.InvokeEvent(InvenEvents.RequestInventoryEvent);
        inventoryChannel.InvokeEvent(InvenEvents.RequestDictionaryEvent);
    }

    private void OnDisable()
    {
        inventoryChannel.RemoveListener<UpdatePlayerDataEvent>(OnPlayerDataUpdated);
        inventoryChannel.RemoveListener<UpdateInventoryEvent>(OnInventoryUpdated);
    }

    private void OnPlayerDataUpdated(UpdatePlayerDataEvent data)
    {
        ClearLibraryUI();
        CreateLibraryUI(data.dictionary);
    }

    private void ClearLibraryUI()
    {
        foreach (var item in _libraryItems)
        {
            item.gameObject.SetActive(false);
            _itemPool.Enqueue(item);
        }
        _libraryItems.Clear();
    }

    private void CreateLibraryUI(Dictionary<string, int> libraryData)
    {
        Debug.Log("CreateLibraryUI");
        foreach (var (itemKey, level) in libraryData)
        {
            var itemData = itemDictionary.GetItemOrDefault(itemKey);
            var icon = itemData.Icon ?? defaultSprite;
            var amount = GetInventoryAmount(itemData);
            var canUpgrade = amount >= GetUpgradeCost(level);

            LibraryItem libItem;
            if (_itemPool.Count > 0)
            {
                libItem = _itemPool.Dequeue();
                libItem.gameObject.SetActive(true);
            }
            else
            {
                var ui = Instantiate(libraryItemUIPrefab, transform);
                libItem = ui.GetComponent<LibraryItem>();
            }

            libItem.SetItemUI(icon, itemData.Name, level, canUpgrade);

            libItem.SetButtonEvent(() =>
            {
                if(_curSelectedItem == null || _curSelectedItem != itemData)
                {
                    ShowItemProfile(itemData, level, amount);
                    _curSelectedItem = itemData;
                }
                else
                {
                    print("이미 구독중인데");
                }
            });
            _libraryItems.Add(libItem);
        }
    }

    private int GetInventoryAmount(ItemDataSO item)
    {
        return _inventoryItems.Find(i => i.data == item)?.stackSize ?? 0;
    }

    private int GetUpgradeCost(int level)
    {
        return level*2;
    }

    private void ShowItemProfile(ItemDataSO item, int level, int amount)
    {
        libraryProfileUI.SetLibraryProfile(
            item.Icon,
            item.Name,
            item.Description,
            level,
            amount,
            GetUpgradeCost(level),
            () =>
            {
                TryUpgrade(item, level+1, amount);
            }
        );
    }

    private void TryUpgrade(ItemDataSO item, int level, int amount)
    {
        int cost = GetUpgradeCost(level-1);

        if (amount < cost)
        {
            Debug.Log($"Not enough items to upgrade.\ncur:{amount}, need:{cost}");
            return;
        }
        DictionaryUpgradeDTO dto = new DictionaryUpgradeDTO()
        {
            dictionaryKey = item.Name,
        };
        NetworkConnector.Instance.GetController<PlayerDataNetworkController>().UpgradeDictionary(dto,level);
        ShowItemProfile(item, level, amount - cost);
    }
    private void OnInventoryUpdated(UpdateInventoryEvent data)
    {
        Debug.Log("getUpdatee");
        _inventoryItems = data.inven;
    }
}
