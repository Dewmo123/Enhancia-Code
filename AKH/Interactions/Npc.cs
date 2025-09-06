using Core.EventSystem;
using KHG.Events;
using KHG.UI;
using UnityEngine;

namespace Scripts.Interactions
{
    public class Npc : MonoBehaviour, ICanInteract
    {
        [SerializeField] private EventChannelSO uiChannel;
        [SerializeField] private UIType uiType;
        public void Interact()
        {
            var evt = UIEvents.SetPanelevent;
            evt.ui = uiType;
            evt.isActive = true;
            uiChannel.InvokeEvent(evt);
        }
    }
}
