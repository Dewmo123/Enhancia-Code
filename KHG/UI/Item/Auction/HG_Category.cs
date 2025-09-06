using Core.EventSystem;
using KHG.Events;
using System;
using UnityEngine;

public class HG_Category : BasePanel
{
    [SerializeField] private HG_ItemDisplayUI displayUI;
    [SerializeField] private GameObject AuctionUI;

    public event Action OnPanelSet;
    public void PanelChange(GameObject targetPanel)
    {
        SetProfileUI();

        var panelChangeEvent = UIEvents.PanelChangeEvent;

        panelChangeEvent.currentPanel = targetPanel;
        OnPanelSet?.Invoke();
        UIChannel.InvokeEvent(panelChangeEvent);
    }

    private void SetProfileUI()
    {
        displayUI.ResetDisplay();
    }
}
