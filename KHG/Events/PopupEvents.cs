using Core.EventSystem;
using UnityEngine;

namespace KHG.Events
{
    public class PopupEvent
    {
        public static readonly SetPopupEvent SetPopupEvent = new();
    }
    public class SetPopupEvent : GameEvent//확인 버튼과 예 아니오 버튼이 있다.
    {
        public bool active;

        public string title;
        public Color titleColor;
        public string description;
        public Color descColor;

        public bool canChoose;//false일 경우 명령어를 넣지 않아도 됨
        public delegate void Commands();
        public Commands Positive;
        public Commands Negative;

        public Color buttonAcolor;
        public string buttonAtext;
        public Color buttonBcolor;
        public string buttonBtext;
    }
}
