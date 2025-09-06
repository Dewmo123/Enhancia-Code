using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace Scripts.Core.Sound
{
    public class BGMPlayer : MonoBehaviour
    {
        [Inject] private PoolManagerMono _poolManager;
        [SerializeField] private PoolItemSO spItem;
        [SerializeField] private SoundSO bgmSO;

        private void Awake()
        {
            var sp = _poolManager.Pop<SoundPlayer>(spItem);
            sp.PlaySound(bgmSO,transform.position);
        }
    }
}
