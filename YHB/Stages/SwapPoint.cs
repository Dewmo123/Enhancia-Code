using Assets._00.Work.YHB.Scripts.StageGenerators;
using Core.EventSystem;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;
using UnityEngine.Pool;

namespace Assets._00.Work.YHB.Scripts.Stages
{
	public sealed class SwapPoint : MonoBehaviour, IPoolable
	{
		[SerializeField] private LayerMask _plyerLayer;

		[SerializeField] private EventChannelSO _cameraChangeEventChannel;
		[SerializeField] private EventChannelSO _volumeChangeEventChannel;

		[SerializeField] private BoxCollider2D _collider;

		private InteractiveStageInfo _interactiveCameraInfo;
		private bool _isEnterPrevious; // 보통 -방향

		[HideInInspector] public bool isLastNode;

        [Header("Pool Set")]
        [field : SerializeField] public PoolItemSO PoolItem {  get; private set; }
        [field: SerializeField] public GameObject GameObject { get; private set; }

        //private bool _setTrigger, _setSwapInformation;

        //public void SetTrigger(Vector2 center, Vector2 size, bool isHorizontal, LayerMask targetLayer)
        //{
        //    _collider.offset = Vector2.zero;
        //    transform.position = center;
        //    _plyerLayer = targetLayer;
        //    _collider.size = isHorizontal ? (Quaternion.Euler(0, 90, 0) * size) : size; // isHorizontal이면 90를 돌림

        //    _setTrigger = true;
        //}

        //public void SetSwapInformation(EventChannelSO cameraChangeEventChannel, EventChannelSO volumeChangeEventChannel, InteractiveStageInfo interactiveCameraInfo)
        //{
        //    _cameraChangeEventChannel = cameraChangeEventChannel;
        //    _volumeChangeEventChannel = volumeChangeEventChannel;
        //    _interactiveCameraInfo = interactiveCameraInfo;

        //    _setSwapInformation = true;
        //}

        public void SetSwapInformation(InteractiveStageInfo interactiveCameraInfo)
		{
			_interactiveCameraInfo = interactiveCameraInfo;
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (1 << collision.gameObject.layer == _plyerLayer)
			{
				_isEnterPrevious = GetCollisionDirection(collision.transform.position);
			}
		}

		private void OnTriggerExit2D(Collider2D collision)
		{
			//if (!(_setTrigger && _setSwapInformation))
			//    return;

			if ((1 << collision.gameObject.layer & _plyerLayer) != 0)
			{
				if (_isEnterPrevious == GetCollisionDirection(collision.transform.position))
					return;

				CameraSwapEvent cameraSwapChangeEvent = CameraEvent.CameraSwapEvent;

				cameraSwapChangeEvent.previousCamera = _interactiveCameraInfo.previousCamera;
				cameraSwapChangeEvent.nextCamera = _interactiveCameraInfo.nextCamera;

				_cameraChangeEventChannel.InvokeEvent(cameraSwapChangeEvent);

				VolumeChangeEvent volumeChangeEvent = VolumeEvent.volumeChangeChangeEvent;

				volumeChangeEvent.previousVolume = _interactiveCameraInfo.previousVolume;
				volumeChangeEvent.nextVolume = _interactiveCameraInfo.nextVolume;

                _volumeChangeEventChannel.InvokeEvent(volumeChangeEvent);
			}
		}

		private bool GetCollisionDirection(Vector2 collisionPosition)
		{
			Vector2 direction = collisionPosition - (Vector2)transform.position;
			return (_interactiveCameraInfo.IsDirectionHorizontal ? direction.x : direction.y) < 0;
		}

		private void OnValidate()
		{
			if (_collider == null)
				return;

			_collider.offset = Vector2.zero;
			_collider.size = new Vector2(Mathf.RoundToInt(_collider.size.x), Mathf.RoundToInt(_collider.size.y));
			_collider.isTrigger = true;
		}

        public void SetUpPool(Pool pool)
        {
        }

        public void ResetItem()
        {
        }
    }
}
