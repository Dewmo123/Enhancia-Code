using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultItemUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI countTmp;
    public void SetData(ItemDataSO data,int count)
    {
        icon.sprite = data.Icon;
        countTmp.text = $"x{count}";
    }
}
