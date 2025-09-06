using UnityEngine;

namespace KHG.UI
{
    public class StatUI : ChangeableUI
    {
        [SerializeField] private GameObject statUI;

        protected override void OnHide()
        {
            if (statUI != null) statUI.SetActive(false);
        }

        protected override void OnShow()
        {
            if (statUI != null) statUI.SetActive(true);
        }
    }
}
