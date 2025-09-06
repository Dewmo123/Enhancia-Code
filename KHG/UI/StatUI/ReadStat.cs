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
        /// ����� ���� UI ����
        /// </summary>
        /// <param name="icon">�ش� ������ ������</param>
        /// <param name="value">�ش� ������ ���� ��</param>
        /// <param name="lv">�ش� ������ ���� ����</param>
        public void SetUI(Sprite icon, string value,string lv)
        {
            iconSprite.sprite = icon;
            curValue.text = value;
            curLevel.text = lv;
        }
    }
}
