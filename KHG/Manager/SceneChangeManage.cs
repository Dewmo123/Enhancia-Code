using Core.EventSystem;
using KHG.Events;
using UnityEngine;

public class SceneChangeManage : MonoBehaviour
{
    [SerializeField] private EventChannelSO uiChannel;
    [Header("Settings")]
    [SerializeField] private bool _state = true;// true = FadeIn, false = FadeOut
    [SerializeField] private float _changeSpeed = 1f;
    [SerializeField] private bool _playOnAwake = true;
    [SerializeField] private string _sceneName = "MainMenu";

    private void Start()
    {
        if(_playOnAwake) Active();
    }

    public void Active()
    {
        SceneChangeEvent evt = UIEvents.SceneChangeEvent;
        evt.targetState = _state;
        evt.changeSpeed = _changeSpeed;
        evt.sceneName = _sceneName;
        uiChannel.InvokeEvent(evt);
    }
}
