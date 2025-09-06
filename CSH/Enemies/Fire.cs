using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CDH.Code.Entities;
using Assets._00.Work.YHB.Scripts.Core;
using UnityEngine;

namespace _00.Work.CSH._01.Scripts.Enemies
{

    public class Fire : MonoBehaviour
    {
        [SerializeField] private GameObject bullet;
        [SerializeField] private ObjectFinderSO finder;
        [SerializeField] private EntityStat stat;
        [SerializeField] private StatSO damageStat;
        private EntityAnimationTrigger _triggerCompo;

        private void Start()
        {
            _triggerCompo = transform.parent.parent.GetComponentInChildren<EntityAnimationTrigger>();
            
            _triggerCompo.OnAttackTrigger += FireBullet;
        }

        private void OnDestroy()
        {
            _triggerCompo.OnAttackTrigger -= FireBullet;
        }

        public void FireBullet()
        {
            EnemyProjectile projectile = Instantiate(bullet, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
            projectile._damage = stat.GetStat(damageStat).Value;
            projectile._target = finder.Object.transform;
        }
    }
}
