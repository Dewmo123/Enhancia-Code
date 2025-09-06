using Core.EventSystem;
using KHG.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HG_ActionCategory : MonoBehaviour
{
    [SerializeField] private HG_Category category;
    [SerializeField] private Transform panelParent;
    private List<GameObject> panels = new List<GameObject>();
    private void Awake()
    {
        category.OnPanelSet += HandlePanelChange;
    }

    private void OnEnable()
    {
        GetPanels();
    }

    private void OnDestroy()
    {
        category.OnPanelSet -= HandlePanelChange;
    }

    private void HandlePanelChange()
    {
        foreach (GameObject panel in panels)
        {
            if (UIEvents.PanelChangeEvent.currentPanel == panel)
            {
                panel.SetActive(true);
            }
            else
                panel.SetActive(false);
        }
    }

    private void GetPanels()
    {
        for (int i = 0; i < panelParent.childCount; i++)
        {
            panels.Add(panelParent.GetChild(i).gameObject);
        }
    }

    public void ChangePanel(GameObject panel)
    {
        category.PanelChange(panel);
    }
}
