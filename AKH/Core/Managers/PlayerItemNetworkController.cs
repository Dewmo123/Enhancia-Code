using System.Collections.Generic;
using Core.EventSystem;
using Core.Network;
using Newtonsoft.Json;
using ServerCode.DTO;

namespace Core.Managers
{
    public class PlayerItemNetworkController : NetworkController
    {
        public PlayerItemNetworkController(WebClient client, EventChannelSO eventChannel) : base(client, eventChannel)
        {
        }
        public void GetMyItems()
        {
            var callback = NetworkEvents.GetMyItemsCallback;
            _webClient.SendGetRequest<List<PlayerItemDTO>>(
                "player-item/get-my-items"
                , result => 
                {
                    callback.results = result;
                    _networkChannel.InvokeEvent(callback);
                });
        }
        public void SendMyItems(List<PlayerItemDTO> items)
        {
            var callback = NetworkEvents.SendMyItemsCallback;
            _webClient.SendPatchRequest(
                "player-item/update-items",
                items,
                success =>
                {
                    callback.success = success;
                    _networkChannel.InvokeEvent(callback);
                });
        }
        public void AddMyItems(List<PlayerItemDTO> items)
        {
            var callback = NetworkEvents.AddItemsCallbackEvent;
            _webClient.SendPatchRequest(
                "player-item/add-items",
                items,
                success =>
                {
                    callback.success = success;
                    _networkChannel.InvokeEvent(callback);
                });
        }
    }
}
