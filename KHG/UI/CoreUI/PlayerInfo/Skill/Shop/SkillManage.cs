using _00.Work.CDH.Code.SkillSystem;
using Assets._00.Work.CDH.Code.SkillSystem.Skills;
using Core.EventSystem;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SkillManage : ChangeableUI
{
    [SerializeField] private EventChannelSO inStageEventChannel;
    [SerializeField] private SkillShop skillShop;
    [SerializeField] private SkillEquip skillEquip;

    private PlayerSkillCompo _playerSkillCompo;
    private Dictionary<Type, SkillItemSO> _skillDict;

    private void Awake()
    {
        if (InStageEvent.ProclaimSkillCompEvent.isInited)
            HandleProclaimSkillComptEvent(InStageEvent.ProclaimSkillCompEvent);
        else
            inStageEventChannel.AddListener<ProclaimSkillCompEvent>(HandleProclaimSkillComptEvent);
    }

    private void OnDestroy()
    {
        inStageEventChannel.RemoveListener<ProclaimSkillCompEvent>(HandleProclaimSkillComptEvent);
    }

    private void HandleProclaimSkillComptEvent(ProclaimSkillCompEvent @event)
    {
        _playerSkillCompo = @event.skillComponent;

        _skillDict = new Dictionary<Type, SkillItemSO>();
        foreach (SkillItemSO skillItem in @event.skillItemList)
            _skillDict.Add(skillItem.SkillType, skillItem);
    }

    [ContextMenu("Active")]
    public void ActiveUI()
    {
        if (_playerSkillCompo == null)
            return;

        Skill[] equipedSkill = _playerSkillCompo.CurrentActiveSkills;

        skillEquip.Initialize();
        for (int i = 0; i < equipedSkill.Length; ++i)
        {
            Type skillType = equipedSkill[i]?.GetType();
            SetSkillItemUI(i, skillType != null ? _skillDict[skillType] : null);
        }
    }

    public void SetSkillItemUI(int index, SkillItemSO skill = null)
    {
        skillEquip.SetSkillItemUI(index, skill);
    }

    protected override void OnShow()
    {
        skillShop.gameObject.SetActive(true);
    }

    protected override void OnHide()
    {
        skillShop.gameObject.SetActive(false);
        skillEquip.SetUiOn(false);
    }
}
