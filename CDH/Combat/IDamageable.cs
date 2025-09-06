using _00.Work.CDH.Code.Entities;
using UnityEngine;

namespace _00.Work.CDH.Code.Combat
{
    public interface IDamageable
    {
        public float GetMaxHealth();
        public void ApplyDamage(float damage, Vector2 direction, Vector2 knockBackPower, bool isPowerAttack, Entity dealer);
    }
}