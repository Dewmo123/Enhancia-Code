using Core.EventSystem;
using KHG.Events;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public EventChannelSO UIChannel;

    public void SetUIChannel(EventChannelSO channel)
    {
        UIChannel = channel;
    }
}
