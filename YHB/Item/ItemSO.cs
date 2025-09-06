using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Item
{
    /// <summary>
    /// 아이템의 ID, 아이콘, 이름을 가지는 SO입니다.
    /// </summary>
    /// <remarks>
    /// 자체적으로 현재 수량을 지니고있습니다.
    /// </remarks>
    public abstract class ItemSO : ScriptableObject
    {
        [field: SerializeField] public int ItemID { get; private set; }

        public Sprite itemIcon;
        public string itemName;
        [TextArea]
        [SerializeField] protected string description;

        protected int _currentCount;
        protected int _maxCount;

        /// <summary>
        /// 아이템 갯수를 0과 최대 값 사이로 설정합니다.
        /// </summary>
        /// <returns>
        /// 설정된 값이 최대 값 이상이거나 0이하라면 false를 반환합니다.
        /// </returns>
        public bool SetItemCount(int count)
            => (_currentCount = Mathf.Clamp(count, 0, _maxCount)) < _maxCount || _currentCount >= 0;
        public bool IsMaxCount()
            => _currentCount <= _maxCount;
        public int GetCurrentCount()
            => _currentCount;
    }
}
