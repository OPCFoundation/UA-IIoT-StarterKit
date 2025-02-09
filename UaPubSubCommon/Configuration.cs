using Opc.Ua;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace UaPubSubCommon
{
    public class Configuration
    {
        public string BrokerHost { get; set; }
        public int BrokerPort { get; set; }
        public bool UseTls { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TopicPrefix { get; set; }
        public string PublisherId { get; set; }
        public bool UseNewEncodings { get; set; }
        public ApplicationDescription ApplicationDescription { get; set; }
        
        public static string GetPrivateHostName()
        {
            // hash the local host name to avoid leaking private information.
            var encoding = new UTF8Encoding(false);
            var bytes = encoding.GetBytes(Dns.GetHostName());

            using (var sha = SHA256.Create())
            {
                using (var strm = new MemoryStream(bytes))
                {
                    var hash = sha.ComputeHash(strm);
                    return Convert.ToHexString(hash).Substring(0, 10).ToLowerInvariant();
                }
            }
        }
    }
}
