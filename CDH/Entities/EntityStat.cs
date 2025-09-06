using _00.Work.CDH.Code.Core.StatSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _00.Work.CDH.Code.Entities
{
    public class EntityStat : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private StatOverride[] statOverrides;
        private StatSO[] _stats; // real stats

        private Entity _owner;

        public void Initialize(Entity entity)
        {
            _owner = entity;
            // 스탯들을 복제하고 오버라이드해 다시 저장
            _stats = statOverrides.Select(stat => stat.CreateStat()).ToArray();
        }
        public StatSO GetStat(StatSO targetStat)
            => GetStat(targetStat.statName);
        public StatSO GetStat(string statName)
            => _stats.FirstOrDefault(stat => EqualityComparer<string>.Default.Equals(stat.statName, statName));

        public bool TryGetStat(StatSO targetStat, out StatSO outStat)
            => outStat = GetStat(targetStat);
        public bool TryGetStat(string statName, out StatSO outStat)
            => outStat = GetStat(statName);

        public void SetBaseValue(StatSO stat, float value)
            => GetStat(stat).BaseValue = value;
        public float GetBaseValue(StatSO stat)
            => GetStat(stat).BaseValue;
        public void InscreaseBaseValue(StatSO stat, float value)
            => GetStat(stat).BaseValue += value;
        public void AddModifier(StatSO stat, object key, float value)
            => GetStat(stat).AddModifier(key, value);
        public void RemoveModifier(StatSO stat, object key)
            => GetStat(stat).RemoveModifier(key);

        public void ClearAllModifier()
        {
            foreach (StatSO stat in _stats)
            {
                stat.ClearAllModifier();
            }
        }
    }
}
