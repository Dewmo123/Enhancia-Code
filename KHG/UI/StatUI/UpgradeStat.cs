using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStat : MonoBehaviour
{
    [SerializeField] private Image iconSprite;
    [SerializeField] private TextMeshProUGUI costTmp;
    [SerializeField] private TextMeshProUGUI goalValueTmp;
    /// <summary>
    /// 업그레이드 스탯 UI 설정
    /// </summary>
    /// <param name="icon">해당 스탯의 아이콘</param>
    /// <param name="cost">해당 스탯 업그레이드 가격</param>
    /// <param name="goal">현재 스탯과 다음스택 ex) 15->30</param>
    public void SetUI(Sprite icon, string cost, string goal)
    {
        iconSprite.sprite = icon;

        costTmp.text = cost;
        goalValueTmp.text = goal;
    }
}
