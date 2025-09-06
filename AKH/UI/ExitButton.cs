using Core.EventSystem;
using KHG.Events;
using UnityEngine;

namespace Scripts.UI
{
    public class ExitButton : MonoBehaviour
    {
        [SerializeField] private EventChannelSO uiChannel;

        public void Exit()
        {
            var evt = UIEvents.SetPanelevent;
            evt.isActive = false;
            uiChannel.InvokeEvent(evt);
        }
    }
}
