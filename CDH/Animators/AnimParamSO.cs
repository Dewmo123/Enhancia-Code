using UnityEngine;

namespace _00.Work.CDH.Code.Animators
{
    [CreateAssetMenu(fileName = "ParamSO", menuName = "SO/Animator/Param")]
    public class AnimParamSO : ScriptableObject
    {
        public string parameterName;
        public int hashValue;
        [TextArea]
        public string description;

        private void OnValidate()
        {
            parameterName = name.Replace("Param", "");
            hashValue = Animator.StringToHash(parameterName);
        }

        //public override void SetNameChanged(string newName)
        //{
        //    newName = newName.ToUpper();
        //    newName = newName.Insert(newName.Length, "Param");
        //    base.SetNameChanged(newName);
        //}
    }
}
