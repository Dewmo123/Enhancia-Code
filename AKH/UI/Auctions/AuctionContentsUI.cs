using Core.EventSystem;
using Core.Managers;
using Core.Network;
using ServerCode.DTO;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace AKH.UI.Auctions
{
    public class AuctionContentsUI : MonoBehaviour
    {
        private AuctionItemUI[] _contents;
        [SerializeField] private EventChannelSO networkChannel;

        private void Awake()
        {
            _contents = GetComponentsInChildren<AuctionItemUI>();
            networkChannel.AddListener<LogInCallbackEvent>(TestLogin);
            networkChannel.AddListener<GetItemsByIdCallbackEvent>(SetUIs);
            //NetworkControllerManager.Instance.LogIn("qwweewq2", "qqwweedd");
        }
        private void Start()
        {
            var playerController = NetworkConnector.Instance.GetController<PlayerInfoNetworkController>();
            playerController.LogIn("qwweewq2", "qqwweedd");
        }
        public void TestLogin(LogInCallbackEvent val)
        {
            if (val.success)
                GetItemsById("sword");
        }
        public void GetItemsById(string itemName)
        {
            NetworkConnector.Instance.GetController<AuctionNetworkController>().GetAuctionItemsByItemName(itemName);
            //StartCoroutine(_auctionClient.SendGetRequest<List<AuctionItemDTO>>($"get-items?itemName={itemName}",SetUIs));
        }
        public void SetUIs(GetItemsByIdCallbackEvent callbackEvent)
        {
            int uiCount = 0;
            foreach(var item in callbackEvent.results)
            {
                _contents[uiCount++].SetUI(null, item);
            }
        }
    }
}
