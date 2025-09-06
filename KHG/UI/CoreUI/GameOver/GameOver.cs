using _00.Work.CDH.Code.Players;
using Assets._00.Work.YHB.Scripts.Core;
using Core.EventSystem;
using DG.Tweening;
using KHG.Events;
using Players.Inven;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KHG.UI
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private GameObject button;

        [SerializeField] private TextMeshProUGUI title;

        [SerializeField] private EventChannelSO invenChannel, uiChannel;

        [Header("UI")]
        [SerializeField] private GameObject invenDataUIPrefab;
        [SerializeField] private Transform resultItemParent;
        [SerializeField] private ObjectFinderSO playerFinder;
        
        private void Start()
        {
            ResetUI();
            invenChannel.AddListener<UpdateInventoryEvent>(HandleInventoryResult);
            playerFinder.GetObject<Player>().OnDead.AddListener(PlayerGameOver);
        }

        private void OnDestroy()
        {
            invenChannel.RemoveListener<UpdateInventoryEvent>(HandleInventoryResult);
        }
        [ContextMenu(itemName:"Test")]
        public void PlayerGameOver()    
        {
            playerFinder.GetObject<Player>().gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            title.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
            title.DOFade(1, 1f).OnComplete(() =>
            {
                title.transform.DOMoveY(transform.position.y + 300, 1f).OnComplete(() =>
                {
                    title.DOFade(0.8f, 1f).OnComplete(() =>
                    {
                        resultPanel.SetActive(true);
                        SetInventoryResult();
                    }); 
                });
            });
        }

        private void ResetUI()
        {
            gameOverPanel.SetActive(false);
            resultPanel.SetActive(false);
            button.SetActive(false);
            int rCnt = resultItemParent.childCount;
            for (int i = 0;i<rCnt;i++)
            {
                Destroy(resultItemParent.GetChild(i).gameObject);
            }
            title.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
        }
        private void SetInventoryResult()
        {
            invenChannel.InvokeEvent(new RequestInventoryEvent());
        }

        private void HandleInventoryResult(UpdateInventoryEvent callback)
        {
            int cnt = 5;
            callback.inven.ForEach((item) =>
            {
                if(item.stackSize > 0)
                {
                    cnt++;
                    SetInvenDataUI(item.data, item.stackSize, cnt);
                }
            });

            StartCoroutine(DelayAction(cnt * 0.1f + 0.5f,()=> SetButton()));
        }

        private void SetInvenDataUI(ItemDataSO data,int count,int cnt)
        {
            GameObject ui = Instantiate(invenDataUIPrefab, resultItemParent);
            ui.SetActive(false);
            ui.GetComponent<ResultItemUI>().SetData(data, count);
            StartCoroutine(SetEnable(ui, true, cnt * 0.1f));
        }

        private IEnumerator SetEnable(GameObject obj, bool value, float time)
        {
            yield return new WaitForSeconds(time);
            obj.SetActive(value);
        }

        private IEnumerator DelayAction(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action?.Invoke();
        }

        private void SetButton()
        {
            button.SetActive(true);
        }
        public void ReturnToLobby()
        {
            button.GetComponent<Button>().interactable = false;
            uiChannel.InvokeEvent(new SceneChangeEvent { sceneName = "LobbyScene", targetState = false, changeSpeed = 1.5f });
            playerFinder.Object.GetComponentInChildren<PlayerInGameInvenData>().SendInvenToServer();
        }
    }
}
