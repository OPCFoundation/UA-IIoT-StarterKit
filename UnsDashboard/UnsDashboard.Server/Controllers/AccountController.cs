using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using UnsDashboard.Server.Model;

namespace UnsDashboard.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(NoStore = true)]
    public class AccountController : CommonController
    {
        private string ClientId = null;
        private string ClientSecret = null;
        private string BaseUrl = null;

        private string AuthorizeUrl = "{0}/oauth/authorize/";
        private string RevokeUrl = "{0}/oauth/revoke/";
        private string DestroyUrl = "{0}/oauth/destroy/";
        private string TokenUrl = "{0}/oauth/token/";
        private string MeUrl = "{0}/oauth/me/";

        public AccountController(
            IConfiguration configuration,
            ILogger<AccountController> logger,
            DatabaseContext context)
        :
            base(configuration, logger, context)
        {
            BaseUrl = Configuration.GetValue<string>("OAuth2BaseUrl");
            ClientId = Configuration.GetValue<string>("OAuth2ClientId");
            ClientSecret = Configuration.GetValue<string>("OAuth2ClientSecret");
            AuthorizeUrl = String.Format(AuthorizeUrl, BaseUrl);
            RevokeUrl = String.Format(RevokeUrl, BaseUrl);
            DestroyUrl = String.Format(DestroyUrl, BaseUrl);
            TokenUrl = String.Format(TokenUrl, BaseUrl);
            MeUrl = String.Format(MeUrl, BaseUrl);
        }

        [HttpGet()]
        [Route("login")]
        [ResponseCache(NoStore = true)]
        public IActionResult Login(string context = null)
        {
            if (context != null && context.StartsWith("/login", StringComparison.OrdinalIgnoreCase))
            {
                context = "/";
            }

            LoadWebSession();
            Session.Context = context;
            SaveWebSession();

            var url = $"{AuthorizeUrl}?client_id={ClientId}&response_type=code&state={Guid.NewGuid()}";

            Logger.LogInformation($"Login: {url}");
            return RedirectPermanent(url);
        }

        [HttpGet()]
        [Route("logout")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Logout(string context = null)
        {
            LoadWebSession();

            if (Session?.AccessToken != null)
            {
                await CancelToken(Session);
                SaveWebSession();
            }

            var callbackUrl = $"{((String.IsNullOrEmpty(context)) ? '/' : context)}";
            return Redirect($"{DestroyUrl}?post_logout_redirect_uri={Request.Scheme}://{Request.Host}{callbackUrl}");
        }

        [HttpGet()]
        [Route("current")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Current()
        {
            LoadWebSession();

            try
            {
                if (Session == null || Session.AccessToken == null)
                {
                    throw new ApiResponseException(nameof(ErrorCodes.NotLoggedIn), ErrorCodes.NotLoggedIn);
                }

                if (Session.ExpiresBy == null || Session.ExpiresBy < DateTime.UtcNow)
                {
                    await RefreshToken(Session);
                    SaveWebSession();
                }

                var account = await (
                    from x in DB.Accounts
                    where Session.UserId != null && x.WpId == Session.UserId
                    orderby x.LastLoginTime descending
                    select x
                 ).FirstOrDefaultAsync();

                if (account == null)
                {
                    await CancelToken(Session);
                    throw new ApiResponseException(nameof(ErrorCodes.UserNotRegistered), ErrorCodes.UserNotRegistered);
                }

                account.Email = Session.UserEmail;
                account.Name = Session.UserName;
                account.CompanyName = Session.CompanyName;
                account.MembershipType = Session.MembershipType;
                account.AccessToken = Session.AccessToken;

                DB.SaveChanges();

                return Ok(new ApiResponse<Account>()
                {
                    Failed = false,
                    Result = account
                });
            }
            catch (Exception exception)
            {
                return Ok(ReturnError<Account>(exception));
            }
        }

        [HttpGet()]
        [Route("callback")]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Callback()
        {
            LoadWebSession();

            string error = HttpContext.Request.Query["error"];
            string description = HttpContext.Request.Query["error_description"];

            if (error != null)
            {
                Logger.LogInformation($"Callback: {error} {description}");
                return Redirect($"/login?status=0&error={error}&error_text={description}");
            }

            try
            {
                await CancelToken(Session);
                await RequestToken(HttpContext.Request.Query["code"], Session);
                SaveWebSession();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Unexpected error requesting token");
                return Redirect($"/login?status=0&error={WebUtility.UrlEncode(e.GetType().Name)}&error_text={WebUtility.UrlEncode(e.Message)}");
            }

            Logger.LogInformation($"Callback: LoggedIn '{Session.Context}'");
            return Redirect($"/login?status=2&url={((String.IsNullOrEmpty(Session.Context)) ? '/' : Session.Context)}");
        }

        private async Task CancelToken(WebSession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            if (session?.AccessToken == null)
            {
                return;
            }

            var form = new Dictionary<string, string>
            {
                { "token", session.AccessToken }
            };

            using (HttpClient client = CreateClient())
            {
                var response = await client.PostAsync(RevokeUrl, new FormUrlEncodedContent(form));
                response.EnsureSuccessStatusCode();

                session.UserId = 0;
                session.UserEmail = null;
                session.UserName = null;
                session.AccessToken = null;
                session.RefreshToken = null;
                session.ExpiresBy = null;
            }
        }

        private async Task RequestToken(string code, WebSession session)
        {
            if (code == null) throw new ArgumentNullException(nameof(code));
            if (session == null) throw new ArgumentNullException(nameof(session));

            var form = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "code", $"{code}" },
                { "client_id", ClientId },
                { "client_secret", ClientSecret },
            };

            using (HttpClient client = CreateClient())
            {
                var response = await client.PostAsync(TokenUrl, new FormUrlEncodedContent(form));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Token>(json);

                session.AccessToken = result.AccessToken;
                session.RefreshToken = result.RefreshToken;
                session.ExpiresBy = DateTime.UtcNow.AddSeconds(result.ExpiresIn);

                response = await client.GetAsync($"{MeUrl}?access_token={session?.AccessToken}");
                response.EnsureSuccessStatusCode();

                json = await response.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<UserInfo>(json);

                if (body.Id != null && Int32.TryParse(body.Id, out var id))
                {
                    session.UserId = id;
                }

                session.UserEmail = body.UserEmail;
                session.UserName = body.DisplayName;
                session.CompanyName = body.CompanyName;
                session.MembershipType = UserInfo.GetMembershipType(body.MembershipType);
            }

            var account = await (
                from x in DB.Accounts
                where Session.UserId != null && x.WpId == Session.UserId
                orderby x.LastLoginTime descending
                select x
             ).FirstOrDefaultAsync();

            if (account == null)
            {
                account = new Account()
                {
                    WpId = Session.UserId,
                    Name = Session.UserName,
                    Email = Session.UserEmail,
                    CompanyName = Session.CompanyName,
                    MembershipType = Session.MembershipType
                };

                DB.Accounts.Add(account);
            }

            account.Name = Session.UserName;
            account.CompanyName = Session.CompanyName;
            account.MembershipType = Session.MembershipType;
            account.LastLoginTime = DateTime.UtcNow;

            DB.SaveChanges();
        }

        private async Task RefreshToken(WebSession session)
        {
            if (session == null) throw new ArgumentNullException(nameof(session));

            var form = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "refresh_token", $"{session.RefreshToken}" },
                { "client_id", ClientId },
                { "client_secret", ClientSecret }
            };

            using (HttpClient client = CreateClient())
            {
                var response = await client.PostAsync(TokenUrl, new FormUrlEncodedContent(form));
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Token>(json);

                session.UserId = 0;
                session.UserEmail = null;
                session.UserName = null;
                session.CompanyName = null;
                session.MembershipType = null;
                session.AccessToken = result.AccessToken;
                session.RefreshToken = result.RefreshToken;
                session.ExpiresBy = DateTime.UtcNow.AddSeconds(result.ExpiresIn);

                response = await client.GetAsync($"{MeUrl}?access_token={session?.AccessToken}");
                response.EnsureSuccessStatusCode();

                json = await response.Content.ReadAsStringAsync();
                var body = JsonSerializer.Deserialize<UserInfo>(json);

                if (Int32.TryParse(body.Id, out var id))
                {
                    session.UserId = id;
                }

                session.UserEmail = body.UserEmail;
                session.UserName = body.DisplayName;
                session.CompanyName = body.CompanyName;
                session.MembershipType = (MembershipType)(body.MembershipType ?? (int)MembershipType.NonMember);
            }

            var account = await (
                from x in DB.Accounts
                where Session.UserId != null && x.WpId == Session.UserId
                orderby x.LastLoginTime descending
                select x
             ).FirstOrDefaultAsync();

            if (account == null)
            {
                await CancelToken(session);
                throw new ApiResponseException(nameof(ErrorCodes.UserNotRegistered), ErrorCodes.UserNotRegistered);
            }

            account.LastLoginTime = DateTime.UtcNow;

            DB.SaveChanges();
        }
    }
}
