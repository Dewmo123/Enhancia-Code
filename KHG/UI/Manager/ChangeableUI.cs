using UnityEngine;

public abstract class ChangeableUI : MonoBehaviour
{
    public virtual void Show()
    {
        OnShow();
    }

    public virtual void Hide()
    {
        OnHide();
    }

    protected abstract void OnShow();
    protected abstract void OnHide();
}
