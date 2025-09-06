using Core.EventSystem;
using KHG.UI;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace KHG.Events
{
    public class UIEvents
    {
        public static readonly PanelChangeEvent PanelChangeEvent = new();
        public static readonly PanelStateEvent PanelStateEvent = new();
        public static readonly ItemSelectEvent ItemSelectEvent = new();
        public static readonly ChangeItemEvent ChangeItemEvent = new();
        public static readonly SetPanelEvent SetPanelevent = new();
        public static readonly SceneChangeEvent SceneChangeEvent = new();
    }
    public class SetPanelEvent : GameEvent
    {
        public UIType ui;
        public bool isActive;
    }
    public class ItemSelectEvent : GameEvent
    {
        /// <summary>
        /// items[0] == ItemCount<br/>
        /// items[1] == ItemPrice
        /// </summary>
        /// <param name="items"></param>
        public delegate void SelectCommand(params int[] items);
        public SelectCommand command;
        public ItemDataSO item;
        public string buttonName, playerId;
        public bool usingSlider, usingInput, needPrice;
        public int quantity, price;
    }
    public class RefreshUIEvent : GameEvent
    {
        public bool success;
    }

    public class ChangeItemEvent : GameEvent
    {
        public ItemDataSO item;
    }

    public class PanelChangeEvent : GameEvent
    {
        public GameObject currentPanel;
    }
    public class PanelStateEvent : GameEvent
    {
        public GameObject panel;
    }


    public class SceneChangeEvent : GameEvent
    {
        public string sceneName;
        public bool targetState;
        public bool useSceneChange = true;
        public float changeSpeed;
    }
}
