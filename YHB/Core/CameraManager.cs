using Assets._00.Work.AKH.Scripts.Core.EventSystem;
using Core.EventSystem;
using Library.Unity;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Core
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Default Setting")]
        [field: SerializeField] public ObjectFinderSO FollowTargetObjectFinder { get; private set; }
        [SerializeField] private EventChannelSO generateEventChannel;
        [SerializeField] private EventChannelSO cameraChangeEventChannel;

        [Header("Camera Setting")]
        [SerializeField] private int activeCameraPriority = 15;
        [SerializeField] private int disableCameraPriority = 10;

        private CinemachineBrain _mainCamera;
        private CinemachineCamera _currentCinemachineCamera;

        private float _defaultBlendTime;
        private CinemachineBlendDefinition.Styles _defaultBlendStyle;

        private void Awake()
        {
            _mainCamera = Camera.main.transform.TryGetOrAddComponent<CinemachineBrain>();
            _defaultBlendTime = _mainCamera.DefaultBlend.Time;
            _defaultBlendStyle = _mainCamera.DefaultBlend.Style;

            cameraChangeEventChannel.AddListener<CameraSwapEvent>(HandleCameraSwapChangeEvent);
            generateEventChannel.AddListener<GenerateStartEvent>(HandleGenerateStartEvent);
            generateEventChannel.AddListener<GenerateCompleteEvent>(HandleGenerateCompleteEvent);
        }

        private void OnDestroy()
        {
            cameraChangeEventChannel.RemoveListener<CameraSwapEvent>(HandleCameraSwapChangeEvent);
            generateEventChannel.RemoveListener<GenerateStartEvent>(HandleGenerateStartEvent);
            generateEventChannel.RemoveListener<GenerateCompleteEvent>(HandleGenerateCompleteEvent);
        }

        private void ChangeCamera(CinemachineCamera newCamera)
        {
            if (_currentCinemachineCamera == null)
            {
                _currentCinemachineCamera = newCamera;
                _currentCinemachineCamera.Follow = FollowTargetObjectFinder.Object.transform;
                return;
            }

            _currentCinemachineCamera.Priority = disableCameraPriority;
            Transform followTarget = _currentCinemachineCamera.Follow;
            _currentCinemachineCamera.Follow = null;

            _currentCinemachineCamera = newCamera;
            _currentCinemachineCamera.Priority = activeCameraPriority;
            _currentCinemachineCamera.Follow = followTarget;

            _currentCinemachineCamera.ForceCameraPosition(FollowTargetObjectFinder.Object.transform.position, Quaternion.identity);
        }

        private void HandleCameraSwapChangeEvent(CameraSwapEvent cameraSwapChangeEvent)
        {
            if (EqualityComparer<CinemachineCamera>.Default.Equals(cameraSwapChangeEvent.nextCamera, _currentCinemachineCamera))
                ChangeCamera(cameraSwapChangeEvent.previousCamera);
            else if (EqualityComparer<CinemachineCamera>.Default.Equals(cameraSwapChangeEvent.previousCamera, _currentCinemachineCamera))
                ChangeCamera(cameraSwapChangeEvent.nextCamera);
        }

        private void HandleGenerateStartEvent(GenerateStartEvent evt)
        {
            _mainCamera.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
            _mainCamera.DefaultBlend.Time = 0;
        }

        private void HandleGenerateCompleteEvent(GenerateCompleteEvent generateCompleteEvent)
        {
            if (generateCompleteEvent.success)
            {
                List<CinemachineCamera> stageCameraList = generateCompleteEvent.stageCameraList.ToList();

                // 추후 개선 필요, 성능에 영향은 그리 크지는 않기 하겠다만...
                foreach (CinemachineCamera camera in stageCameraList)
                {
                    camera.Follow = FollowTargetObjectFinder.Object.transform;
                }

                ChangeCamera(stageCameraList[0]);
                _mainCamera.DefaultBlend.Style = _defaultBlendStyle;
                _mainCamera.DefaultBlend.Time = _defaultBlendTime;
            }
        }
    }
}
