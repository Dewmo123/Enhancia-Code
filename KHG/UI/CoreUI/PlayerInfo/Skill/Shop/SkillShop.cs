using _00.Work.CDH.Code.SkillSystem;
using Assets._00.Work.CDH.Code.SkillSystem.Skills;
using Core.EventSystem;
using Library.Normal;
using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillShop : MonoBehaviour
{
    [SerializeField] private List<SkillItemSO> skillItemSOs;
    [SerializeField] private SkillEquip skillEquip;
    [SerializeField] private Transform productParent;

    [SerializeField] private List<Animator> btnAnimators;

    [SerializeField] private EventChannelSO uiChannel;
    [SerializeField] private EventChannelSO inStageEventChannel;
    private PlayerSkillCompo _skillComp;
    private UniqueRandomPool<SkillItemSO> uniqueRandomPool;
    private List<SkillProduct> skills = new();

    private void Awake()
    {
        GetSkillUIs();

        // √ ±‚»≠
        uniqueRandomPool = new UniqueRandomPool<SkillItemSO>();
        uniqueRandomPool.AddRange(skillItemSOs);

        if (InStageEvent.ProclaimSkillCompEvent.isInited)
            HandleProclaimSkillComptEvent(InStageEvent.ProclaimSkillCompEvent);
        else
            inStageEventChannel.AddListener<ProclaimSkillCompEvent>(HandleProclaimSkillComptEvent);
    }

    private void OnDestroy()
    {
        inStageEventChannel.RemoveListener<ProclaimSkillCompEvent>(HandleProclaimSkillComptEvent);
    }

    private void OnEnable()
    {
        GetRandomSkills();
        ResetAnimators();
    }
    private void ResetAnimators()
    {
        foreach (Animator animator in btnAnimators)
        {
            animator.Rebind();
            animator.Update(0f);
            animator.Play("Normal", 0, 0f);
        }
    }

    private void HandleProclaimSkillComptEvent(ProclaimSkillCompEvent @event)
    {
        _skillComp = @event.skillComponent;
    }

    private void GetRandomSkills()
    {
        for (int i = 0; i < skills.Count; ++i)
        {
            SkillItemSO randomSkill = uniqueRandomPool.GetValue();

            bool isDuplicate = false;
            for (int j = 0; j < _skillComp.CurrentActiveSkills.Length; ++j)
                if (EqualityComparer<Type>.Default.Equals(randomSkill?.GetType(), _skillComp.CurrentActiveSkills[j]?.GetType()))
                    isDuplicate = true;

            if (isDuplicate)
            {
                --i;
                continue;
            }

            SetSkillUI(skills[i], randomSkill, skillEquip);
        }
        uniqueRandomPool.Reset();
    }

    private void GetSkillUIs()
    {
        foreach (var skillUI in productParent)
        {
            if (skillUI is Transform skillTransform)
            {
                SkillProduct skillProduct = skillTransform.GetComponent<SkillProduct>();
                if (skillProduct != null)
                {
                    skills.Add(skillProduct);
                }
            }
        }
    }
    private void SetSkillUI(SkillProduct targetProduct, SkillItemSO skill, SkillEquip equiper)
    {
        targetProduct.SetSkillInfo(skill, equiper, () => gameObject.SetActive(false));
    }
}