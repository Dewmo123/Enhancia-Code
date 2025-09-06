using ServerCode.DTO;
using System;
using System.Collections.Generic;

namespace Core.EventSystem
{
    public class NetworkEvents
    {
        public static readonly SignUpCallbackEvent SignUpCallback = new();
        public static readonly LogInCallbackEvent LogInCallback = new();

        public static readonly GetItemsByIdCallbackEvent GetItemsByIdCallback = new();
        public static readonly GetItemsByPlayerIdCallbackEvent GetitemsByPlayerIdCallback = new();
        public static readonly GetMyItemsCallbackEvent GetMyItemsCallback = new();
        public static readonly GetMyDataCallbackEvent GetMyDataCallback = new();

        public static readonly DeleteMyAuctionItemCallbackEvent DeleteMyAuctionItemCallback = new();
        public static readonly SendMyItemsCallbackEvent SendMyItemsCallback = new();
        public static readonly PostAuctionItemCallbackEvent PostAuctionItemCallbackEvent = new();
        public static readonly PurchaseAuctionItemCallbackEvent PurchaseAuctionItemCallbackEvent = new();
        public static readonly UpgradeDictionaryCallbackEvent UpgradeDictionaryCallbackEvent = new();
        public static readonly UpgradeEquipCallbackEvent UpgradeEquipCallbackEvent = new();
        public static readonly AddItemsCallbackEvent AddItemsCallbackEvent = new();

    }
    public class SignUpCallbackEvent : GameEvent
    {
        public bool success;
    }
    public class LogInCallbackEvent : GameEvent
    {
        public string playerId;
        public bool success;
    }
    public class GetItemsByIdCallbackEvent : GameEvent
    {
        public List<AuctionItemDTO> results;
    }
    public class GetItemsByPlayerIdCallbackEvent : GameEvent
    {
        public List<AuctionItemDTO> results;
    }
    public class DeleteMyAuctionItemCallbackEvent : GameEvent
    {
        public bool success;
    }
    public class GetMyItemsCallbackEvent : GameEvent
    {
        public List<PlayerItemDTO> results;
    }
    public class SendMyItemsCallbackEvent : GameEvent
    {
        public bool success;
    }
    public class PostAuctionItemCallbackEvent : GameEvent
    {
        public bool success;
    }
    public class PurchaseAuctionItemCallbackEvent : GameEvent
    {
        public BuyerDTO buyerInfo;
        public bool success;
    }
    public class UpgradeDictionaryCallbackEvent : GameEvent
    {
        public DictionaryUpgradeDTO upgradeDic;
        public int level;
        public bool success;
    }
    public class UpgradeEquipCallbackEvent : GameEvent
    {
        public EquipType equipType;
        public bool success;
    }
    public class GetMyDataCallbackEvent : GameEvent
    {
        public PlayerDataDTO result;
    }
    public class AddItemsCallbackEvent : GameEvent
    {
        public bool success;
    }
}
