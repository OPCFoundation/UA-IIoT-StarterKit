using System.Text;
using System.Text.Json;

namespace UnsDashboard.Server.Model
{
    public class WebSession
    {
        public string Context { get; set; }

        public int? UserId { get; set; }

        public string UserEmail { get; set; }

        public string UserName { get; set; }

        public string CompanyName { get; set; }

        public MembershipType? MembershipType { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime? ExpiresBy { get; set; }

        public byte[] ToBytes()
        {
            string json = JsonSerializer.Serialize(this);
            return new UTF8Encoding(false).GetBytes(json);
        }

        public static WebSession FromBytes(byte[] bytes)
        {
            var json = new UTF8Encoding(false).GetString(bytes);
            return JsonSerializer.Deserialize<WebSession>(json);
        }
    }
}
