using Core.EventSystem;
using KHG.Events;
using UnityEngine;

namespace Scripts.Interactions
{
    public enum Scene
    {
        Title = 0,
        Lobby = 1,
        Game = 2
    }
    public class ChangeSceneNpc : MonoBehaviour, ICanInteract
    {
        [SerializeField] private EventChannelSO uiChannel;

        [SerializeField] private string sceneName;
        public void Interact()
        {
            var evt = UIEvents.SceneChangeEvent;
            evt.targetState = false;
            evt.sceneName = sceneName;
            evt.changeSpeed = 1;
            uiChannel.InvokeEvent(evt);
        }
    }
}
