using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using Assets._00.Work.YHB.Scripts.Core;
using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image hpBar, barFollower;
    [SerializeField] private ObjectFinderSO playerFinder;
    private EntityHealth _playerHealth;
    private void Start()
    {
        _playerHealth = playerFinder.GetObject<Player>().GetCompo<EntityHealth>();
        _playerHealth.OnDamaged.AddListener(HandleHpChanged);
    }

    private void HandleHpChanged(float arg0)
    {
        HpDecrease(_playerHealth.HpPercent);
    }
    private void HpDecrease(float percent)
    {
        hpBar.fillAmount = percent;
        barFollower.DOFillAmount(percent, 1f);
    }
}
