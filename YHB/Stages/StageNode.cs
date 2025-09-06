using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using DirectionInformation = Assets._00.Work.YHB.Scripts.Core.CommonEnum.DirectionInformation;
using Library.Unity;
using DewmoLib.ObjectPool.RunTime;


#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.Tilemaps;
#endif

namespace Assets._00.Work.YHB.Scripts.Stages
{
    public class StageNode : MonoBehaviour, IPoolable
    {
#if UNITY_EDITOR
        [HideInInspector][SerializeField] private GameObject stageCamPrefabForDefaultSetting;
        [HideInInspector][SerializeField] private int defaultTileMapLayer = 6; // Default Reference처럼 안 된다..
#endif

        [SerializeField] private Grid grid; // NodeSwapPoints의 각 방향의 생성지점을 가장 가까운 그리드 칸으로 이동시키기 위함
        [field: SerializeField] public CinemachineCamera StageCam { get; private set; }
        [field: SerializeField] public Volume StageVolume { get; private set; }

        [Tooltip("up, down은 안 씀")]
        [field: SerializeField] public TransformPoints SwapPoints { get; private set; }
        private Dictionary<DirectionInformation, StageNode> _neighborNodes = new Dictionary<DirectionInformation, StageNode>(); // 인접 노드

        public StageNode this[DirectionInformation direction]
            => _neighborNodes[direction];

        // [HideInInspector] public int distance = -1; // BFS알고리즘을 사용하기 위한 거리값

        #region Pool
        [Header("Pool Set")]
        [field: SerializeField] public PoolItemSO PoolItem { get; private set; }
        [field: SerializeField] public GameObject GameObject { get; private set; }

        public void SetUpPool(Pool pool)
        {
        }

        public void ResetItem()
        {
        }

        #endregion

        public void SetDirectionStageNode(DirectionInformation direction, StageNode stageNode)
            => _neighborNodes[direction] = stageNode;

#if UNITY_EDITOR
        [ContextMenu("Set Swap Point Transfom")]
        private void SetSwapPointTransform()
        {
            if (grid == null)
                SetGrid();

            Vector2 cellSize = grid.cellSize;

            foreach (DirectionInformation dir in Enum.GetValues(typeof(DirectionInformation))) // 모든방향을
            {
                Transform swapPoint = SwapPoints[dir];

                if (swapPoint == null)
                    continue;

                Vector2 direction = swapPoint.position - transform.position;

                int xCount = Mathf.CeilToInt(direction.x / cellSize.x); // 시작점으로부터 셀의 갯수 (방향포함)
                int yCount = Mathf.CeilToInt(direction.y / cellSize.y);

                SwapPoints[dir].position = (Vector2)transform.position + new Vector2(xCount * cellSize.x - (cellSize.x / 2), yCount * cellSize.y - (cellSize.y / 2));
            }
        }

        [ContextMenu("Set StageNode Default Settings")]
        private void SetStageNodeDefaultSettings()
        {
            SetTileMap();

            if (StageCam == null)
                SetStageCamera();

            SetCameraConfiner();

            if (grid == null)
                SetGrid();

            SetDirectionTransform();
        }

        private void SetTileMap()
        {
            if (transform.GetComponentInChildren<Tilemap>() == null)
            {
                GameObject gameObject = new GameObjectBuilder()
                    .SetName("TileMap")
                    .SetParent(transform)
                    .TryCatch(builder => builder.SetLayer(defaultTileMapLayer), (builder, ex) => Debug.LogError(ex.Message))
                    .SetLocalPosition(Vector3.zero)
                    .SetScale(Vector3.one)
                    .TryAddComponent<Tilemap>()
                    .TryAddComponent<TilemapRenderer>()
                    .SetComponent<TilemapCollider2D>(col => col.compositeOperation = Collider2D.CompositeOperation.Merge)
                    .SetComponent<Rigidbody2D>(rid => rid.bodyType = RigidbodyType2D.Static)
                    .TryAddComponent<CompositeCollider2D>()
                    .Build();
            }
        }

        private void SetStageCamera()
        {
            StageCam = GetComponentInChildren<CinemachineCamera>();

            if (StageCam == null)
            {
                if (stageCamPrefabForDefaultSetting == null)
                    Debug.LogWarning("Stage Cam Prefab For Default Setting is null");

                GameObject tempGameObject = (GameObject)PrefabUtility.InstantiatePrefab(stageCamPrefabForDefaultSetting);

                if (tempGameObject.TryGetComponent(out CinemachineCamera cam))
                {
                    tempGameObject.transform.parent = transform;
                    StageCam = cam;
                    StageCam.name = "StageCamera";
                }
                else
                {
                    Destroy(tempGameObject);
                    Debug.LogWarning("Stage Cam Prefab For Default Setting hasn't " + typeof(CinemachineCamera));
                }
            }
        }

        // StageCamera 없으면 실행 되면 안 됨
        private void SetCameraConfiner()
        {
            Transform cameraConfiner = FindChildWithTag(transform, "CameraConfiner");
            Collider2D collider;
            if (cameraConfiner == null)
            {
                cameraConfiner = new GameObject("CameraConfiner").transform;
                cameraConfiner.parent = transform;
                cameraConfiner.tag = "CameraConfiner";
            }

            if (!cameraConfiner.TryGetComponent(out collider))
                collider = cameraConfiner.gameObject.AddComponent<BoxCollider2D>();

            collider.includeLayers = 0;
            collider.excludeLayers = -1;

            if (StageCam.TryGetComponent(out CinemachineConfiner2D camConfine))
                camConfine.BoundingShape2D = collider;
            else
                Debug.LogWarning("Can't find " + typeof(CinemachineConfiner2D) + " in Stage Camera");
        }

        private void SetDirectionTransform()
        {
            foreach (DirectionInformation dir in Enum.GetValues(typeof(DirectionInformation)))
            {
                Transform swapPoint = SwapPoints[dir];

                if (swapPoint == null)
                {
                    GameObject directionTransform = new GameObject(dir.ToString());
                    directionTransform.transform.parent = transform;
                    SwapPoints.SetTransform(dir, directionTransform.transform);
                }
            }
        }

        private void SetGrid()
        {
            if (!transform.TryGetComponent(out grid)) // 가져오는데 실패하면 컴포넌트 추가
                grid = gameObject.AddComponent<Grid>();
        }

        // 지랄적이지만, 에디터 단계에서 임의로 메서드를 실행 시키겠다고 할 때만 호출할거라 ㄱㅊ
        private Transform FindChildWithTag(Transform parent, string tag)
        {
            foreach (Transform child in parent)
            {
                if (child.CompareTag(tag))
                    return child;

                // 자식의 자식까지 탐색
                Transform result = FindChildWithTag(child, tag);
                if (result != null)
                    return result;
            }

            return null;
        }
#endif
    }
}
