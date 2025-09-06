using Core.EventSystem;
using Core.Managers;
using Core.Network;
using KHG.UI;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HG_Warner : MonoBehaviour
{
    [SerializeField] private EventChannelSO networkChannel;
    [SerializeField] protected TextMeshProUGUI _idField;
    [SerializeField] protected TextMeshProUGUI _pwField;

    [SerializeField] private TextMeshProUGUI _idWarnTMP;
    [SerializeField] private TextMeshProUGUI _pwWarnTMP;
    [SerializeField] private TextMeshProUGUI _LogInWarnTMP;

    [SerializeField] private LogInUI _logInUI;

    [SerializeField] private Button _accessBtn;

    public GameObject _bufferingUI;

    [Header("ID warning messeges")]
    [TextArea(2, 5)]
    [SerializeField] private string _idW1 = "id는 8글자를 초과할수 없습니다!";
    [TextArea(2, 5)]
    [SerializeField] private string _idW2 = "id는 영단어로만 이루어져야 합니다!";
    [TextArea(2, 5)]
    [SerializeField] private string _idW3 = "id에 공백을 포함할수 없습니다!";
    [Header("PW warning messeges")]
    [TextArea(2, 5)]
    [SerializeField] private string _pwW1 = "비밀번호는 영단어 대/소문자와 특수문자를 포함합니다!";
    [TextArea(2, 5)]
    [SerializeField] private string _pwW2 = "비밀번호는 8자 이상 24자 이하입니다!";
    [Header("LogIn warning messege")]
    [TextArea(2, 5)]
    [SerializeField] private string _LoginFailedMsg1 = "아이디 또는 비밀번호가 일치하지 않습니다.";
    [TextArea(2, 5)]
    [Header("SignUp Failed messege")]
    [SerializeField] private string _SignupFailedMsg1 = "이미 존재하는 아이디입니다.";
    [Header("SignUp success messege")]
    [TextArea(2, 5)]
    [SerializeField] private string _SignUpSuccessMsg = "회원가입에 성공했습니다. 로그인해주세요.";

    protected PlayerInfoNetworkController _networkController;

    private void Start()
    {
        networkChannel.AddListener<LogInCallbackEvent>(HandleLoginEvent);
        networkChannel.AddListener<SignUpCallbackEvent>(HandleSignupEvent);
        _networkController = NetworkConnector.Instance.GetController<PlayerInfoNetworkController>();
    }
    private void OnDestroy()
    {
        networkChannel.RemoveListener<LogInCallbackEvent>(HandleLoginEvent);
        networkChannel.RemoveListener<SignUpCallbackEvent>(HandleSignupEvent);
    }

    private void HandleSignupEvent(SignUpCallbackEvent result)
    {
        if (result.success)
        {
            _LogInWarnTMP.color = Color.green;
            SetString(_LogInWarnTMP, _SignUpSuccessMsg, 3f);
        }
        else
        {
            _LogInWarnTMP.color = Color.red;
            SetString(_LogInWarnTMP, _SignupFailedMsg1, 3f);
        }
        _bufferingUI.SetActive(false);
    }

    private void HandleLoginEvent(LogInCallbackEvent result)
    {
        if (result.success)
        {
            _logInUI.Close();
        }
        else
        {
            _LogInWarnTMP.color = Color.red;
            SetString(_LogInWarnTMP, _LoginFailedMsg1, 3f);
        }
        _bufferingUI.SetActive(false);
    }

    public void PwVerify()
    {
        string text = _pwField.text;

        if (text.Length < 8 && text.Length > 24)
        {
            VerifyFailed(_pwWarnTMP, _pwW2);
            return;
        }
        foreach (char character in text)
        {
            if (((character >= 33 && character <= 125) || character == 8203))
            {
                SetString(_pwWarnTMP, string.Empty);
            }
            else
            {
                print($"{character.ToString()}:{(int)character}");
                VerifyFailed(_pwWarnTMP, _pwW1);
                return;
            }
        }
        SetString(_idWarnTMP, string.Empty);
        VerifySuccess();
    }
    public void IdVerify()
    {
        string text = _idField.text;
        if (text.Length > 8)
        {
            VerifyFailed(_idWarnTMP, _idW1);
            return;
        }
        foreach (char character in text)
        {
            if (((character >= 48 && character <= 57) || (character >= 65 && character <= 90) || (character >= 97 && character <= 122)) || character == 8203)
            {
                SetString(_idWarnTMP, string.Empty);
            }
            else
            {
                print($"{character.ToString()}:{(int)character}");
                VerifyFailed(_idWarnTMP, _idW2);
                return;
            }
        }
        SetString(_pwWarnTMP, string.Empty);
        VerifySuccess();
    }

    private void VerifyFailed(TextMeshProUGUI target, string msg)
    {
        _accessBtn.interactable = false;
        SetString(target, msg);
    }

    private void VerifySuccess()
    {
        _accessBtn.interactable = true;
    }

    private void SetString(TextMeshProUGUI target, string msg, float destroyTime = 0)
    {
        target.text = msg;
        if (destroyTime != 0) StartCoroutine(ResetText(target, destroyTime));
    }

    private IEnumerator ResetText(TextMeshProUGUI target, float time)
    {
        yield return new WaitForSeconds(time);
        _LogInWarnTMP.color = Color.red;
        target.text = string.Empty;
    }
}
