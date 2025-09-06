using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour
{
    public bool IsAssigned { get; set; } = false;

    [SerializeField] private Image iconImg;
    [SerializeField] private TextMeshProUGUI nameTmp;
    [SerializeField] private Button button;

    private bool _haveCommand;
    public void SetUI(Sprite icon, string name)
    {
        IsAssigned = true;
        iconImg.sprite = icon;
        nameTmp.text = name;
        if (button)
        {
            button.onClick.RemoveAllListeners();//이거 되나?
            //button.onClick.AddListener(() =>
            //{
            //    command?.Invoke();
            //});
        }
    }

    public void SetButtonAction(Action command)
    {
        if (button)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                command?.Invoke();
                _haveCommand = true;
            });
        }
    }

    private void OnDestroy()
    {
        if(_haveCommand) button.onClick.RemoveAllListeners();
    }
}
