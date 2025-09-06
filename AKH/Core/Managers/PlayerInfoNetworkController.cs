using Core.EventSystem;
using Core.Network;
using ServerCode.DTO;

namespace Core.Managers
{
    public class PlayerInfoNetworkController : NetworkController
    {
        public PlayerInfoNetworkController(WebClient client, EventChannelSO eventChannel) : base(client, eventChannel)
        {
        }
        public void LogIn(string id, string password)
        {
            var playerInfo = new PlayerDTO() { id = id, password = password };
            var callbackEvent = NetworkEvents.LogInCallback;
            _webClient.SendPostRequest("player-info/log-in",
                playerInfo,
                val =>
                {
                    if (val)
                        callbackEvent.playerId = id;
                    callbackEvent.success = val;
                    _networkChannel.InvokeEvent(callbackEvent);
                });
        }
        public void SignUp(string id, string password)
        {
            var playerInfo = new PlayerDTO() { id = id, password = password };
            var callbackEvent = NetworkEvents.SignUpCallback;
            _webClient.SendPostRequest("player-info/sign-up",
                playerInfo,
                val =>
                {
                    callbackEvent.success = val;
                    _networkChannel.InvokeEvent(callbackEvent);
                });
        }
    }
}
