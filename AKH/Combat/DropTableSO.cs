using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Combat
{

    [Serializable]
    public struct DropRateInfo
    {
        public int minCnt, maxCnt;
        [Range(0, 1)]
        public float probability;
        public ItemDataSO item;
    }
    public struct DropInfo
    {
        public ItemDataSO item;
        public int quantity;
    }
    [CreateAssetMenu(fileName = "ItemDropTableSO", menuName = "SO/Items/DropTable")]
    public class DropTableSO : ScriptableObject
    {
        public List<DropRateInfo> table;
        
        public Queue<DropInfo> PullUpItem()
        {
            Queue<DropInfo> dropInfos = new();
            foreach (var info in table)
            {
                int quantity = info.minCnt;
                for (int i = 0; i < info.maxCnt - info.minCnt; i++)
                    if (UnityEngine.Random.value < info.probability)
                        quantity++;
                    else
                        break;
                if (quantity > 0)
                    dropInfos.Enqueue(new DropInfo() { item = info.item, quantity = quantity});
            }
            return dropInfos;
        }
    }
}
