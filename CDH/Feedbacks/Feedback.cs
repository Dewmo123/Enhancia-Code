using UnityEngine;

namespace _00.Work.CDH.Code.Feedbacks
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void CreateFeedback();

        public virtual void FinishFeedback()
        { }
    }
}