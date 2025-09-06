using Assets._00.Work.CDH.Code.SkillSystem.Skills;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillProduct : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Button button;

    public void SetSkillInfo(SkillItemSO skill,SkillEquip equip,Action selectedEvent)
    {
        this.icon.sprite = skill.icon;
        this.nameText.text = skill.skillName;
        this.button.onClick.RemoveAllListeners();
        this.button.onClick.AddListener(() =>
        {
            equip.ChangeSkillSelected(skill);
            selectedEvent?.Invoke();
        });
    }

    private void OnDestroy()
    {
        this.button.onClick.RemoveAllListeners();
    }
}
