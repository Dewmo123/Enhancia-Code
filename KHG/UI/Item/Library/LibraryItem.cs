using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LibraryItem : MonoBehaviour
{
    [SerializeField] private Image img;
    [SerializeField] private TextMeshProUGUI nameTMP;
    [SerializeField] private TextMeshProUGUI lvTMP;
    [SerializeField] private Outline ableBg;

    private Button _button;

    //�߰�: �ܺ� ���ٿ� Ű�� ����
    public string ItemKey { get; private set; }
    public int Level { get; private set; }

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    public void SetItemUI(Sprite sprite, string name, int level, bool canUpgrade)
    {
        img.sprite = sprite;
        nameTMP.text = name;
        lvTMP.text = $"Lv.{level}";
        ableBg.enabled = canUpgrade;

        ItemKey = name; // ItemKey = item.name ��ġ
        Level = level;
    }

    public void SetButtonEvent(Action action)
    {
        //_button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => action?.Invoke());
    }
}
