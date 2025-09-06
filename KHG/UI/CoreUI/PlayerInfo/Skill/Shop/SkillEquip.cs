using Assets._00.Work.CDH.Code.SkillSystem.Skills;
using Core.EventSystem;
using KHG.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillEquip : MonoBehaviour
{
    [SerializeField] private EventChannelSO inStageEventChannel;
    [SerializeField] private GameObject EquipUI;
    [SerializeField] private SkillItem selectedSkillUi;
    [SerializeField] private SkillItem changerSkillUi;
    [SerializeField] private Button skillApplyButton;

    [SerializeField] private EventChannelSO uiChannel;
    [SerializeField] private Transform itemGrider;
    [SerializeField] private SkillManage skillMan;
    [Header("ArgumentExeption")]
    [SerializeField] private Sprite nullImg;
    [SerializeField] private string nullName;

    private List<SkillItem> _itemUI;
    private SkillItemSO _selectedActiveSkill;
    private SkillItemSO _selectedTargetSkill;

    private int _selectedItem;

    private void Awake()
    {
        InitItemUIs();
    }
    public void Initialize()
    {
        _selectedItem = -1;
        _selectedActiveSkill = null;
    }

    public void ChangeSkillSelected(SkillItemSO skill)
    {
        SetUiOn();
        _selectedActiveSkill = skill;
        SetStaticUI(skill);
    }

    public void SetSkillItemUI(int index, SkillItemSO skill = null)
    {
        SkillItem skillItem = _itemUI[index];
        if (skill == null)
            skillItem.SetUI(null, "비어있음");
        else
            skillItem.SetUI(skill.icon, skill.skillName);

        skillItem.SetButtonAction(() =>
        {
            SetChangerUI(skill);

            _selectedActiveSkill = skill;
            int savedIndex = index;
            _selectedItem = savedIndex;

            skillItem.IsAssigned = true;
            skillApplyButton.interactable = true;
        });
        
    }

    public void SkillApply()
    {
        var evt = UIEvents.SetPanelevent;
        evt.isActive = false;
        evt.ui = KHG.UI.UIType.SkillManage;
        uiChannel.InvokeEvent(evt); 
        inStageEventChannel.InvokeEvent(InStageEvent.RequestSkillChangeEvent.Initialize(_selectedItem, _selectedTargetSkill));
    }

    private void CheckNullItem()
    {
        foreach (var item in _itemUI)
        {
            if (item.IsAssigned == false)
            {
                print("할당되지 않음:" + item);
                item.SetUI(nullImg, nullName);
                item.SetButtonAction(null);
            }
        }
    }

    public void SetUiOn(bool val = true)
    {
        if(val) skillMan.ActiveUI();
        EquipUI.SetActive(val);
    }

    private void SetChangerUI(SkillItemSO skill)
    {
        changerSkillUi.SetUI(skill != null ? skill.icon : nullImg,
            skill != null ? skill.skillName : nullName);
    }

    private void SetStaticUI(SkillItemSO skill)
    {
        _selectedTargetSkill = skill;
        selectedSkillUi.SetUI(_selectedActiveSkill.icon, _selectedActiveSkill.skillName);
    }

    private void InitItemUIs()
    {
        _itemUI = new List<SkillItem>();

        foreach (RectTransform skillTransform in itemGrider)
        {
            if (skillTransform.TryGetComponent(out SkillItem skillItem))
            {
                skillItem.IsAssigned = false;
                _itemUI.Add(skillItem);
            }
        }
        CheckNullItem();
    }
}
