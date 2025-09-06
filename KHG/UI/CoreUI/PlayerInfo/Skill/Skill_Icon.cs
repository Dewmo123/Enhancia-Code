using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Skill_Icon : MonoBehaviour
{
    [SerializeField] private Image iconImg;
    [SerializeField] private Outline outline;
    [SerializeField] private TextMeshProUGUI chargeText;
    public void SetIcon(Sprite icon)
    {
        iconImg.sprite = icon;
    }

    public void SetCoolTime(float currentTime, float totalTime)
    {
        if (Mathf.Abs(totalTime - currentTime) < 2 * Time.deltaTime)
        {
            outline.enabled = true;
            iconImg.fillAmount = 1;
        }
        else
        {
            outline.enabled = false;
            iconImg.fillAmount = currentTime / totalTime;
        }
    }

    public void SetSkillCharge(int count)
    {
        if (count > 0)
        {
            chargeText.gameObject.SetActive(true);
            chargeText.text = count.ToString();
        }
        else
        {
            chargeText.gameObject.SetActive(false);
        }
    }
}
