using Core.EventSystem;
using Core.Network;
using DG.Tweening;
using ServerCode.DTO;

namespace Core.Managers
{
    public class PlayerDataNetworkController : NetworkController
    {
        public PlayerDataNetworkController(WebClient client, EventChannelSO eventChannel) : base(client, eventChannel)
        {
        }
        public void UpgradeDictionary(DictionaryUpgradeDTO dto, int level)
        {
            var callback = NetworkEvents.UpgradeDictionaryCallbackEvent;
            _webClient.SendPatchRequest(
                "player-data/upgrade-dictionary",
                dto,
                val =>
                {
                    callback.upgradeDic = dto;
                    callback.success = val;
                    callback.level = level;
                    _networkChannel.InvokeEvent(callback);
                });
        }
        public void UpgradeEquip(EquipType equipType)
        {
            var callback = NetworkEvents.UpgradeEquipCallbackEvent;
            _webClient.SendPatchRequest(
                "player-data/upgrade-equipment",
                equipType,
                val =>
                {
                    callback.equipType = equipType;
                    callback.success = val;
                    _networkChannel.InvokeEvent(callback);
                });
        }
        public void GetMyData()
        {
            var callback = NetworkEvents.GetMyDataCallback;
            _webClient.SendGetRequest<PlayerDataDTO>(
                "player-data/get-my-data",
                val =>
                {
                    callback.result = val;
                    _networkChannel.InvokeEvent(callback);
                });
        }
        public void StageEnd(StageEndDTO stageEndDTO)
        {
            _webClient.SendPatchRequest(
                "player-data/stage-end",
                stageEndDTO,
                val =>
                {

                });
        }
    }
}
