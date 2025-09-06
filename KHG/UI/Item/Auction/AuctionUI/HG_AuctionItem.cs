using AKH.Scripts.SO;
using Core.EventSystem;
using Core.Managers;
using Core.Network;
using KHG.Events;
using ServerCode.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HG_AuctionItem : MonoBehaviour
{
    [SerializeField] private TMP_InputField searchInputField;
    [SerializeField] private EventChannelSO networkChannel;
    [SerializeField] private EventChannelSO uiChannel;
    [SerializeField] private GameObject itemUISample;
    [SerializeField] private TextMeshProUGUI noticeTMP;
    [SerializeField] private HG_ItemDisplayUI displayUI;
    [SerializeField] private ItemDictionarySO dicSO;
    [SerializeField] private HG_PopupUI popupUI;

    private readonly List<GameObject> _itemUIs = new();

    private void OnEnable()
    {
        searchInputField.text = string.Empty;
        ClearItemUI();
        networkChannel.AddListener<GetItemsByIdCallbackEvent>(SetItemListUI);
        ShowNotice("ã������ �������� �˻����ּ���...");
    }

    private void OnDisable()
    {
        networkChannel.RemoveListener<GetItemsByIdCallbackEvent>(SetItemListUI);
    }

    public void SearchItem()
    {
        ClearItemUI();
        RequestItem(searchInputField.text);
    }

    private void ClearItemUI()
    {
        foreach (var item in _itemUIs)
        {
            Destroy(item);
        }
        _itemUIs.Clear();
    }

    private void RequestItem(string name)
    {
        string trimmedName = name.Trim().Replace("\u200B", "");
        if (string.IsNullOrEmpty(trimmedName)) return;

        try
        {
            NetworkConnector.Instance
                .GetController<AuctionNetworkController>()
                .GetAuctionItemsByItemName(trimmedName);
        }
        catch (Exception e)
        {
            Debug.LogError($"������ ��û ����: {e.Message}");
        }
    }

    private void SetItemListUI(GetItemsByIdCallbackEvent callbackEvent)
    {
        ClearItemUI();

        if (callbackEvent.results == null || callbackEvent.results.Count == 0)
        {
            ShowNotice("�ش� �������� ã�� �� �����ϴ�.");
            return;
        }

        ShowNotice(string.Empty);

        foreach (var item in callbackEvent.results)
        {
            var ui = Instantiate(itemUISample, transform);
            _itemUIs.Add(ui);

            var itemUI = ui.GetComponentInChildren<InvItem>();
            Debug.Log("������ ����Ʈ ���� - ����: " + item.itemName);
            SetupItemUI(itemUI, item);
        }

        StartCoroutine(ListAnimation());
    }

    private void SetupItemUI(InvItem itemUI, AuctionItemDTO item)
    {
        itemUI.SetInvItem(null, item.itemName, item.quantity, item.pricePerUnit);

        var itemSO = dicSO.GetItemOrDefault(item.itemName);

        var uiEvent = new ItemSelectEvent(); //���� �̺�Ʈ �ν��Ͻ� ����
        uiEvent.item = itemSO;
        uiEvent.buttonName = "����";
        uiEvent.quantity = item.quantity;
        uiEvent.price = item.pricePerUnit;
        uiEvent.usingInput = false;
        uiEvent.usingSlider = true;
        uiEvent.playerId = item.playerId;
        uiEvent.needPrice = true;

        //item �����ؼ� ���ٷ� �ѱ�
        uiEvent.command = (items) =>
        {
            PurchaseItem(item, items);
        };

        itemUI.gameObject.GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("Ŭ���� ������: " + item.itemName);
            uiChannel.InvokeEvent(uiEvent);
        });
    }

    private void PurchaseItem(AuctionItemDTO selectedItem, params int[] items)
    {
        if (selectedItem == null || items.Length == 0) return;

        var buyerInfo = new BuyerDTO
        {
            buyerId = NetworkConnector.Instance.PlayerId,
            itemInfo = selectedItem,
            buyCount = items[0]
        };
        
        Debug.Log($"[���� ��û] ������: {selectedItem.itemName}, ����: {items[0]}, ����: {selectedItem.pricePerUnit}");

        NetworkConnector.Instance
            .GetController<AuctionNetworkController>()
            .Purchase(buyerInfo);
    }
    private IEnumerator ListAnimation()
    {
        foreach (var item in _itemUIs)
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
    private void ShowNotice(string message)
    {
        noticeTMP.text = message;
    }
}
