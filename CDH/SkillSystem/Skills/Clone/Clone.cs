using _00.Work.CDH.Code.Animators;
using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using Assets._00.Work.CDH.Code.SkillSystem;
using DewmoLib.ObjectPool.RunTime;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _00.Work.CDH.Code.Combat.DamageCaster;

namespace _00.Work.CDH.Code.SkillSystem.Skills.Clone
{
	public class Clone : SkillModule
	{
        [SerializeField] private SpriteRenderer spriteRenderer;
		[SerializeField] private Animator animator;
		[SerializeField] private DamageCaster damageCaster;
		[SerializeField] private AnimParamSO comboCounterParam;
		[SerializeField] private AnimParamSO isAttackParam;

		[SerializeField] private List<AttackDataSO> skillAttackDataList;

		[SerializeField] private float cloneDuration;
		[SerializeField] private float findEnemyRadius = 8f;

		private Transform _currentClosestEnemy;

		private int _comboCounter;
		private int _comboCount;
		private int _maxHitCnt;
		private EntityRenderer _entityRenderer;


        public void SetUp(
			GetCalculatedDamageHandler getCalculatedDamageFunc,
			SkillDataSO skillSo,
			Entity owner,
			PoolManagerMono poolManager,
            int maxComboCount,
            int maxHitCnt
            )
        {
            base.SetUp(getCalculatedDamageFunc, skillSo, owner, poolManager);

            _comboCounter = 0;
            _maxHitCnt = maxHitCnt;
            _comboCount = maxComboCount;

            animator.SetInteger(comboCounterParam.hashValue, _comboCounter);
            animator.SetBool(isAttackParam.hashValue, true);

            damageCaster.InitCaster(owner);

            if (_entityRenderer == null)
                _entityRenderer = _owner.GetCompo<EntityRenderer>();

            FacingToPlayerFacingDirection();
        }

        public override void ResetItem()
        {
            base.ResetItem();
            Color temp = spriteRenderer.color;
            temp.a = 1f;
            spriteRenderer.color = temp;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
			damageCaster.ChangeMaxHitCount(_maxHitCnt);
        }

        private void FacingToPlayerFacingDirection()
		{
			if (_entityRenderer.FacingDirection < 0)
				transform.Rotate(0, 180, 0);
		}

		private IEnumerator FadeAfterDelay(float skillCloneDuration)
		{
			yield return new WaitForSeconds(skillCloneDuration);
			spriteRenderer.DOFade(0, 0.7f).OnComplete(() => _poolManager.Push(this));
		}

		public void AttackTrigger()
		{
			animator.SetBool(isAttackParam.hashValue, true);
			AttackDataSO attackData = skillAttackDataList[_comboCounter];

			damageCaster.ChangeMaxHitCount(_maxHitCnt);
			damageCaster.CastDamage(
				enemyHealth => GetCalculatedDamage?.Invoke(enemyHealth) ?? 0,
				attackData.knockBackForce,
				attackData.isPowerAttack
				);
		}

		public void AnimationEnd()
		{
			_comboCounter++;
			if (_comboCounter < _comboCount)
			{
				animator.SetBool(isAttackParam.hashValue, false);
				animator.SetInteger(comboCounterParam.hashValue, _comboCounter);
				// FacingToClosestTarget(_skillCompo.FindClosestEnemy(transform.position, findEnemyRadius));
			}
			else
			{
				StartCoroutine(FadeAfterDelay(cloneDuration));
			}
		}

		public void SetAnimationSetting()
		{
			animator.SetBool(isAttackParam.hashValue, true);
		}

       
    }
}