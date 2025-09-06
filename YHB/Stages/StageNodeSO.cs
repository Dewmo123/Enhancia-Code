using DewmoLib.ObjectPool.RunTime;
using System;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Stages
{
	[CreateAssetMenu(fileName = "StageNode", menuName = "SO/Stage/Node", order = 0)]
	public class StageNodeSO : ScriptableObject, IComparable<StageNodeSO>
	{
        [field: SerializeField] public PoolItemSO stageNodePoolItem { get; private set; }
        [Tooltip("스테이지가 나타나는 시점")]
        public ushort activeStageLevel;
        [Tooltip("스테이지가 나타나지 않는 시점")]
        public ushort disableStageLevel;

#if UNITY_EDITOR
        [Header("Only Unity Editor Setting")]
        public bool isUniqueStage;
#endif

        public int CompareTo(StageNodeSO other)
        {
            int result = activeStageLevel.CompareTo(other.activeStageLevel);

            if (result == 0)
                result = disableStageLevel.CompareTo(other.disableStageLevel);

            return result;
        }

        private void OnValidate()
        {
            activeStageLevel = (ushort)Mathf.Max(activeStageLevel, 1);
            disableStageLevel = (ushort)Mathf.Max(activeStageLevel, disableStageLevel);

            if (stageNodePoolItem == null)
                return;

            if (!stageNodePoolItem.prefab.TryGetComponent(out StageNode stageNode))
            {
                stageNodePoolItem = null;
                Debug.Log("Stage node Prefab is null!");
            }
        }
    }
}
