using System;

namespace Scripts.InvenSystem
{
    [Serializable]//디버깅용 지워도됨
    public class InventoryItem : ICloneable
    {
        public ItemDataSO data;
        public int stackSize;

        public InventoryItem(ItemDataSO newItemData, int count = 1)
        {
            data = newItemData;
            stackSize = count;
        }
        public bool AddStack(int count)
        {
            if (count < 0)
                return false;
            stackSize += count;
            return true;
        }
        public bool RemoveStack(int count = 1)
        {
            stackSize -= count;
            return stackSize >= 0;
        }
        public bool UpdateStack(int quantity)
        {
            stackSize = quantity;
            return stackSize >= 0;
        }
        public void Clear()
        {
            data = null;
            stackSize = 0;
        }

        public object Clone()
        {
            return null;
            return new InventoryItem(data, stackSize);
        }
    }
}