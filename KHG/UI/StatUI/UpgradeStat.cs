using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStat : MonoBehaviour
{
    [SerializeField] private Image iconSprite;
    [SerializeField] private TextMeshProUGUI costTmp;
    [SerializeField] private TextMeshProUGUI goalValueTmp;
    /// <summary>
    /// ���׷��̵� ���� UI ����
    /// </summary>
    /// <param name="icon">�ش� ������ ������</param>
    /// <param name="cost">�ش� ���� ���׷��̵� ����</param>
    /// <param name="goal">���� ���Ȱ� �������� ex) 15->30</param>
    public void SetUI(Sprite icon, string cost, string goal)
    {
        iconSprite.sprite = icon;

        costTmp.text = cost;
        goalValueTmp.text = goal;
    }
}
