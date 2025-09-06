using AKH.Scripts.SO;
using Core.EventSystem;
using Core.Managers;
using Core.Network;
using KHG.Events;
using ServerCode.DTO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HG_SellingUI : MonoBehaviour
{
    [SerializeField] private Transform itemPrefab;
    [SerializeField] private RectTransform gridRoot, contentRoot;
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private EventChannelSO networkChannel, uiChannel;
    [SerializeField] private GameObject emptyNotice;
    [SerializeField] private HG_ItemDisplayUI displayUI;
    [SerializeField] private ItemDictionarySO itemDict;

    private readonly List<GameObject> itemListObjs = new();

    private void OnEnable()
    {
        networkChannel.AddListener<GetItemsByPlayerIdCallbackEvent>(HandleSellingItem);
        NetworkConnector.Instance.GetController<AuctionNetworkController>().GetAuctionItemsByPlayerId();
        NetworkConnector.Instance.GetController<PlayerItemNetworkController>().GetMyItems();
    }
    private void OnDisable()
    {
        networkChannel.RemoveListener<GetItemsByPlayerIdCallbackEvent>(HandleSellingItem);
    }

    private void HandleSellingItem(GetItemsByPlayerIdCallbackEvent callback)
    {
        emptyNotice.SetActive(true);
        int cnt = callback.results.Count;
        RefreshUI(cnt);
        if (cnt == 0) return;
        emptyNotice.SetActive(false);
        for (int i = 0; i < cnt; i++)
        {
            print(callback.results[i].itemName);
            print(callback.results[i].playerId);
            var data = itemDict.GetItemOrDefault(callback.results[i].itemName);
            var info = callback.results[i];
            var obj = itemListObjs[i];
            var itemUI = obj.GetComponentInChildren<InvItem>();
            itemUI.SetInvItem(data.Icon, data.Name, info.quantity);
            ConfigureItemButton(info, itemUI, data);
        }

        StartCoroutine(ListAnimation());
    }

    private IEnumerator ListAnimation()
    {
        foreach (var item in itemListObjs)
        {
            yield return new WaitForSeconds(0.2f);
            var animator = item.GetComponent<Animator>();
            if (animator != null && animator.runtimeAnimatorController != null)
            {
                animator.SetTrigger("Spawn");
            }
            else
            {
                Debug.LogWarning($"Animator missing or no Controller on {item.name}");
            }
        }
    }
    private void ConfigureItemButton(AuctionItemDTO info, InvItem invItem, ItemDataSO itemSO)
    {
        invItem.SetButton(()
            =>
        {
            displayUI.SetDisplay(new ItemSelectEvent
            {
                command = (value) =>
                {
                    NetworkConnector.Instance.GetController<AuctionNetworkController>().DeleteMyAuctionItem(info.itemName, info.pricePerUnit);
                    uiChannel.InvokeEvent(new RefreshUIEvent());
                },
                item = itemSO,
                buttonName = "³»¸®±â",
                usingSlider = false,
                usingInput = false,
                quantity = info.quantity
            });
        });
    }

    private void RefreshUI(int count)
    {
        foreach (var obj in itemListObjs) Destroy(obj);
        itemListObjs.Clear();

        for (int i = 0; i < count; i++)
        {
            var obj = Instantiate(itemPrefab, gridRoot).gameObject;
            obj.name = $"MyAuctionItem_{i}";
            itemListObjs.Add(obj);
        }

        ResizeGrid(count);
    }

    private void ResizeGrid(int count)
    {
        if (gridLayout == null || contentRoot == null) return;

        int cols = gridLayout.constraintCount;
        int rows = Mathf.CeilToInt((float)count / cols);

        float cellHeight = gridLayout.cellSize.y;
        float spacingY = gridLayout.spacing.y;

        float totalHeight = (rows * cellHeight) + ((rows - 1) * spacingY);
        Debug.Log($"[ResizeGrid] Rows: {rows}, Total Height: {totalHeight}");

        contentRoot.sizeDelta = new Vector2(contentRoot.sizeDelta.x, totalHeight);
    }

}
