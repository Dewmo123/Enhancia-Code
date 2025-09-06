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
        // �ִϸ��̼� ������ֱ�
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
            Debug.Log($"{target} �޾ƿ�"); // �� �޾ƿ���

            _owner = owner; // ������ �޾ƿ�
            _dmg = damage; // ������ �޾ƿ�

            if (_isBurst)
            {
                Debug.Log("isBurst is true");
                return;
            }

            Debug.Log($"_dmg: {_dmg}"); // �� �޾ƿ�

            Vector2 start = transform.position;
            Vector2 distance = target - start;

            Debug.DrawLine(start, target, Color.red, 1f);

            // ������ �������� ��ȯ
            float radAngle = _launchAngle * Mathf.Deg2Rad;
            float distanceX = distance.x;
            float distanceY = distance.y;

            float cos = Mathf.Cos(radAngle);
            float sin = Mathf.Sin(radAngle);
            float tan = Mathf.Tan(radAngle);

            Debug.Log($"distanceX: {distanceX}, distanceY: {distanceY}");

            // distanceX�� ��ȣ�� ���� �߻� ���� ������ ����
            float direction = Mathf.Sign(distanceX);
            float absDistanceX = Mathf.Abs(distanceX);

            // ������ ���� ���� (distanceX�� �׻� ����� ���)
            float denominator = 2 * cos * cos * (absDistanceX * tan - distanceY);

            if (Mathf.Abs(denominator) < 0.001f || denominator <= 0f)
            {
                Debug.LogWarning("������ ��� ���� : denominator 0�� �����. fallback ���� �߻�.");
                Vector2 fallbackDir = distance.normalized;
                _rigid.linearVelocity = fallbackDir * 10f;
                return;
            }

            float veloSq = (_gravity * absDistanceX * absDistanceX) / denominator;

            if (veloSq <= 0 || float.IsNaN(veloSq))
            {
                Debug.LogWarning("������ ��� ���� : veloSq�� ��ȿ���� ����. fallback ���� �߻�");
                Vector2 fallbackDir = distance.normalized;
                _rigid.linearVelocity = fallbackDir * 10f;
                return;
            }

            float velocity = Mathf.Sqrt(veloSq);
            // x������ direction(��/��)�� ���� ��ȣ�� ������
            Vector2 launchVelocity = new Vector2(cos * velocity * direction, sin * velocity);

            _rigid.linearVelocity = launchVelocity;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IDamageable damageable))
            {
                //_spitter.OnBurstEvent?.Invoke(collision.transform); // �����Ϳ��� �̺�Ʈ ȣ��

                Vector2 direction = collision.transform.position - transform.position;
                Vector2 knockback = knockbackPower;
                knockback.x *= Mathf.Sign(direction.x); // ���⿡ ���� knockback�� x���� ����

                damageable.ApplyDamage(_dmg, direction, knockback, powerAttack, _owner);
            }

            _isBurst = true;
            _animator.Play("burst_Burst_2", 0);

            _myPool.Push(this); // �������� ¥����

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

