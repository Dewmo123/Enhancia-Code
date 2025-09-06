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
        quantitySlider.value = 1; // �����̴� ���� ���� 1�� ����
        SetBuyCount(); // �ʱ� ���� �ؽ�Ʈ ����

        playerIdText.text = evt.playerId;
        quantityText.text = $"{evt.quantity}��";
        priceText.text = $"{evt.price}���";

        priceInput.gameObject.SetActive(evt.usingInput);
        quantitySlider.gameObject.SetActive(evt.usingSlider);
        priceText.gameObject.SetActive(evt.needPrice);

        // �����̴� ���� ����� ������ ���� ����
        quantitySlider.onValueChanged.RemoveAllListeners();
        quantitySlider.onValueChanged.AddListener(_ => SetBuyCount());

        // ���� �Է� �ʵ嵵 �Է� ���� �� ���� ����
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
        quantityText.text = $"{buyAmount}��";
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
            title = evt.success ? "��� ����" : "���ε� ����",
            description = evt.success ? "�ش� �������� ���������� ��� �Ǿ����ϴ�." : "������ ���ε忡 �����߽��ϴ�.\n���߿� �ٽ� �õ����ּ���.",
            titleColor = evt.success ? Color.green : Color.red,
            buttonAtext = "Ȯ��",
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
            title = evt.success ? "���� ����" : "���� ����",
            description = evt.success ? "�������� �κ��丮�� �߰��Ǿ����ϴ�." : "������ ���ſ� �����߽��ϴ�.\n�ܾ�, ���� ����, ����ڸ� Ȯ�����ּ���.",
            titleColor = evt.success ? Color.green : Color.red,
            buttonAtext = "Ȯ��",
            descColor = Color.gray,
            buttonAcolor = Color.black
        };
        uiChannel.InvokeEvent(pEvt);
    }
}
