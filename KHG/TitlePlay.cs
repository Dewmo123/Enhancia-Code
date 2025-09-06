using Core.EventSystem;
using KHG.Events;
using System;
using System.Collections;
using System.Net.NetworkInformation;
using UnityEngine;

public class TitlePlay : MonoBehaviour
{
    [SerializeField] private EventChannelSO uiChannel,networkChannel;

    [SerializeField] private Animator playerAnimator;

    private void Start()
    {
        networkChannel.AddListener<LogInCallbackEvent>(HandleLoginEvent);
    }

    private void OnDestroy()
    {
        networkChannel.RemoveListener<LogInCallbackEvent>(HandleLoginEvent);
    }

    private void HandleLoginEvent(LogInCallbackEvent callback)
    {
        if(callback.success)
        {
            Debug.Log("qwer");
            ChangeScene();
        }
    }

    private void ChangeScene()
    {
        StartCoroutine(ChangeScene(3.5f));
        playerAnimator.SetTrigger("GameStart");
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
 