using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using _00.Work.CDH.Code.SkillSystem;
using AKH.Scripts.Combat;
using UnityEngine;

namespace AKH.Scripts.Players.Skills
{
	public class DashFireSkill : Skill
	{
		[SerializeField] private GameObject bulletPrefab;
		[SerializeField] private StatSO damageStatSO;
		[SerializeField] private float shootInterval;

        //private StatSO damageStat;
		private Player _player;

		public override void InitializeSkill(Entity entity)
		{
			base.InitializeSkill(entity);
			_player = entity as Player;
			damageStat = entity.GetCompo<EntityStat>().GetStat(damageStatSO);
		}

		protected override void UseSkill()
		{
			int count = SkillData.AttackCount;
			_player.ChangeState(SkillData.skillState.animParam.parameterName);
			float center = _player.transform.rotation.z;
			if (count % 2 == 1)
			{
				InitBullet(0);
				count--;
			}
			for (int i = 1; i <= count; i++)
			{
				if (i % 2 == 0)
					InitBullet(center - shootInterval * (i / 2));
				else
					InitBullet(center + shootInterval * (i - 1));
			}
		}

		private void InitBullet(float rotZ)
		{
			float rotY = _player.transform.rotation.y * 180;
			Quaternion rotation = Quaternion.Euler(0, rotY, rotZ);
			GameObject gameObject = Instantiate(bulletPrefab, _player.transform.position, rotation);
			Bullet bullet = gameObject.GetComponent<Bullet>();
			bullet.SetBullet(
				SkillData.canCollisionTargetLayer,
				CalculateDamage(damageStat.Value),
				SkillData.enemyKnockbackForce,
				SkillData.isPowerAttack,
				_entity
				);
		}

		private void OnValidate()
		{
			if (bulletPrefab == null || !bulletPrefab.TryGetComponent(out Bullet b))
				bulletPrefab = null;
		}
	}
}
