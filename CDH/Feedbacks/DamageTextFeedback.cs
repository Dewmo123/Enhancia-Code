using Assets._00.Work.YHB.Scripts.Core;
using Assets._00.Work.YHB.Scripts.Others;
using DewmoLib.Dependencies;
using DewmoLib.ObjectPool.RunTime;
using UnityEngine;

namespace _00.Work.CDH.Code.Feedbacks
{
    public class DamageTextFeedback : Feedback
    {
        [SerializeField] private PoolItemSO damageTextPoolItem;
        [SerializeField] private ObjectFinderSO poolMonoFinder;
        private PoolManagerMono _poolManager;

        [SerializeField] private Color textColor;
        [SerializeField] private float textSize = 5;

        private void Awake()
        {
            bool settingPoolSuceess = poolMonoFinder.GetObject(out _poolManager);
            Debug.Assert(settingPoolSuceess, "can't set poolmono");
        }

        public override void CreateFeedback()
        {
            Debug.Log("<color=red>이거 안 씁니다.<color/>");
        }

        public void CreateFeedback(float damage)
        {
            DamageText damageText = _poolManager.Pop<DamageText>(damageTextPoolItem);
            damageText.SetText(Mathf.RoundToInt(damage).ToString(), textSize, transform.position, textColor);
        }
    }
}
