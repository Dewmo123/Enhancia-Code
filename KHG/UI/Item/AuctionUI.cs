using UnityEngine;

namespace KHG.UI
{
    public class AuctionUI : ChangeableUI
    {
        [SerializeField] private GameObject auction;

        protected override void OnShow()
        {
            auction.SetActive(true);
        }

        protected override void OnHide()
        {
            auction.SetActive(false);
        }
    }
}
