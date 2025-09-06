using System;
using System.Collections.Generic;
using System.Linq;
using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.SkillSystem;
using UnityEngine;
using UnityEngine.Events;

namespace _00.Work.CDH.Code.Entities
{
    public abstract class Entity : MonoBehaviour, IDamageable
    {
        public delegate void OnDamageHandler(float damage, Vector2 direction, Vector2 knockBackPower, bool isPowerAttack, Entity dealer);
        public event OnDamageHandler OnDamage;

        public delegate float GetMaxHealthHandler();
        public event GetMaxHealthHandler GetMaxHealthValue;

        public UnityEvent OnHit;
        public UnityEvent OnDead;
        public Action OnEntityResetEvent;
        public bool isDead { get; set; }
        
        protected Dictionary<Type, IEntityComponent> _components;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IEntityComponent>();
            AddComponentToDictionary();
            ComponentInitialize();
            AfterInitialize();
        }
        protected virtual void AddComponentToDictionary()
        {
            GetComponentsInChildren<IEntityComponent>(true).ToList().ForEach(compo => _components.Add(compo.GetType(), compo));
        }
        protected virtual void ComponentInitialize()
        {
            _components.Values.ToList().ForEach(compo => compo.Initialize(this));
        }
        protected virtual void AfterInitialize()
        {
            _components.Values.OfType<IAfterInit>().ToList().ForEach(compo => compo.AfterInit());
            OnHit.AddListener(HandleHit);
            OnDead.AddListener(HandleDead);
        }

        protected virtual void OnDestroy()
        {
            OnHit.RemoveListener(HandleHit);
            OnDead.RemoveListener(HandleDead);
        }

        protected abstract void HandleHit();
        protected abstract void HandleDead();

        public T GetCompo<T>(bool isDerived = false) where T : IEntityComponent
        {
            if (_components.TryGetValue(typeof(T), out IEntityComponent component))
                return (T)component;
            if(isDerived == false) return default;

            Type findType = _components.Keys.FirstOrDefault(type => type.IsSubclassOf(typeof(T)));
            if(findType != null)
                return (T)_components[findType];

            return default;
        }

        public void ApplyDamage(float damage, Vector2 direction, Vector2 knockBackPower, bool isPowerAttack, Entity dealer)
            => OnDamage?.Invoke(damage, direction, knockBackPower, isPowerAttack, dealer);

        public float GetMaxHealth()
            => GetMaxHealthValue?.Invoke() ?? 0;
    }
}
