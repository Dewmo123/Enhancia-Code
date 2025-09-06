using Core.EventSystem;
using DG.Tweening;
using KHG.Events;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private EventChannelSO uiChannel;

    [SerializeField] private Image background;
    //public event Action<bool> OnSceneStateChanged;

    private void Awake()
    {
        uiChannel.AddListener<SceneChangeEvent>(OnSceneChange);
    }

    private void OnDestroy()
    {
        uiChannel.RemoveListener<SceneChangeEvent>(OnSceneChange);
    }

    private void OnSceneChange(SceneChangeEvent callback)
    {
        Debug.Log("asd");
        float spd = callback.changeSpeed;
        bool state = callback.targetState;
        print("Scene State : " + state);
        if(callback.useSceneChange == false)
        {
            SetUI(state, spd, null);
            return;
        }
        Action endAction = () => SceneManager.LoadScene(callback.sceneName);
        SetUI(state, spd, endAction);

        SceneChangeEvent evt = UIEvents.SceneChangeEvent;
        evt.useSceneChange = true;
        //OnSceneStateChanged?.Invoke(state);
    }
    private void SetUI(bool state, float spd, Action endAction)
    {
        SetBackgrond(true);
        int alphaValue = state ? 0 : 1;
        int rA = state ? 1 : 0;
        background.color = new Color(background.color.r, background.color.g, background.color.b, rA);
        background.DOFade(alphaValue, spd).OnComplete(() =>
        {
            if (state == false) endAction?.Invoke();
            else SetBackgrond(false);
        });
    }
    private void SetBackgrond(bool value)
    {
        background.gameObject.SetActive(value);
    }
}
