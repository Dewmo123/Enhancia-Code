using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KHG.UI.Stat
{
    public class ReadStat : MonoBehaviour
    {
        [SerializeField] private Image iconSprite;
        [SerializeField] private TextMeshProUGUI curValue;
        [SerializeField] private TextMeshProUGUI curLevel;
        /// <summary>
        /// 보기용 스탯 UI 설정
        /// </summary>
        /// <param name="icon">해당 스탯의 아이콘</param>
        /// <param name="value">해당 스탯의 현재 값</param>
        /// <param name="lv">해당 스탯의 현재 레벨</param>
        public void SetUI(Sprite icon, string value,string lv)
        {
            iconSprite.sprite = icon;
            curValue.text = value;
            curLevel.text = lv;
        }
    }
}
