using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LibraryProfileUI : MonoBehaviour
{
    [SerializeField] private Sprite defaultSpirte;
    [SerializeField] private Image profileImg;
    [SerializeField] private TextMeshProUGUI titleTmp, subtitleTmp, amoutTmp, lvTmp;
    [SerializeField] private Button upgradeButton;

    private bool canUpgrade;

    private void OnEnable()
    {
        DefaultSetting();
    }
    public void SetLibraryProfile(Sprite sprite, string title, string subtitle, int lv, int currentVal, int targetVal, Action buttonAction)
    {
        profileImg.sprite = sprite;
        titleTmp.text = title;
        subtitleTmp.text = subtitle;

        lvTmp.text = $"Lv.{lv} -> Lv.{lv + 1}";
        amoutTmp.text = $"{currentVal}/{targetVal}";
        CheckUpgradeable(currentVal, targetVal);
        upgradeButton.onClick.RemoveAllListeners();
        upgradeButton.onClick.AddListener(() => 
        {
            buttonAction?.Invoke();
            });
    }

    private void DefaultSetting()
    {
        profileImg.sprite = defaultSpirte;
        titleTmp.text = "----";
        subtitleTmp.text = "----";

        lvTmp.text = $":)";
        amoutTmp.text = "00/00";
        upgradeButton.enabled = false;
    }
    private void CheckUpgradeable(int currentVal, int targetVal)
    {
        bool value = currentVal >= targetVal;
        upgradeButton.enabled = value;
        canUpgrade = value;
    }

    private Action SetButtonAction(Action buttonAction)
    {
        if (canUpgrade)
            return buttonAction;
        else
            return ()=>print("you need enough amount!");//대충 경고띄워도 될듯하다
    }
}
