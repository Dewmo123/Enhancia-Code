using Assets._00.Work.AKH.Scripts.Core.EventSystem;
using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Stages;
using Core.EventSystem;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Library.Normal;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

namespace Assets._00.Work.YHB.Scripts.StageGenerators
{
    public class StageGenerator : MonoBehaviour
    {
        [Header("Generate Option")]
        [SerializeField] private int minStageNodeCount;
        [SerializeField] private int maxStageNodeCount;

        [Header("Generate Event")]
        [SerializeField] private EventChannelSO generateEventChannel;

        [Header("Swap Point")]
        [SerializeField] private PoolItemSO swapPointPoolItem;

        [Header("Stage Nodes")]
        [SerializeField] private List<StageNodeSO> stageNodeSOList;
        [SerializeField] private StageNodeSO startStageNodeSO;
        [SerializeField] private StageNodeSO lastStageNodeSO;

        [Header("Shop Stage")]
        [SerializeField] private StageNodeSO shopStageNodeSO;
        [SerializeField] private int startShopCircleOffset;
        [SerializeField] private int shopCircle;

        private StageNode _startStageNode;
        private Random _rand;

        private UniqueRandomPool<StageNodeSO> _stageBag; // 생성할 스테이지 범위
        private List<StageNodeSO> _stages; // 생성할 스테이지 목록
        // 여기서는 풀을 모르게 하려고
        // 생성시에도 하려면 따로 Init -> Action저장을 해야함 -> 굳이?
        private List<StageNode> _generatedStageNodeList;
        private List<SwapPoint> _generatedSwapPointList;

        [Inject] private PoolManagerMono _poolManager;

        private void Awake()
        {
            _stages = new List<StageNodeSO>();
            _stageBag = new UniqueRandomPool<StageNodeSO>();
            _rand = new Random();
            _generatedStageNodeList = new List<StageNode>();
            _generatedSwapPointList = new List<SwapPoint>();

            Debug.Log("제출시 지워도 됨. 단, 빌드전에 미리 정렬 시켜놓고 해야함. 그냥 성능에 영향을 살짝 끼침 근데 그리 크진 않을듯");
            SortStageNodeList();
        }

        public void ClearStageNode()
        {
            if (_generatedStageNodeList.Count > 0)
                _generatedStageNodeList.ForEach(stageNode => _poolManager.Push(stageNode)); ;
            _generatedStageNodeList.Clear();
        }

        public void ClearSwapPoint()
        {
            if (_generatedSwapPointList.Count > 0)
                _generatedSwapPointList.ForEach(swapPoint => _poolManager.Push(swapPoint));
            _generatedSwapPointList.Clear();
        }

        [ContextMenu("stage node list sort")]
        private void SortStageNodeList()
        {
            // active기준으로 정렬하고, 같으면 disable기준으로 정렬시킴
            // 중복제거
            stageNodeSOList = stageNodeSOList.Distinct(EqualsComarerSbtractNull<StageNodeSO>.Default).ToList();
            stageNodeSOList.Sort(GreaterComparer<StageNodeSO>.Default);
        }

        #region GenerateStage
        public async void GenerateStage(int currentFloor, Vector3 generateStartPosition)
        {
            generateEventChannel.InvokeEvent(GenerateEvent.GenerateStartEvent);

            (bool success, StageNode lastNode) generateResult = await TryGenerateStage(currentFloor, generateStartPosition);
            bool success = generateResult.success;// 빌드본에서도 실행 되어야만 함
            Debug.Assert(success, "Can't generate stage"); // 에디터에서만 실행 빌드시 제거

            await Task.Yield();

            if (_startStageNode == null)
            {
                Debug.LogError("Start stage is null!");
                generateEventChannel.InvokeEvent(GenerateEvent.GenerateCompleteEvent.Initialize(false, null, null, 0));
                return;
            }

            generateEventChannel.InvokeEvent(GenerateEvent.GenerateCompleteEvent.Initialize(
                success,
                _generatedStageNodeList.Select(stageNode => stageNode.StageCam),
                _generatedStageNodeList.Select(stageNode => stageNode.StageVolume),
                currentFloor));
        }

        private async Task<(bool success, StageNode lastNode)> TryGenerateStage(int currentFloor, Vector3 generateStartPosition) // 상당히 무거움
        {
            List<StageNodeSO> stageNodeRange;
            int count;
            if ((currentFloor + startShopCircleOffset) % shopCircle == 0) // 상점을 생성해야하는 층이면
            {
                // shopStage만 생성하도록 함
                stageNodeRange = new List<StageNodeSO> { shopStageNodeSO };
                count = 1;
            }
            else // 상점을 생성해야하는 층이 아니면
            {
                // 스테이지 노드 리스트를 복사하고
                stageNodeRange = stageNodeSOList.ToList();
                // 스테이지 노드의 현재 층이 비활성 스테이지 보다 더 크면 제거함.
                // 혹은 활성 스테이지가 현재 층보다 더 크면 제거함.
                stageNodeRange.RemoveAll(stage => stage.activeStageLevel > currentFloor || currentFloor > stage.disableStageLevel);
                count = _rand.Next(minStageNodeCount, maxStageNodeCount + 1);
            }

            if (stageNodeRange.Count <= 0)
                return (false, null);

            _stageBag.SetRange(stageNodeRange);
            _stages.Clear();

            for (int i = 0; i < count; ++i)
                _stages.Add(_stageBag.GetValue());

            StageNode lastNode = await GenerateStageNodes(_stages, generateStartPosition);

            return (true, lastNode);
        }

        private async Task<StageNode> GenerateStageNodes(List<StageNodeSO> stages, Vector3 generateStartPosition)
        {
            StageNode previousStageNode;

            #region Init
            StageNode stageNode = GenerateStageNode(startStageNodeSO);
            stageNode.transform.position = generateStartPosition;

            previousStageNode = stageNode;
            _startStageNode = stageNode;
            _startStageNode.StageCam.Priority = int.MaxValue;

            _generatedStageNodeList.Add(stageNode);
            #endregion

            stages.Add(lastStageNodeSO); // 마지막 노드만 추가

            foreach (StageNodeSO item in stages)
            {
                stageNode = GenerateStageNode(item);
                SetStageNode(previousStageNode, stageNode);
                SetStageNodeSwapPointMatching(stageNode);
                previousStageNode = stageNode;

                _generatedStageNodeList.Add(stageNode);

                await Task.Yield();
            }

            return stageNode;
        }

        private StageNode GenerateStageNode(StageNodeSO stageNodeSO)
        {
            StageNode stageNode = _poolManager.Pop<StageNode>(stageNodeSO.stageNodePoolItem);
            stageNode.transform.parent = transform;
            return stageNode;
        }

        private void SetStageNode(StageNode previousStageNode, StageNode stageNode)
        {
            previousStageNode.SetDirectionStageNode(CommonEnum.DirectionInformation.Right, stageNode);
            stageNode.SetDirectionStageNode(CommonEnum.DirectionInformation.Left, previousStageNode);
        }

        /// <summary>
        /// StageNode의 전 StageNode와 StageNode를 연결합니다.
        /// </summary>
        /// <remarks>
        /// 전 StageNode가 Null이라면, 건너뜁니다.
        /// </remarks>
        private void SetStageNodeSwapPointMatching(StageNode stageNode)
        {
            // 만일 위, 아래도 추가 되면 간단하게 방향받고 Enum에 -1곱하면 됨.
            // 자기 왼쪽에 있는 스테이지 노드의 오른쪽 SwapPoint
            StageNode previousStageNode = stageNode[CommonEnum.DirectionInformation.Left];

            if (previousStageNode == null) // 스왑 포인트 위치를 매칭 시킬 필요가 없음
                return;

            Transform previousStageSwapPoint = previousStageNode.SwapPoints[CommonEnum.DirectionInformation.Right];
            Transform currentStageSwapPoint = stageNode.SwapPoints[CommonEnum.DirectionInformation.Left];

            Vector3 targetPositionDirection = previousStageSwapPoint.position - currentStageSwapPoint.localPosition; // 타겟에서 로컬 포지션만큼 떨어진 값을 구해서
            stageNode.transform.position = targetPositionDirection; // 해당 위치로 이동함

            GenerateSwapPoint(previousStageNode, stageNode, previousStageSwapPoint.position);
        }

        private void GenerateSwapPoint(StageNode previousStageNode, StageNode stageNode, Vector3 generatePosition)
        {
            SwapPoint swapPoint = _poolManager.Pop<SwapPoint>(swapPointPoolItem);
            swapPoint.transform.position = generatePosition;
            swapPoint.transform.parent = transform;

            // StageNode들의 Swap정보를 저장함
            InteractiveStageInfo interactiveStageInfo = new InteractiveStageInfo(
                previousCamera: previousStageNode.StageCam,
                nextCamera: stageNode.StageCam,
                previousVolume: previousStageNode.StageVolume,
                nextVolume: stageNode.StageVolume);

            swapPoint.SetSwapInformation(interactiveStageInfo); // SwapPoint에 저장한 정보를 넣어줌
            _generatedSwapPointList.Add(swapPoint);
        }
        #endregion
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (stageNodeSOList != null)
                stageNodeSOList = stageNodeSOList.Where(stageNodeSO => !stageNodeSO.isUniqueStage).ToList();

            if (startStageNodeSO == null || lastStageNodeSO == null)
            {
                Debug.LogWarning("Need start stage node and last stage node");
                return;
            }

            if (swapPointPoolItem == null)
            {
                Debug.LogWarning("Need swap point");
                return;
            }

            if (!swapPointPoolItem.prefab.TryGetComponent(out SwapPoint swapPoint))
                swapPointPoolItem = null;
        }
#endif
    }
}
