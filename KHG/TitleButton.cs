using Core.EventSystem;
using DG.Tweening.Core.Easing;
using KHG.Events;
using System.Collections;
using UnityEngine;

namespace KHG.Title
{
    public class TitleButton : MonoBehaviour
    {
        [SerializeField] private Animator settingAnimator;
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private Animator parentUI;

        [SerializeField] private EventChannelSO uiChannel;

        private bool _isSettingOpen;
        private bool _canInteract = true;

        private void Start()
        {
            if(parentUI)parentUI.Play("StartTitle");
        }
        public void OnPlayClicked()
        {
            if(_canInteract == false) return;
            _canInteract = false;
            if(_isSettingOpen)settingAnimator.Play("SettingClose");
            parentUI.Play("EndTitle");
        }
        public void OnSettingClicked()
        {
            if (_canInteract == false) return;
            _isSettingOpen = !_isSettingOpen;
            if (_isSettingOpen) settingAnimator.Play("SettingOpen");
            else settingAnimator.Play("SettingClose");
        }

        public void OnQuitClicked()
        {
            Application.Quit();
        }

        private IEnumerator ChangeScene(float time)
        {
            yield return new WaitForSeconds(time);
            SceneChangeEvent changeEvt = new SceneChangeEvent();
            changeEvt.sceneName = "LobbyScene";
            changeEvt.targetState = false;
            changeEvt.changeSpeed = 0f;
            uiChannel.InvokeEvent(changeEvt);
        }
    }
}
