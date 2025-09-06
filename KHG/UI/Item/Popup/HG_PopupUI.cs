using Core.EventSystem;
using KHG.Events;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HG_PopupUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI subtitle;
    [SerializeField] private Button buttonA;
    [SerializeField] private TextMeshProUGUI btnAText;
    [SerializeField] private Button buttonB;
    [SerializeField] private TextMeshProUGUI btnBText;
    [SerializeField] private GameObject popup;

    [SerializeField] private EventChannelSO uiChannel;

    private void Awake()
    {
        uiChannel.AddListener<SetPopupEvent>(SetPopupUI);
        SetPopuoUIDefault();
    }
    private void OnDestroy()
    {
        uiChannel.RemoveListener<SetPopupEvent>(SetPopupUI);
    }
    private void SetPopupUI(SetPopupEvent evt)
    {
        SetPanel(evt.title, evt.description, evt.titleColor, evt.descColor, evt.active);
        if (evt.canChoose == true)
        {
            buttonA.gameObject.SetActive(true);
            buttonB.gameObject.SetActive(true);

            buttonA.onClick.AddListener(()=>evt.Positive?.Invoke());
            buttonB.onClick.AddListener(()=>evt.Negative?.Invoke());

            btnAText.text = evt.buttonAtext;
            btnAText.color = evt.buttonAcolor;

            btnBText.text = evt.buttonBtext;
            btnBText.color = evt.buttonBcolor;
        }
        else
        {
            buttonA.gameObject.SetActive(true);
            buttonB.gameObject.SetActive(false);

            btnAText.text = evt.buttonAtext;
            btnAText.color = evt.buttonAcolor;

            buttonA.onClick.AddListener(() => ConfirmedPressed());
        }
    }

    private void SetPopuoUIDefault()//기본 버튼 설정
    {
        buttonA.gameObject.SetActive(true);//button을 하나로 설정
        buttonB.gameObject.SetActive(false);

        title.text = "오류";
        subtitle.text = "알수 없는 오류...";

        buttonA.onClick.AddListener(()=>ConfirmedPressed());//그 하나의 버튼에 확인 기능을 추가
    }

    public void SetPanel(string titleMsg, string subtitleMsg, Color titleColor, Color subtitleColor, bool isVisible = false)
    {
        if (IsInvalidInput(titleMsg, subtitleMsg)) return;

        ApplyText(title, titleMsg, titleColor);
        ApplyText(subtitle, subtitleMsg, subtitleColor);

        SetPopupVisibility(isVisible);
    }

    public void SetPanelText(string titleMsg, string subtitleMsg, bool isVisible = false)
    {
        if (IsInvalidInput(titleMsg, subtitleMsg)) return;

        ApplyText(title, titleMsg);
        ApplyText(subtitle, subtitleMsg);

        SetPopupVisibility(isVisible);
    }

    public void ConfirmedPressed()
    {
        SetPopupVisibility(false);
    }

    private bool IsInvalidInput(string titleMsg, string subtitleMsg)
    {
        return string.IsNullOrEmpty(titleMsg) || string.IsNullOrEmpty(subtitleMsg);
    }

    private void ApplyText(TextMeshProUGUI target, string message, Color? color = null)
    {
        target.text = message;
        if (color.HasValue)
            target.color = color.Value;
    }

    private void SetPopupVisibility(bool isVisible)
    {
        popup.SetActive(isVisible);
    }
}

public enum PUPUP_MODE
{
    Ok,
    YesOrNo,

}