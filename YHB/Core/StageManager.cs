using _00.Work.CDH.Code.Entities;
using _00.Work.CDH.Code.Players;
using Assets._00.Work.YHB.Scripts.StageGenerators;
using Assets._00.Work.YHB.Scripts.Stages;
using Core.EventSystem;
using Core.Managers;
using Core.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Core
{
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private StageGenerator stageGenerator;
        [SerializeField] private ObjectFinderSO playerFinder;
        [SerializeField] private EventChannelSO inStageEventChannel;

        private int _currentFloor;

        private void Awake()
        {
            inStageEventChannel.AddListener<StageEndEvent>(HandleStageEndEvent);
            playerFinder.GetObject<Entity>().OnDead.AddListener(HandlePlayerDead);
        }

        private void HandlePlayerDead()
        {
            Debug.Log(_currentFloor);
            NetworkConnector.Instance.GetController<PlayerDataNetworkController>()
                .StageEnd(new ServerCode.DTO.StageEndDTO() { stageCount = _currentFloor - 1 });
        }

        private void Start()
        {
            _currentFloor = 0;
            NextStage();
        }

        private void OnDestroy()
        {
            inStageEventChannel.RemoveListener<StageEndEvent>(HandleStageEndEvent);
        }

        private void HandleStageEndEvent(StageEndEvent evt)
        {
            NextStage();
        }

        private void NextStage()
        {
            ClearStageNodes();

            playerFinder.Object.transform.position = Vector3.zero;
            stageGenerator.GenerateStage(++_currentFloor, Vector3.zero);
        }

        private void ClearStageNodes()
        {
            stageGenerator.ClearStageNode();
            stageGenerator.ClearSwapPoint();
        }

#if UNITY_EDITOR
        [Header("Test")]
        [SerializeField] private int nextfloor;
        [ContextMenu("Applynextfloor")]
        private void Applynextfloor()
        {
            _currentFloor = nextfloor;
        }
#endif
    }
}
