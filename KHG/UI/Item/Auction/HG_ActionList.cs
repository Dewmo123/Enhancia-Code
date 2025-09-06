using Core.EventSystem;
using Core.Managers;
using UnityEngine;

public class HG_ActionList : MonoBehaviour
{
    [SerializeField] private EventChannelSO networkChannel;
    void OnEnable()
    {
        networkChannel.AddListener<GetItemsByIdCallbackEvent>(GetItem);
    }

    private void GetItem(GetItemsByIdCallbackEvent value)
    {
        int uiCount = 0;
        foreach (var item in value.results)
        {

        }
    }

    void Update()
    {
        
    }
}
