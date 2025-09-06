using _00.Work.CDH.Code.Combat;
using _00.Work.CDH.Code.Entities;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace _00.Work.CIW._01.Scripts
{
    public class Burst : MonoBehaviour, IPoolable
    {
        //[SerializeField] PoolItemSO _poolItem;
        private Pool _myPool;
        // 애니메이션 재생해주기
        [SerializeField] Animator _animator;

        //[SerializeField] float _speed = 5f;
        [SerializeField] float _launchAngle = 45f;
        [SerializeField] float _gravity = 9.81f;

        [SerializeField] private Vector2 knockbackPower;
        [SerializeField] private bool powerAttack;

        float _dmg;
        bool _isBurst = false;

        Rigidbody2D _rigid;
        Entity _owner;

        [field : SerializeField] public PoolItemSO PoolItem { get; private set; }
        [field: SerializeField] public GameObject GameObject { get; private set; }

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
        }
        public void LaunchToTarget(Entity owner, Vector2 target, float  damage)
        {
            Debug.Log($"{target} 받아옴"); // 잘 받아와짐

            _owner = owner; // 소유자 받아옴
            _dmg = damage; // 데미지 받아옴

            if (_isBurst)
            {
                Debug.Log("isBurst is true");
                return;
            }

            Debug.Log($"_dmg: {_dmg}"); // 잘 받아옴

            Vector2 start = transform.position;
            Vector2 distance = target - start;

            Debug.DrawLine(start, target, Color.red, 1f);

            // 각도를 라디안으로 변환
            float radAngle = _launchAngle * Mathf.Deg2Rad;
            float distanceX = distance.x;
            float distanceY = distance.y;

            float cos = Mathf.Cos(radAngle);
            float sin = Mathf.Sin(radAngle);
            float tan = Mathf.Tan(radAngle);

            Debug.Log($"distanceX: {distanceX}, distanceY: {distanceY}");

            // distanceX의 부호에 따라 발사 각도 방향을 보정
            float direction = Mathf.Sign(distanceX);
            float absDistanceX = Mathf.Abs(distanceX);

            // 포물선 공식 적용 (distanceX는 항상 양수로 계산)
            float denominator = 2 * cos * cos * (absDistanceX * tan - distanceY);

            if (Mathf.Abs(denominator) < 0.001f || denominator <= 0f)
            {
                Debug.LogWarning("포물선 계산 실패 : denominator 0에 가까움. fallback 직선 발사.");
                Vector2 fallbackDir = distance.normalized;
                _rigid.linearVelocity = fallbackDir * 10f;
                return;
            }

            float veloSq = (_gravity * absDistanceX * absDistanceX) / denominator;

            if (veloSq <= 0 || float.IsNaN(veloSq))
            {
                Debug.LogWarning("포물선 계산 실패 : veloSq가 유효하지 않음. fallback 직선 발사");
                Vector2 fallbackDir = distance.normalized;
                _rigid.linearVelocity = fallbackDir * 10f;
                return;
            }

            float velocity = Mathf.Sqrt(veloSq);
            // x방향은 direction(좌/우)에 따라 부호를 곱해줌
            Vector2 launchVelocity = new Vector2(cos * velocity * direction, sin * velocity);

            _rigid.linearVelocity = launchVelocity;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                //_spitter.OnBurstEvent?.Invoke(collision.transform); // 스피터에서 이벤트 호출

                Vector2 direction = collision.transform.position - transform.position;
                Vector2 knockback = knockbackPower;
                knockback.x *= Mathf.Sign(direction.x); // 방향에 따라 knockback의 x값을 조정

                damageable.ApplyDamage(_dmg, direction, knockback, powerAttack, _owner);
            }

            _isBurst = true;
            _animator.Play("burst_Burst_2", 0);

            _myPool.Push(this); // 에러떠요 짜증나

            //_owner.DeadToPush(); // error
            //Destroy(this.gameObject);
        }

        public void SetUpPool(Pool pool)
        {
            _myPool = pool;
        }

        public void ResetItem()
        {
            _isBurst = false;
        }
    }
}

