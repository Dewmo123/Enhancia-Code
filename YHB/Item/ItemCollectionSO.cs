using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Item
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// <see cref="ItemSO"/> 참조 바람
    /// </remarks>
    public class ItemCollectionSO : ItemSO
    {
        private const int UPGRADE_INITIAL_DEMAND_VALUE = 5;
        private const float UPGRADE_RATIO_DEMAND_VALUE = 1.5f;

        public int CollectionLevel { get; private set; }
        public int CurrentUpgradeDemandCount
        {
            get
                => Mathf.RoundToInt(UPGRADE_INITIAL_DEMAND_VALUE * Mathf.Pow(UPGRADE_RATIO_DEMAND_VALUE, CollectionLevel - 1)); // a1 * r^(n-1)
        }

        /// <summary>
        /// 들어온 값이 만약 현재 레벨보다 작으면 값이 설정 되지 않음
        /// </summary>
        public void SetCollectionLevel(int value)
            => CollectionLevel = Mathf.Max(value, CollectionLevel);

        /// <summary>
        /// 만약 레벨업을 시도하고 성공하면 true를 반환합니다.
        /// </summary>
        public bool TryCollectionUpgrade()
        {
            int upgradeDemandCount = CurrentUpgradeDemandCount;
            bool tryUpgrade = _currentCount >= upgradeDemandCount;

            if (tryUpgrade)
            {
                CollectionLevel++;
                _currentCount -= upgradeDemandCount;
            }

            return tryUpgrade;
        }
    }
}
