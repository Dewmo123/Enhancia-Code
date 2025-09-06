using Core.EventSystem;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.NetworkHandlers
{
    public class TitleSceneLogic : MonoBehaviour
    {
        [SerializeField] private EventChannelSO networkChannel;

        private void Start()
        {
            networkChannel.AddListener<LogInCallbackEvent>(HandleLogin);
        }
        private void OnDestroy()
        {
            networkChannel.RemoveListener<LogInCallbackEvent>(HandleLogin);
        }
        private void HandleLogin(LogInCallbackEvent @event)
        {
            if (@event.success)
                SceneManager.LoadScene(1);
        }
    }
}
