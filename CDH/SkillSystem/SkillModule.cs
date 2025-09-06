using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.SkillSystem.Skills;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;
using static _00.Work.CDH.Code.Combat.DamageCaster;

namespace Assets._00.Work.CDH.Code.SkillSystem
{
    public abstract class SkillModule : MonoBehaviour, IPoolable
    {
        [field: SerializeField] public GameObject GameObject { get; private set; }
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }

        protected SkillDataSO _skillSo;
        protected Rigidbody2D _rigid;
        protected Entity _owner;
        protected PoolManagerMono _poolManager;

        protected GetCalculatedDamageHandler GetCalculatedDamage;

        public virtual void SetUpPool(Pool pool)
        {
        }

        public virtual void ResetItem()
        {
        }

        public virtual void SetUp(
            GetCalculatedDamageHandler getCalculatedDamageFunc,
            SkillDataSO skillSo,
            Entity owner,
            PoolManagerMono poolManager
            )
        {
            GetCalculatedDamage = getCalculatedDamageFunc;
            _skillSo = skillSo;
            _rigid = GetComponent<Rigidbody2D>();
            _owner = owner;
            _poolManager = poolManager;
        }
    }
}
