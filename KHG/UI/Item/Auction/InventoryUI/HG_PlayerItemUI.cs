using AKH.Scripts.SO;
using Core.EventSystem;
using Core.Managers;
using Core.Network;
using KHG.Events;
using Scripts.InvenSystem;
using ServerCode.DTO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HG_PlayerItemUI : MonoBehaviour
{
    [SerializeField] private Transform itemPrefab;
    [SerializeField] private RectTransform gridRoot, contentRoot;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private EventChannelSO invenChannel, uiChannel;
    [SerializeField] private GameObject emptyNotice;
    [SerializeField] private HG_ItemDisplayUI displayUI;
    [SerializeField] private ItemDictionarySO itemDict;

    private readonly List<GameObject> itemListObjs = new();

    private void OnEnable()
    {
        invenChannel.AddListener<UpdateInventoryEvent>(HandleInventory);
        NetworkConnector.Instance.GetController<PlayerItemNetworkController>().GetMyItems();
    }

    private void OnDisable()
    {
        invenChannel.RemoveListener<UpdateInventoryEvent>(HandleInventory);
    }

    private void HandleInventory(UpdateInventoryEvent evt)
    {
        int count = evt.inven.Count;
        RefreshUI(count);

        emptyNotice.SetActive(count == 0);
        if (count == 0) return;

        for (int i = 0; i < count; i++)
        {
            var data = itemDict.GetItemOrDefault(evt.inven[i].data.Name);
            var info = evt.inven[i];
            var obj = itemListObjs[i];
            var itemUI = obj.GetComponentInChildren<InvItem>();
            itemUI.SetInvItem(data.Icon, data.Name, info.stackSize);
            ConfigureItemButton(data, info, obj);
        }
    }

    private void ConfigureItemButton(ItemDataSO data, InventoryItem info, GameObject obj)
    {
        var item = data.Clone() as ItemDataSO;
        var evt = new ItemSelectEvent
        {
            item = item,
            buttonName = "경매장 등록",
            usingSlider = true,
            usingInput = true,
            playerId = "",
            quantity = info.stackSize, //슬라이더에 사용할 수량 지정
            needPrice = false,
            command = values =>
            {
                Debug.Log(item.Name);
                Upload(item.Name, values);
                invenChannel.InvokeEvent(InvenEvents.RequestInventoryEvent);
            }
        };
        obj.GetComponentInChildren<Button>().onClick.AddListener(() =>
        {
            uiChannel.InvokeEvent(evt);
        });
    }


    private void Upload(string Name, params int[] values)
    {
        //Debug.Log(Name);
        var dto = new AuctionItemDTO
        {
            playerId = "",
            itemName = Name,
            quantity = values[0],
            pricePerUnit = values[1]
        };

        NetworkConnector.Instance.GetController<AuctionNetworkController>().PostItemToAuction(dto);
    }

    private void RefreshUI(int count)
    {
        foreach (var obj in itemListObjs) Destroy(obj);
        itemListObjs.Clear();
        Debug.Log($"RefreshUI count: {count}");
        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(itemPrefab, gridRoot).gameObject;
            obj.name = $"ItemListUI_{i}";
            itemListObjs.Add(obj);
        }

        ResizeGrid(count);
    }

    private void ResizeGrid(int count)
    {
        if (gridLayout == null || gridRoot == null) return;

        int cols = gridLayout.constraintCount;
        int rows = Mathf.CeilToInt((float)count / cols);

        float cellHeight = gridLayout.cellSize.y;
        float spacingY = gridLayout.spacing.y;

        float totalHeight = (rows * cellHeight) + ((rows - 1) * spacingY);

        contentRoot.sizeDelta = new Vector2(contentRoot.sizeDelta.x, totalHeight);
    }
    //qwweewq3
}
