using UnityEngine;
using UnityEngine.Events;

namespace _00.Work.CDH.Code.SkillSystem.Skills.Clone
{
    public class CloneRenderer : MonoBehaviour
    {
        public UnityEvent OnAttackTrigger;
        public UnityEvent OnAnimationEnd;
        public UnityEvent OnSetAnimationSetting;
        
        private void AttackTrigger() => OnAttackTrigger?.Invoke();
        private void AnimationEnd() => OnAnimationEnd?.Invoke();
        private void SetAnimationSetting() => OnSetAnimationSetting?.Invoke();
    }
}