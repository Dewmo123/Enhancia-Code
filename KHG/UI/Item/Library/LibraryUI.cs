using UnityEngine;

namespace KHG.UI
{
    public class LibraryUI : ChangeableUI
    {
        private Animator _animator;
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        protected override void OnHide()
        {
            _animator.SetBool("enable", false);
        }

        protected override void OnShow()
        {
            _animator.SetBool("enable", true);
        }
    }
}
