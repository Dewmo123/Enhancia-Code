using Core.EventSystem;
using KHG.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HG_ItemDisplayUI : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private Sprite defaultIcon;
    [SerializeField] private Slider quantitySlider;
    [SerializeField] private Button actionButton;
    [SerializeField] private TMP_InputField priceInput;
    [SerializeField] private TextMeshProUGUI actionText;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemDesc;
    [SerializeField] private TextMeshProUGUI playerIdText;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private EventChannelSO uiChannel;
    [SerializeField] private EventChannelSO netChannel;
    [SerializeField] private HG_PopupUI popupUI;

    private ItemDataSO currentItem;
    private int buyAmount = 1;
    private int unitPrice = 1;
    private ItemSelectEvent.SelectCommand _currentCommand;
    private void Awake()
    {
        uiChannel.AddListener<ItemSelectEvent>(OnItemSelected);
        netChannel.AddListener<PostAuctionItemCallbackEvent>(OnUploadCallback);
        netChannel.AddListener<PurchaseAuctionItemCallbackEvent>(OnPurchaseCallback);
        actionButton.onClick.AddListener(PlayCommand);
        Debug.Log("AwAKE");
    }

    private void OnEnable()
    {
        ResetDisplay();
    }

    private void OnDestroy()
    {
        Debug.Log("DESTORY");
        actionButton.onClick.RemoveListener(PlayCommand);
        uiChannel.RemoveListener<ItemSelectEvent>(OnItemSelected);
        netChannel.RemoveListener<PostAuctionItemCallbackEvent>(OnUploadCallback);
        netChannel.RemoveListener<PurchaseAuctionItemCallbackEvent>(OnPurchaseCallback);
    }

    private void OnItemSelected(ItemSelectEvent evt)
    {
        SetDisplay(evt);
    }

    public void SetDisplay(ItemSelectEvent evt)
    {
        currentItem = evt.item;
        itemImage.sprite = currentItem.Icon;
        itemName.text = currentItem.Name;
        itemDesc.text = currentItem.Description;
        actionText.text = evt.buttonName;
        quantitySlider.maxValue = evt.quantity;
        quantitySlider.value = 1; // 슬라이더 시작 값을 1로 설정
        SetBuyCount(); // 초기 수량 텍스트 갱신

        playerIdText.text = evt.playerId;
        quantityText.text = $"{evt.quantity}개";
        priceText.text = $"{evt.price}골드";

        priceInput.gameObject.SetActive(evt.usingInput);
        quantitySlider.gameObject.SetActive(evt.usingSlider);
        priceText.gameObject.SetActive(evt.needPrice);

        // 슬라이더 값이 변경될 때마다 수량 갱신
        quantitySlider.onValueChanged.RemoveAllListeners();
        quantitySlider.onValueChanged.AddListener(_ => SetBuyCount());

        // 가격 입력 필드도 입력 변경 시 가격 갱신
        priceInput.onValueChanged.RemoveAllListeners();
        priceInput.onValueChanged.AddListener(_ => SetPriceCount());
        _currentCommand = evt.command;
        if (!actionButton.enabled)
        {
            actionButton.enabled = true;
        }
    }

    private void PlayCommand()
    {
        Debug.Log("QWE");
        _currentCommand.Invoke(buyAmount, unitPrice);
        uiChannel.InvokeEvent(new RefreshUIEvent());
    }

    public void ResetDisplay()
    {
        itemImage.sprite = defaultIcon;
        itemName.text = "---";
        itemDesc.text = "---";
        quantityText.text = "---";
        priceText.text = "---";
        playerIdText.text = "---";
        actionText.text = "X_X";

        priceInput.gameObject.SetActive(false);
        quantitySlider.gameObject.SetActive(false);
        priceText.gameObject.SetActive(false);

        actionButton.enabled = false;
    }

    public void SetBuyCount()
    {
        buyAmount = (int)quantitySlider.value;
        quantityText.text = $"{buyAmount}개";
    }

    public void SetPriceCount()
    {
        if (int.TryParse(priceInput.text, out int result))
        {
            unitPrice = result;
        }
        else
        {
            unitPrice = 0;
        }
    }

    private void OnUploadCallback(PostAuctionItemCallbackEvent evt)
    {
        SetPopupEvent pEvt = new SetPopupEvent
        {
            active = true,
            canChoose = false,
            title = evt.success ? "등록 성공" : "업로드 실패",
            description = evt.success ? "해당 아이템이 성공적으로 등록 되었습니다." : "아이템 업로드에 실패했습니다.\n나중에 다시 시도해주세요.",
            titleColor = evt.success ? Color.green : Color.red,
            buttonAtext = "확인",
            descColor = Color.gray,
            buttonAcolor = Color.black
        };
        uiChannel.InvokeEvent(pEvt);
    }

    private void OnPurchaseCallback(PurchaseAuctionItemCallbackEvent evt)
    {
        SetPopupEvent pEvt = new SetPopupEvent
        {
            active = true,
            canChoose = false,
            title = evt.success ? "구매 성공" : "구매 실패",
            description = evt.success ? "아이템이 인벤토리에 추가되었습니다." : "아이템 구매에 실패했습니다.\n잔액, 구매 수량, 사용자를 확인해주세요.",
            titleColor = evt.success ? Color.green : Color.red,
            buttonAtext = "확인",
            descColor = Color.gray,
            buttonAcolor = Color.black
        };
        uiChannel.InvokeEvent(pEvt);
    }
}
