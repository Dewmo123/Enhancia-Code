using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using _00.Work.CDH.Code.SkillSystem.Skills;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.CDH.Code.SkillSystem.Skills.Thunder
{
    public class Thunder : SkillModule
    {
        [SerializeField] private DamageCaster dc;

        public override void SetUp(DamageCaster.GetCalculatedDamageHandler getCalculatedDamageFunc, SkillDataSO skillSo, Entity owner, PoolManagerMono poolManager)
        {
            base.SetUp(getCalculatedDamageFunc, skillSo, owner, poolManager);
        }

        public void Attack()
        {
            dc.InitCaster(_owner);
            dc.ChangeMaxHitCount(_skillSo.attackTargetCount);
            dc.CastDamage(GetCalculatedDamage, _skillSo.enemyKnockbackForce, _skillSo.isPowerAttack);

            DOVirtual.DelayedCall(1f, () => _poolManager.Push(this));
        }
    }
}
