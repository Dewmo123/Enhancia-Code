using Unity.Cinemachine;
using UnityEngine;

namespace Core.EventSystem
{
    public class CameraEvent
    {
        public static readonly CameraSwapEvent CameraSwapEvent = new CameraSwapEvent();
        public static readonly CameraShakeEvent CameraShakeEvent = new CameraShakeEvent();
    }

    public class CameraSwapEvent : GameEvent
    {
        public CinemachineCamera previousCamera;
        public CinemachineCamera nextCamera;
    }

    public class CameraShakeEvent : GameEvent
    {
        public float shakePower;
        public float shakeDuration;
        public bool weekShake;

        /// <summary>
        /// world position기준으로 카메라를 흔들지 여부입니다. (기본 : 상관없이 흔듬)
        /// </summary>
        public bool useWorldPosition;
        /// <summary>
        /// 흔들림이 발생될 위치입니다.
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// 흔들리는 힘이 얼마나 빠르게 감소할지 여부입니다. (0~1) (기본 : 감소안함)
        /// </summary>
        public float dissipationRate;
        /// <summary>
        /// 발생 위치로부터 얼마나 빠르게 카메라를 흔드는 힘이 도착하는지 여부입니다. (기본 : 즉시)
        /// </summary>
        public float propagationSpeed;
        /// <summary>
        /// 얼마나 멀리까지 힘이 도착하는지 여부입니다. 거리가 멀수록 힘이 약해집니다.
        /// </summary>
        public float maxWorldDistance;

        public CameraShakeEvent Initialize(float shakePower, float shakeDuration, bool weekShake = false,
            bool useWorldPosition = false, Vector2 position = default, float dissipationRate = 0, float propagationSpeed = 0, float maxWorldDistance = 100)
        {
            this.shakePower = shakePower;
            this.shakeDuration = shakeDuration;
            this.weekShake = weekShake;

            this.position = position;
            this.useWorldPosition = useWorldPosition;
            this.dissipationRate = dissipationRate;
            this.propagationSpeed = propagationSpeed;
            this.maxWorldDistance = maxWorldDistance;

            return this;
        }
    }
}
