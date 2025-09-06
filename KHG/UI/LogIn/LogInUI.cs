using DG.Tweening;
using UnityEngine;

namespace KHG.UI
{
    public class LogInUI : ChangeableUI
    {
        [SerializeField] private GameObject uiMain;

        private RectTransform uiRect;

        private void Awake()
        {
            uiRect = uiMain.GetComponent<RectTransform>();
        }

        protected override void OnShow()
        {
            print(("LOGIN_OPEN"));
            uiMain.SetActive(true);
        }

        protected override void OnHide()
        {
            Close();
        }

        public void Close()
        {
            print(("LOGIN_CLOSE"));
            uiMain.SetActive(false);
        }
    }
}
