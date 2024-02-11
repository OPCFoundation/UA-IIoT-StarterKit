using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace UnsDashboard.Server.Model
{
    public class Account
    {
        public int? Id { get; set; }

        public int? WpId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string CompanyName { get; set; }

        public MembershipType? MembershipType { get; set; }

        public DateTime? LastLoginTime { get; set; }

        [NotMapped]
        public string AccessToken { get; set; }
    }

    public enum MembershipType
    {
        NonMember = 0,
        Logo = 1,
        NonVoting = 2,
        EndUser = 3,
        Corporate = 4
    }

    public class UserInfo
    {
        [JsonPropertyName("ID")]
        public string Id { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("user_login")]
        public string UserLogin { get; set; }

        [JsonPropertyName("user_nicename")]
        public string UserNiceName { get; set; }

        [JsonPropertyName("user_email")]
        public string UserEmail { get; set; }

        [JsonPropertyName("user_registered")]
        public string UserRegistered { get; set; }

        [JsonPropertyName("user_status")]
        public string UserStatus { get; set; }

        [JsonPropertyName("spam")]
        public string IsSpam { get; set; }

        [JsonPropertyName("deleted")]
        public string IsDeleted { get; set; }

        [JsonPropertyName("real_pass")]
        public string RealPass { get; set; }

        [JsonPropertyName("is_logged_in")]
        public string IsLoggedIn { get; set; }

        [JsonPropertyName("company_id")]
        public int? CompanyId { get; set; }

        [JsonPropertyName("company_name")]
        public string CompanyName { get; set; }

        [JsonPropertyName("membership_type")]
        public int? MembershipType { get; set; }

        public static MembershipType GetMembershipType(int? membershipType)
        {
            switch (membershipType ?? 0)
            {
                case 1:
                case 2:
                case 3:
                case 4: return UnsDashboard.Server.Model.MembershipType.Corporate;
                case 5: return UnsDashboard.Server.Model.MembershipType.EndUser;
                case 6: return UnsDashboard.Server.Model.MembershipType.NonVoting;
                case 7: return UnsDashboard.Server.Model.MembershipType.Logo;
                default: return UnsDashboard.Server.Model.MembershipType.NonMember;
            }
        }
    }
}
