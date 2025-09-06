using _00.Work.CDH.Code.Core.StatSystem;
using _00.Work.CSH._01.Scripts.Enemies;
using Assets._00.Work.AKH.Scripts.Core.EventSystem;
using Assets._00.Work.YHB.Scripts.Core;
using Core.EventSystem;
using DewmoLib.ObjectPool.RunTime;
using Library.Normal;
using Library.Unity;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Stages
{
    public class EnemySpawnPoint : MonoBehaviour
    {
        [Header("Event Channel")]
        [SerializeField] private EventChannelSO stageGenerateEventChannel;
        [SerializeField] private EventChannelSO inStageEventChannel;

        [Header("Pool")]
        [SerializeField] private ObjectFinderSO poolMonoFinder;

        [Header("Spawn Infomation")]
        [SerializeField] private sbyte spawnCount;
        [SerializeField] private sbyte offsetSpawnCount;
        public EnemySpawnInfomation[] enemySpawnInfomationList;

        [Header("Modifier Stats")]
        [Range(0f, 2f)]
        [SerializeField] private float enemyEnchanceMinValueInLocal = 0;
        [Range(0f, 2f)]
        [SerializeField] private float enemyEnchanceMaxValueInLocal = 1;
        [SerializeField] private float enemyEnchanceMultipyValue = 10;
        [SerializeField] private float enemyEnchanceValue = 0.05f;
        [Tooltip("다른 모든 적 소환 포인트들과 동일하게 stat들 적용됩니다.")]
        [SerializeField] private List<StatSO> targetStatList;

        private static List<StatSO> targetModifierStatList;

        private PoolManagerMono _poolManager;

        private int _totalFrequency = 0;
        private List<BTEnemy> _enemies;

        private void Awake()
        {
            if (targetModifierStatList == null)
                targetModifierStatList = new List<StatSO>();
            targetModifierStatList.MargeTo(targetStatList);

            _poolManager = poolMonoFinder.GetObject<PoolManagerMono>();
        }

        private void Start()
        {
            _enemies = new List<BTEnemy>();

            foreach (EnemySpawnInfomation enemySpawnInfomation in enemySpawnInfomationList)
                _totalFrequency += enemySpawnInfomation.spawnFrequency;
        }

        private void OnEnable()
        {
            stageGenerateEventChannel.AddListener<GenerateCompleteEvent>(HandleStageGenerateComplete);
            inStageEventChannel.AddListener<StageEndEvent>(HandleStageEndEvent);
        }

        private void OnDisable()
        {
            stageGenerateEventChannel.RemoveListener<GenerateCompleteEvent>(HandleStageGenerateComplete);
            inStageEventChannel.RemoveListener<StageEndEvent>(HandleStageEndEvent);
        }

        private void HandleStageGenerateComplete(GenerateCompleteEvent evt)
        {
            if (!evt.success)
                return;

            int count = spawnCount + Random.Range(-offsetSpawnCount, offsetSpawnCount);
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy(evt.currentFloor);
            }

            inStageEventChannel.InvokeEvent(EnemySpawnEvent.EnemyCountChangeEvent.Initialize(_enemies.Count)); ;
        }

        private void HandleStageEndEvent(StageEndEvent evt)
        {
            ClearEnemies();
        }

        private void SpawnEnemy(int currentFloor)
        {
            int seed = Random.Range(1, _totalFrequency);
            // 최적화 필요할 듯.
            foreach (EnemySpawnInfomation enemySpawnInfomation in enemySpawnInfomationList)
            {
                if ((seed -= enemySpawnInfomation.spawnFrequency) <= 0)
                {
                    SpawnFromEnemySpawnInfomation(enemySpawnInfomation, currentFloor);
                    break;
                }
            }
        }

        private void SpawnFromEnemySpawnInfomation(EnemySpawnInfomation enemySpawnInfomation, int currentFloor)
        {
            BTEnemy obj = _poolManager.Pop<BTEnemy>(enemySpawnInfomation.enemyPoolItem);
            BTEnemy enemy = new GameObjectBuilder(obj.gameObject)
                .SetParent(transform)
                .SetPosition(transform.position)
                .SetToGetComponent<BTEnemy>(e =>
                {
                    e.SetStat(CalcStatCost(currentFloor) *
                        Random.Range(enemyEnchanceMinValueInLocal, enemyEnchanceMaxValueInLocal),
                        targetModifierStatList.ToArray());
                    e.OnEnemyDead += HandleEnemyDeadEvent;
                })
                .Build()
                .GetComponent<BTEnemy>();
            _enemies.Add(enemy);
        }

        private void ClearEnemies()
        {
            foreach (BTEnemy enemy in _enemies)
            {
                enemy.PushToPool();
            }
            _enemies.Clear();
        }

        private void HandleEnemyDeadEvent(BTEnemy enemy)
        {
            enemy.OnEnemyDead -= HandleEnemyDeadEvent;
            inStageEventChannel.InvokeEvent(EnemySpawnEvent.EnemyCountChangeEvent.Initialize(-1));
        }

        private int CalcStatCost(float level)
        {
            float a = 10f;
            float k = enemyEnchanceValue;
            return Mathf.RoundToInt(-a * (1 - Mathf.Exp(k * Mathf.RoundToInt(level))));
        }

        private void OnValidate()
        {
            enemyEnchanceMaxValueInLocal = Mathf.Max(enemyEnchanceMinValueInLocal, enemyEnchanceMaxValueInLocal);

            foreach (EnemySpawnInfomation enemySpawnInfomation in enemySpawnInfomationList)
            {
                if (enemySpawnInfomation.enemyPoolItem != null
                    && !enemySpawnInfomation.enemyPoolItem.prefab.TryGetComponent(out BTEnemy enemy))
                    Debug.LogWarning($"{enemySpawnInfomation.enemyPoolItem.name} is not BTenemy");
            }
        }

#if UNITY_EDITOR 

        [ContextMenu("Sort Transfom")]
        private void SortTransform()
        {
            Vector3 origin = transform.position;

            origin.x = Mathf.CeilToInt(origin.x) - 0.5f;
            origin.y = Mathf.CeilToInt(origin.y) - 0.5f;
            origin.z = 0;


            transform.position = origin;
        }
#endif
    }
}
