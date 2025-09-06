using _00.Work.CDH.Code.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Assets._00.Work.CDH.Code.SkillSystem.Effects
{
    public class SkillEffectAnimationTrigger : MonoBehaviour
    {
        public event Action OnAnimationEnd;

        private void AnimationEnd() => OnAnimationEnd?.Invoke();
    }
}
