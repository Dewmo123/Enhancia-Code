using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;


namespace _00.Work.CSH._01.Scripts.Enemies
{

    public class EnemyProjectile : MonoBehaviour, IPoolable, IDamageable
    {
        public float _damage;
        public Transform _target;
        public Animator boom;
        private bool isboom = false;
        private PoolManagerMono _poolMono;

        [SerializeField] private LayerMask collisionTargetLayer;
        [SerializeField] public ObjectFinderSO poolMonoFinder;
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        [field: SerializeField] public GameObject GameObject { get; private set; }

        private void Awake()
        {
            _poolMono = poolMonoFinder.GetObject<PoolManagerMono>();
        }

        private void Update()
        {
            if (isboom) return;
            transform.position = Vector2.MoveTowards(transform.position, _target.position + new Vector3(0, 1, 0), 0.04f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((1 << collision.gameObject.layer & collisionTargetLayer) != 0)
            {
                if (collision.TryGetComponent(out IDamageable damageable))
                {
                    Vector2 direction = (collision.transform.position - transform.position).normalized;
                    Vector2 knockBack = direction * 5f; // 넉백 힘 설정 (원하면 0으로도 가능)

                    damageable.ApplyDamage(_damage, direction, knockBack, false, null); // isPowerAttack은 false, dealer는 없으니 null
                }
            }

            Dead();
        }

        private void Dead()
        {
            boom.Play("AttackBoom", 0);
            isboom = true;
            GoToPool();
        }

        private void GoToPool()
        {
            _poolMono.Push(this);
        }

        public void SetUpPool(Pool pool)
        {
        }

        public void ResetItem()
        {
            _target = null;
            isboom = false;
        }

        public float GetMaxHealth()
            => 0;

        public void ApplyDamage(float damage, Vector2 direction, Vector2 knockBackPower, bool isPowerAttack, Entity dealer)
        {
            boom.Play("AttackBoom", 0);
            isboom = true;
            GoToPool();
        }
    }
}
