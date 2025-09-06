using ServerCode.DTO;
using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemUISO", menuName = "SO/UI/ItemUISO")]
public class ItemDataSO : ScriptableObject,ICloneable
{
    public Sprite Icon;
    public string Name;
    [TextArea]
    public string Description;

    public object Clone()
    {
        var item = CreateInstance<ItemDataSO>();
        item.Name = Name;
        item.Icon = Icon;
        item.Description = Description;
        return item;
    }
}
