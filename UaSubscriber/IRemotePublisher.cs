using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using System.Text;
using UaPubSubCommon;

namespace UaSubscriber
{
    public interface IRemotePublisher
    {
        string PublisherId { get; set; }

        Dictionary<string, bool> GetTopics();

        Task<bool> ProcessMessage(string topic, string json, CancellationToken ct);
    }
}
