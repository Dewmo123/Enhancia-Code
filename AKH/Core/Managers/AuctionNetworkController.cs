using Core.EventSystem;
using Core.Network;
using ServerCode.DTO;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Managers
{
    public class AuctionNetworkController : NetworkController
    {
        public AuctionNetworkController(WebClient client, EventChannelSO eventChannel) : base(client, eventChannel)
        {
        }
        public void PostItemToAuction(AuctionItemDTO auction)
        {
            var callbackEvent = NetworkEvents.PostAuctionItemCallbackEvent;
            _webClient.SendPostRequest(
                "auction/post",
                auction,
                success =>
                {
                    callbackEvent.success = success;
                    _networkChannel.InvokeEvent(callbackEvent);
                });
        }
        public void GetAuctionItemsByItemName(string itemName)
        {
            var callbackEvent = NetworkEvents.GetItemsByIdCallback;
            _webClient.SendGetRequest<List<AuctionItemDTO>>(
                $"auction/get-items?itemName={itemName}",
                results =>
                {
                    callbackEvent.results = results;
                    _networkChannel.InvokeEvent(callbackEvent);
                });
        }
        public void GetAuctionItemsByPlayerId()
        {
            var callbackEvent = NetworkEvents.GetitemsByPlayerIdCallback;
            _webClient.SendGetRequest<List<AuctionItemDTO>>(
                    $"auction/get-my-items",
                    result =>
                    {
                        callbackEvent.results = result;
                        _networkChannel.InvokeEvent(callbackEvent);
                    });
        }
        public void DeleteMyAuctionItem(string itemName,int pricePerUnit)
        {
            var callbackEvent = NetworkEvents.DeleteMyAuctionItemCallback;
            _webClient.SendDeleteRequest(
                $"auction/cancel?itemName={itemName}&pricePerUnit={pricePerUnit}",
                result =>
                {
                    callbackEvent.success = result;
                    _networkChannel.InvokeEvent(callbackEvent);
                });
        }
        public void Purchase(BuyerDTO info)
        {
            var callbackEvent = NetworkEvents.PurchaseAuctionItemCallbackEvent;
            _webClient.SendPatchRequest(
                $"auction/purchase",
                info,
                result =>
                {
                    callbackEvent.buyerInfo = info;
                    callbackEvent.success = result;
                    _networkChannel.InvokeEvent(callbackEvent);
                });
        }
    }
}
