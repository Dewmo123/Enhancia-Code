using System;
using UnityEngine;

namespace _00.Work.CDH.Code.Combat
{
    [CreateAssetMenu(fileName = "AttackData", menuName = "SO/Combat/AttackData", order = 0)]
    public class AttackDataSO : ScriptableObject
    {
        [Tooltip("<color=red>이름 구조</color> : 전부 영소문자로 작성합니다. 띄어쓰기가 필요한 경우 언더바(_)를 사용합니다." +
            "\n<color=green>예 : jump_attack</color>처럼 작성합니다.\n" +
            "<color=red>명명 규칙</color> :\n" +
            "단어는 최대한 짧게 사용합니다.\n" +
            "최대한 2개를 넘지 않게 하세요.\n" +
            "모든 단어는 명사를 사용하세요.\n" +
            "숫자도 _바를 입력해 구분합니다.\n" +
            "가독성을 위해 단어 뒤에 attack을 붙입니다.\n" +
            "<color=green>예 : jump_attack</color>처럼 작성합니다.")]
        public string attackName;
        public Vector2 movement;
        public Vector2 knockBackForce;
        public float damageMultiplier = 1f;
        public float damageIncrease = 0f;
        public bool isPowerAttack;

        public float cameraShakePower;
        public float cameraShakeDuration;

        private void OnEnable()
        {
            attackName = this.name;
        }
    }
}