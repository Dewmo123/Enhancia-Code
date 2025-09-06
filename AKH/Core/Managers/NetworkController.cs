using Core.EventSystem;
using Core.Network;

namespace Core.Managers
{
    public abstract class NetworkController
    {
        protected EventChannelSO _networkChannel;
        protected WebClient _webClient;
        public NetworkController(WebClient client, EventChannelSO channel)
        {
            _webClient = client;
            _networkChannel = channel;
        }
    }
}
