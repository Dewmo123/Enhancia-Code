using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using _00.Work.CIW._01.Scripts;
using _00.Work.CIW._01.Scripts.Enemy;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace Assets._00.Work.CIW._01.Scripts.Enemy
{
    public class SptitterAttackComponent : MonoBehaviour, IEntityComponent
    {
        [SerializeField] EntityStat _dmgStat;
        [SerializeField] StatSO _dmgStatSO;
        [SerializeField] ObjectFinderSO finder;
        [SerializeField] PoolItemSO burstPoolItem;

        PoolManagerMono _poolManager;

        Spitter _spitter;
        Burst _burst;

        float _dmg;

        public void Initialize(Entity entity)
        {
            _dmg = _dmgStat.GetStat(_dmgStatSO).BaseValue;
            _spitter = entity.GetComponent<Spitter>();
            finder.GetObject(out _poolManager);

            _spitter.OnBurstEvent += BurstAttack;
        }

        public void BurstAttack(Transform target)
        {
            Burst burst = _poolManager.Pop<Burst>(burstPoolItem);
            burst.transform.position = transform.position;

            Debug.Assert(burst != null, $"can't pop burst");
            burst.LaunchToTarget(_spitter, target.transform.position, _dmg);

            //_burst.LaunchToTarget(dir, dmg); // error

            //Vector2 knockBack = dir * 5f;

            //if (target.TryGetComponent<IDamageable>(out var damageable))
            //{
            //    damageable.ApplyDamage(dmg, dir, knockBack, false, null);
            //}
        }

        private void OnDisable()
        {
            if (_spitter != null)
                _spitter.OnBurstEvent -= BurstAttack;
            else
                Debug.LogWarning($"스피터 왜 에러나냐 아안엏램ㅈㄱ햐ㅐㅏ덤게하 {gameObject.name}");
        }
    }
}
