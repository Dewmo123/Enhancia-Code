using _00.Work.CDH.Code.SkillSystem;
using Assets._00.Work.CDH.Code.SkillSystem.Skills;
using Core.EventSystem;
using System;
using System.Collections.Generic;
using UnityEngine;
using static _00.Work.CDH.Code.SkillSystem.Skill;

public class SkillUiManager : MonoBehaviour
{
    [SerializeField] private EventChannelSO inStageEventChannel;
    [SerializeField] private Sprite skillNullIcon;
    [SerializeField] private List<Skill_Icon> _skillIcons;

    private PlayerSkillCompo _skillComp;

    private List<Action<int>> ChangeSkillChargeCountActionList;
    private List<CooldownInfo> CooldownDelegateList;
    private Dictionary<Type, SkillItemSO> _skillDict;
    private Skill[] _skills;

    private void Awake()
    {
        // 더 이쁘게 만들기엔 시간이 부족
        if (InStageEvent.ProclaimSkillCompEvent.isInited)
            HandleProclaimSkillComptEvent(InStageEvent.ProclaimSkillCompEvent);
        else
            inStageEventChannel.AddListener<ProclaimSkillCompEvent>(HandleProclaimSkillComptEvent);
        inStageEventChannel.AddListener<SkillChangeEvent>(HandleSkillChangeEvent);
    }

    private void OnDestroy()
    {
        inStageEventChannel.RemoveListener<ProclaimSkillCompEvent>(HandleProclaimSkillComptEvent);
        inStageEventChannel.RemoveListener<SkillChangeEvent>(HandleSkillChangeEvent);

        if (_skills != null)
            for (int index = 0; index < _skills.Length; ++index)
                DeSubscriptionSkillEvents(index);
    }

    private void HandleProclaimSkillComptEvent(ProclaimSkillCompEvent @event)
    {
        // 더 이쁘게 만들기엔 시간이 부족
        _skills = new Skill[PlayerSkillCompo.MAX_ACTIVE_SKILL_COUNT];
        _skillComp = @event.skillComponent;

        ChangeSkillChargeCountActionList = new List<Action<int>>();
        CooldownDelegateList = new List<CooldownInfo>();

        // 저장 후 구독, 해지를 위함
        for (int index = 0; index < _skills.Length; ++index)
        {
            int savedIndex = index;
            ChangeSkillChargeCountActionList.Add((count) => SetSkillChargeCount(savedIndex, count));
            CooldownDelegateList.Add((current, total) => SetSkillCoolTime(savedIndex, current, total));
        }

        _skillDict = new Dictionary<Type, SkillItemSO>();
        foreach (SkillItemSO skillItem in @event.skillItemList)
            _skillDict.Add(skillItem.SkillType, skillItem);
    }

    private void HandleSkillChangeEvent(SkillChangeEvent @event)
    {
        SetSkill(@event.skillIndex, @event.skillItemSO);
    }

    private void OnEnable()
    {
        if (_skillComp == null)
            return;

        for (int i = 0; i < _skillComp.CurrentActiveSkills.Length; ++i)
        {
            Type skillType = _skillComp.CurrentActiveSkills[i]?.GetType();
            SetSkill(i, skillType != null ? _skillDict[skillType] : null);
        }
    }

    private void SetSkill(int index, SkillItemSO skillItem)
    {
        Debug.Assert(0 <= index && index < _skills.Length, $"index is out of range. skill item : {(skillItem != null ? skillItem.name : "Null")}");

        DeSubscriptionSkillEvents(index);

        if (skillItem != null)
        {
            // 스킬 변경
            _skills[index] = _skillComp.GetSkill(skillItem.SkillType);
            SubscriptionSkillEvents(index);
        }

        SetSkillIcon(index, skillItem != null ? skillItem.icon : skillNullIcon);
    }

    private void SubscriptionSkillEvents(int index)
    {
        if (index < 0 || index >= _skillIcons.Count || _skills[index] == null)
            return;

        _skills[index].OnChangeSkillChargeCount += ChangeSkillChargeCountActionList[index];
        _skills[index].OnCooldown += CooldownDelegateList[index];
    }


    private void DeSubscriptionSkillEvents(int index)
    {
        if (index < 0 || index >= _skillIcons.Count || _skills[index] == null)
            return;

        _skills[index].OnChangeSkillChargeCount -= ChangeSkillChargeCountActionList[index];
        _skills[index].OnCooldown -= CooldownDelegateList[index];
    }

    public void SetSkillCoolTime(int index, float currentTime, float totalTime)
    {
        if (index < 0 || index >= _skillIcons.Count)
            return;
        _skillIcons[index].SetCoolTime(totalTime - currentTime, totalTime);
    }

    public void SetSkillIcon(int index, Sprite icon)
    {
        if (index < 0 || index >= _skillIcons.Count)
            return;
        _skillIcons[index].SetIcon(icon);
        _skillIcons[index].SetCoolTime(0, 0);
    }

    public void SetSkillChargeCount(int index, int count)
    {
        if (index < 0 || index >= _skillIcons.Count) return;
        _skillIcons[index].SetSkillCharge(count);
    }
}
