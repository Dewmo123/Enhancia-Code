using Core.EventSystem;
using Core.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Network
{
    [DefaultExecutionOrder(-20)]
    public class NetworkConnector : MonoBehaviour
    {
        private static NetworkConnector _instance;
        private WebClient _webClient;
        public static NetworkConnector Instance => _instance;

        [SerializeField] private EventChannelSO networkChannel;

        [SerializeField] private string[] controllerNames;
        [SerializeField] private string managerNamespace;

        private Dictionary<Type, NetworkController> _controllers;
        public string PlayerId { get; private set; }
        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Debug.LogWarning("Instance already exists in this scene");

            DontDestroyOnLoad(this);

            _webClient = new WebClient("http://sdludb.duckdns.org:3303/api");
            _controllers = new Dictionary<Type, NetworkController>();
            foreach (var item in controllerNames)
            {
                var t = Type.GetType($"{managerNamespace}.{item}");
                var controller = Activator.CreateInstance(t, _webClient, networkChannel) as NetworkController;
                if (_controllers.ContainsKey(controller.GetType()))
                    Debug.LogWarning("Same Controller");
                _controllers.Add(controller.GetType(), controller);
            }
            networkChannel.AddListener<LogInCallbackEvent>(SetPlayerId);
        }

        private void SetPlayerId(LogInCallbackEvent @event)
        {
            if (@event.success)
                PlayerId = @event.playerId;
        }

        public T GetController<T>() where T : NetworkController => (T)_controllers.GetValueOrDefault(typeof(T));
    }
}
