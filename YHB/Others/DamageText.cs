using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using TMPro;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Others
{
    public class DamageText : TextObject, IPoolable
    {
        [Header("Pool Set")]
        [SerializeField] protected ObjectFinderSO poolMonoFinder;
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        [field: SerializeField] public GameObject GameObject { get; private set; }

        [Header("Text Set")]
        [SerializeField] private float originLifeTime;
        [SerializeField] private Vector2 moveDirection;
        [SerializeField] private float arrivedTimeSetValue;

        private Pool _myPool;

        private Vector2 _movement;
        private Vector3 _startPosition;

        private float _lifeTime;
        private bool _spawned;
        private float _timeCycleLerpValue;

        protected override void Awake()
        {
            base.Awake();
            _movement = moveDirection / originLifeTime;

            _timeCycleLerpValue = Mathf.PI / 2 / originLifeTime;
            _spawned = false;
        }

        public void SetText(string text, float size, Vector3 position, Color textColor)
        {
            _text.text = text;
            _startPosition = position;
            transform.position = _startPosition;

            _text.fontSize = size;
            _text.color = textColor;
        }

        private void Update()
        {
            if (!_spawned)
                return;

            if ((_lifeTime += Time.deltaTime) >= originLifeTime)
            {
                _spawned = false;
                Dead();
            }

            float moveValue = Mathf.Clamp01(arrivedTimeSetValue * Mathf.Sin(_lifeTime * _timeCycleLerpValue));
            transform.position = _startPosition + (Vector3)(moveValue * _movement);

            Color color = _text.color;
            float difference = Mathf.Cos(_lifeTime * _timeCycleLerpValue);
            color.a = difference;
            _text.color = color;
        }

        public void ResetItem()
        {
            _lifeTime = 0;
            _spawned = true;

            Color color = _text.color;
            color.a = 1;
            _text.color = color;
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool; 
        }

        private void Dead()
        {
            _myPool.Push(this);
        }
    }
}
