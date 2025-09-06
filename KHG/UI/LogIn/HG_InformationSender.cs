using Core.Managers;
using Core.Network;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
//테스트
public class HG_InformationSender : HG_Warner
{
    public void TryLogIn()
    {
        var id = _idField.text.Trim().Replace("\u200B", "");
        string pw = _pwField.text.Trim().Replace("\u200B", "");
        _networkController.LogIn(id, pw);
        _bufferingUI.SetActive(true);
    }   
    public void TrySignUp()
    {
        var id = _idField.text.Trim().Replace("\u200B", "");
        string pw = _pwField.text.Trim().Replace("\u200B", "");
        _networkController.SignUp(id, pw);
        _bufferingUI.SetActive(true);
    }
}
