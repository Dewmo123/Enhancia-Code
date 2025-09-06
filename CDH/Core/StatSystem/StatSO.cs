using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace _00.Work.CDH.Code.Core.StatSystem
{
    [CreateAssetMenu(fileName = "StatSO", menuName = "SO/StatSystem/Stat")]
    public class StatSO : ScriptableObject, ICloneable
    {
        public delegate void ValueChangeHandler(StatSO stat, float current, float previous);
        public event ValueChangeHandler OnValueChange;

        [Tooltip("<color=red>이름 구조</color> : 전부 영소문자로 작성합니다. 띄어쓰기가 필요한 경우 언더바(_)를 사용합니다." +
            "\n<color=green>예 : jump_count</color>처럼 작성합니다.\n" +
            "<color=red>명명 규칙</color> :\n" +
            "단어는 최대한 짧게 사용합니다.\n" +
            "최대한 2개를 넘지 않게 하세요.\n" +
            "모든 단어는 명사를 사용하세요.\n" +
            "뒤에 stat이 붙지 않습니다.\n" +
            "불변 스텟은 (NONE_)을 앞에 붙이세요." +
            "\n<color=green>예 : NONE_jump_count</color>")]
        public string statName;
        [TextArea]
        public string description;

        [SerializeField] private Sprite icon;
        [SerializeField] private string displayName;
        [SerializeField] private float baseValue, minValue, maxValue;

        private Dictionary<object, float> _modifyDictionary = new Dictionary<object, float>();

        [field : SerializeField] public bool IsPercent { get; private set; }

        private float _modifiedValue = 0;

        #region Property section

        public Sprite Icon => icon;
        public float MaxValue
        {
            get => maxValue;
            set => maxValue = value;
        }
        public float MinValue
        {
            get => minValue;
            set => minValue = value;
        }
        public float Value => Mathf.Clamp(baseValue + _modifiedValue, MinValue, MaxValue);
        public bool IsMax => Mathf.Approximately(Value, MaxValue);
        public bool IsMin => Mathf.Approximately(Value, MinValue);

        public float BaseValue
        {
            get => baseValue;
            set
            {
                float previousValue = value;
                baseValue = Mathf.Clamp(previousValue, minValue, maxValue);
                TryInvokeValueChangeEvent(Value, previousValue); 
            }
        }

        #endregion

        public void AddModifier(object key, float value)
        {
            if (_modifyDictionary.ContainsKey(key)) return;
            float prevValue = Value; //이전값을 기억해 놨다가

            _modifiedValue += value;
            _modifyDictionary.Add(key, value);

            TryInvokeValueChangeEvent(Value, prevValue);
        }

        public void RemoveModifier(object key)
        {
            if(_modifyDictionary.TryGetValue(key, out float value))
            {
                float prevValue = Value;
                _modifiedValue -= value;
                _modifyDictionary.Remove(key);

                TryInvokeValueChangeEvent(Value, prevValue);
            }
        }

        public void ClearAllModifier()
        {
            float prevValue = Value;
            _modifyDictionary.Clear();
            _modifiedValue = 0;
            TryInvokeValueChangeEvent(Value, prevValue);
        }

        private void TryInvokeValueChangeEvent(float current, float previousValue)
        {
            //이전값과 일치하지 않으면 이벤트 인보크
            if (Mathf.Approximately(current, previousValue) == false)
            {
                OnValueChange?.Invoke(this, current, previousValue);
            }
        }

        public object Clone() => Instantiate(this);
    }
}
