using ServerCode.DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace AKH.UI.Auctions
{
    public class AuctionItemUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemNameTxt;
        [SerializeField] private TextMeshProUGUI itemPriceTxt;
        [SerializeField] private TextMeshProUGUI itemQuantityTxt;
        [SerializeField] private Image itemImage;
        private AuctionItemDTO _item;

        public void SetUI(Sprite itemImage, AuctionItemDTO itemDTO)
        {
            _item = itemDTO;
            itemNameTxt.text = _item.itemName;
            itemPriceTxt.text = _item.pricePerUnit.ToString();
            itemQuantityTxt.text = _item.quantity.ToString();
            this.itemImage.sprite = itemImage;
        }
        public void ResetUI()
        {
            itemNameTxt.text = null;
            itemPriceTxt.text = null;
            itemQuantityTxt.text = null;
            itemImage.sprite = null;
        }
    }
}