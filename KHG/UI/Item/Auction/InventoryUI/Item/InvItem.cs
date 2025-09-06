using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNameTMP;
    [SerializeField] private TextMeshProUGUI itemQuantityTMP;
    [SerializeField] private TextMeshProUGUI itemPriceTMP;
    [SerializeField] private Button button;

    public void SetInvItem(Sprite image,string name,int quantity,int price = 0)
    {
        itemImage.sprite = image;
        itemNameTMP.text = name;
        itemQuantityTMP.text = quantity.ToString();
        if(itemPriceTMP != null && price != 0)
            itemPriceTMP.text = price.ToString();
        else if(itemPriceTMP != null)
            itemPriceTMP.text = string.Empty;
    }

    public void SetButton(Action action)
    {
        button.onClick.AddListener(() => action());
    }
}
