using _00.Work.CDH.Code.Players;
using AYellowpaper.SerializedCollections;
using Core.EventSystem;
using KHG.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KHG.UI
{
    public class UIManagement : MonoBehaviour
    {
        [SerializeField] private EventChannelSO uiChannel;
        [SerializeField] private PlayerInputSO playerInput;
        public SerializedDictionary<UIType,ChangeableUI> panels;

        private UIType _currentUI = UIType.NULL;
        private void Awake()
        {
            uiChannel.AddListener<SetPanelEvent>(OnSetPanel);
        }
        private void OnDestroy()
        {
            uiChannel.RemoveListener<SetPanelEvent>(OnSetPanel);
        }
        private void OnSetPanel(SetPanelEvent callback)
        {
            if (callback.isActive)
                ShowPanel(callback.ui);
            else
                HideAllPanels();
        }

        public void ShowPanel(UIType type)
        {
            if(panels[_currentUI] != null) panels[_currentUI].Hide();
            panels[type].Show();
            _currentUI = type;
            playerInput.SetEnabled(false);
        }

        public void HideAllPanels()
        {
            if (panels[_currentUI] != null) panels[_currentUI].Hide();
            _currentUI = UIType.NULL;
            playerInput.SetEnabled(true);
        }
    }
    public enum UIType//이거 메서드랑 이름 똑같이 해서 이름 바꾸면 에러남
    {
        AuctionUI=0,
        LibraryUI=1,
        LogInUI=2,
        StatUI=3,
        NULL=4,
        SkillManage=5,
        SettingUI=6
    }
}