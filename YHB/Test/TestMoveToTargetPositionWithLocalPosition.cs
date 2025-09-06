using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Assets._00.Work.YHB.Scripts.Test
{
    public class TestMoveToTargetPositionWithLocalPosition : MonoBehaviour
    {
        [SerializeField] private Transform local;
        [SerializeField] private Transform target;

        [ContextMenu("Move")]
        private void Move()
        {
            Vector3 targetPositionDirection = target.position - local.localPosition; // 타겟에서 로컬 포지션만큼 떨어진 값을 구해서 
            transform.position = targetPositionDirection; // 해당 위치로 이동함
        }
    }
}
