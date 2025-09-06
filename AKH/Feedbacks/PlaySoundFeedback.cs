using _00.Work.CDH.Code.Feedbacks;
using Assets._00.Work.YHB.Scripts.Core;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using Scripts.Core.Sound;
using UnityEngine;

namespace Scripts.Feedbacks
{
    public class PlaySoundFeedback : Feedback
    {

        [SerializeField] private SoundSO sound;
        [SerializeField] private PoolItemSO spItem;
        [SerializeField] private Transform playTrm;

        [SerializeField] private ObjectFinderSO poolManagerFinder;
        private PoolManagerMono _poolManager;
        private void Awake()
        {
            _poolManager = poolManagerFinder.GetObject<PoolManagerMono>();
        }
        public override void CreateFeedback()
        {
            var sp = _poolManager.Pop<SoundPlayer>(spItem);
            sp.PlaySound(sound, playTrm.position);
        }
    }
}
