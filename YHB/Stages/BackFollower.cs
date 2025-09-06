using Assets._00.Work.AKH.Scripts.Core.EventSystem;
using Assets._00.Work.YHB.Scripts.Core;
using Core.EventSystem;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Stages
{
    public class BackFollower : MonoBehaviour
    {
        [SerializeField] private ObjectFinderSO playerFinder;
        [SerializeField] private EventChannelSO generateEventChannel;

#if UNITY_EDITOR
        [Header("Bounds Setting")]
        [SerializeField] private Vector2 followBoundsCenter;
        [SerializeField] private Vector2 followBoundsSize;
        [SerializeField] private Vector2 moveBoundsCenter;
        [SerializeField] private Vector2 moveBoundsSize;
#endif

        [Header("Bounds")]
        [HideInInspector][SerializeField] private Vector2 followStartPoint;
        [HideInInspector][SerializeField] private Vector2 followEndPoint;
        [HideInInspector][SerializeField] private Vector2 moveStartPoint, moveEndPoint;

        private Vector2 _startPosition;
        private bool _complete;

        private void Awake()
        {
            _complete = false;
            generateEventChannel.AddListener<GenerateCompleteEvent>(HandleGenerateCompleteEvent);
        }

        private void HandleGenerateCompleteEvent(GenerateCompleteEvent evt)
        {
            _complete = true;
            _startPosition = transform.position;
        }

        private void OnDestroy()
        {
            generateEventChannel.RemoveListener<GenerateCompleteEvent>(HandleGenerateCompleteEvent);
        }

        private void Update()
        {
            if (_complete)
            {
                (float, float) perTargetValue = GetTargetVectorLerpValue(_startPosition + followStartPoint, _startPosition + followEndPoint, (Vector2)playerFinder.Object.transform.position);
                transform.position = _startPosition + GetLerpVector(moveStartPoint, moveEndPoint, perTargetValue);
            }
        }

        private (float, float) GetTargetVectorLerpValue(Vector2 s, Vector2 e, Vector2 t)
        {
            float x = Mathf.Clamp01((t.x - s.x) / (e.x - s.x));
            float y = Mathf.Clamp01((t.y - s.y) / (e.y - s.y));
            return (x, y);
        }

        private Vector2 GetLerpVector(Vector2 s, Vector2 e, (float, float) t)
        {
            return new Vector2(Mathf.Lerp(s.x, e.x, t.Item1), Mathf.Lerp(s.y, e.y, t.Item2));
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Vector2 bs = followBoundsSize / 2;
            followStartPoint = new Vector2(followBoundsCenter.x + bs.x, followBoundsCenter.y + bs.y);
            followEndPoint = new Vector2(followBoundsCenter.x - bs.x, followBoundsCenter.y - bs.y);

            bs = moveBoundsSize / 2;
            moveStartPoint = new Vector2(moveBoundsCenter.x + bs.x, moveBoundsCenter.y + bs.y);
            moveEndPoint = new Vector2(moveBoundsCenter.x - bs.x, moveBoundsCenter.y - bs.y);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube((Vector2)transform.position + followBoundsCenter, followBoundsSize);
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube((Vector2)transform.position + moveBoundsCenter, moveBoundsSize);
        }
#endif
    }
}
