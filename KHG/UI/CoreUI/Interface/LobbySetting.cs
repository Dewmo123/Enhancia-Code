using Core.EventSystem;
using KHG.Events;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class LobbySetting : ChangeableUI
{
    [SerializeField] private EventChannelSO uiChannel;
    [Header("UI Components")]
    [SerializeField] private GameObject settingUI;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    // PlayerPrefs Keys
    private const string MASTER_KEY = "Volume_Master";
    private const string BGM_KEY = "Volume_BGM";
    private const string SFX_KEY = "Volume_SFX";

    private void OnEnable()
    {
        LoadVolumes();

        // Add listeners
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }
    protected override void OnShow()
    {
        settingUI.SetActive(true);
        SetSlide();
    }

    protected override void OnHide()
    {
        settingUI.SetActive(false);
    }

    private void LoadVolumes()
    {
        float master = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float bgm = PlayerPrefs.GetFloat(BGM_KEY, 0.5f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 0.5f);

        SetMasterVolume(master);
        SetBGMVolume(bgm);
        SetSFXVolume(sfx);
    }
    private void SetSlide()
    {
        float master = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        float bgm = PlayerPrefs.GetFloat(BGM_KEY, 0.5f);
        float sfx = PlayerPrefs.GetFloat(SFX_KEY, 0.5f);

        masterSlider.value = master;
        bgmSlider.value = bgm;
        sfxSlider.value = sfx;
    }

    public void SetMasterVolume(float value)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat(MASTER_KEY, value);
    }

    public void SetBGMVolume(float value)
    {
        audioMixer.SetFloat("BGM", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat(BGM_KEY, value);
    }

    public void SetSFXVolume(float value)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f);
        PlayerPrefs.SetFloat(SFX_KEY, value);
    }

    public void OnConfirmButton()
    {
        PlayerPrefs.Save(); // 저장 명시적으로 실행
        settingUI.SetActive(false);
    }

    public void OnExitButton()
    {
        SceneChangeEvent evt = UIEvents.SceneChangeEvent;
        evt.sceneName = "TitleScene";
        evt.targetState = false;
        evt.useSceneChange = true;
        evt.changeSpeed = 0.3f;

        uiChannel.InvokeEvent(evt);
    }
}

