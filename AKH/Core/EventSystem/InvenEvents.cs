using Scripts.Combat;
using Scripts.InvenSystem;
using System.Collections.Generic;

namespace Core.EventSystem
{
    public class InvenEvents
    {
        public static readonly RequestPlayerDataEvent RequestDictionaryEvent = new();
        public static readonly RequestInventoryEvent RequestInventoryEvent = new();
        public static readonly UpdatePlayerDataEvent SendPlayerDataEvent = new();
        public static readonly UpdateInventoryEvent updateInventoryEvent= new();
        public static readonly RequestAddItemsEvent RequestAddItemsEvent = new();
    }
    public class RequestPlayerDataEvent : GameEvent { };
    public class RequestInventoryEvent : GameEvent { };
    public class RequestAddItemsEvent : GameEvent 
    {
        public Queue<DropInfo> dropInfos;
    };
    public class UpdatePlayerDataEvent : GameEvent
    {
        public Dictionary<string, int> dictionary;
        public int gold;
    }
    public class UpdateInventoryEvent : GameEvent
    {
        public List<InventoryItem> inven;
    }
}
