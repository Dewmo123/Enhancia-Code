using _00.Work.CDH.Code.Combat;
using Core.EventSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Assets._00.Work.YHB.Scripts.Core
{
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private CinemachineImpulseSource impulseSource;
        [SerializeField] private EventChannelSO cameraEventChannel;
        [SerializeField] private float defaultShakeMultiplier, weekShakeMultiplier;

        public void Awake()
        {
            cameraEventChannel.AddListener<CameraShakeEvent>(HandleCameraShakeEvent);
        }

        private void OnDestroy()
        {
            cameraEventChannel.RemoveListener<CameraShakeEvent>(HandleCameraShakeEvent);
        }

        private void HandleCameraShakeEvent(CameraShakeEvent evt)
        {
            Vector2 shakePower = Vector2.zero;

            float hitMultiplier = evt.weekShake ? weekShakeMultiplier : defaultShakeMultiplier;
            float cameraShakePower = evt.shakePower * hitMultiplier;

            int directionMultiplier = Random.value < 0.5f ? -1 : 1;

            shakePower.x = cameraShakePower * directionMultiplier;
            shakePower.y = cameraShakePower * directionMultiplier;

            impulseSource.DefaultVelocity = shakePower;
            impulseSource.ImpulseDefinition.ImpulseDuration = evt.shakeDuration;

            if (evt.useWorldPosition)
            {
                impulseSource.ImpulseDefinition.ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Propagating;

                impulseSource.transform.position = evt.position;
                impulseSource.ImpulseDefinition.DissipationRate = evt.dissipationRate;
                impulseSource.ImpulseDefinition.PropagationSpeed = evt.propagationSpeed;
                impulseSource.ImpulseDefinition.DissipationDistance = evt.maxWorldDistance;
            }
            else
            {
                impulseSource.ImpulseDefinition.ImpulseType = CinemachineImpulseDefinition.ImpulseTypes.Uniform;
            }

            impulseSource.GenerateImpulse();
        }
    }
}
