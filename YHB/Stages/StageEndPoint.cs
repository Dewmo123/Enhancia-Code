using Core.EventSystem;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Stages
{
    public class StageEndPoint : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private EventChannelSO inStageEventChannel;

        // collider의 Trigger를 꺼서 더는 못 내려가게 하기 위함.
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((1 << collision.gameObject.layer & playerLayer) != 0)
                inStageEventChannel.InvokeEvent(InStageEvent.StageEndEvent);
        }

#if UNITY_EDITOR
        private void Awake()
        {
            Debug.Log("Layer는 Ground제외 아무거나. 계속 떨어지는 느낌을 주기 위함. 그렇기에, 벽도 Ground 주지마셈");
        }
#endif
    }
}
