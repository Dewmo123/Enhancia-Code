using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using UnityEngine;

namespace AKH.Scripts.Combat
{
	public class Bullet : MonoBehaviour
	{
		[SerializeField] private float speed;
		[SerializeField] private float lifeTime = 5f;

		private LayerMask _whatIsObstruct;
		private float _damage;
		private Vector2 _knockbackPower;
		private bool _isPowerAttack;
		private Entity _owner;

		private float _lifeTimer;

		private void Awake()
		{
			GetComponent<Rigidbody2D>().linearVelocity = transform.right * speed;
		}

		private void Update()
		{
			if ((_lifeTimer = Mathf.Max(_lifeTimer - Time.deltaTime, 0)) <= 0)
			{
				Destroy(gameObject);
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if ((1 << collision.gameObject.layer & _whatIsObstruct) != 0)
			{
				if (collision.TryGetComponent(out IDamageable damageable))
				{
					damageable.ApplyDamage(_damage, transform.right, _knockbackPower, _isPowerAttack, _owner);
				}
				Destroy(gameObject);
			}
		}

		public void SetBullet(LayerMask obstructLayer, float damage, Vector2 knockbackPower, bool isPowerAttack, Entity owner)
		{
			_lifeTimer = lifeTime;

			_whatIsObstruct = obstructLayer;
			_damage = damage;
			_knockbackPower = knockbackPower;
			_isPowerAttack = isPowerAttack;
			_owner = owner;
		}
	}
}
