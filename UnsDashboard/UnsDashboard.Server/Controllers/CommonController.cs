using Microsoft.AspNetCore.Mvc;
using UnsDashboard.Server.Model;

namespace UnsDashboard.Server.Controllers
{
    public class CommonController : ControllerBase
    {
        protected IConfiguration Configuration { get; private set; }

        protected ILogger Logger { get; private set; }

        protected WebSession Session { get; private set; }

        protected string ConnectionString { get; private set; }

        protected DatabaseContext DB { get; private set; }

        public CommonController(IConfiguration configuration, ILogger logger, DatabaseContext db)
        {
            Configuration = configuration;
            Logger = logger;
            ConnectionString = Configuration.GetConnectionString("DatabaseContext");
            DB = db;
        }

        protected void LoadWebSession()
        {
            if (HttpContext.Session.TryGetValue("SD", out byte[] data))
            {
                Session = WebSession.FromBytes(data);
            }
            else
            {
                Session = new WebSession();
            }
        }

        protected void SaveWebSession()
        {
            var bytes = Session.ToBytes();
            HttpContext.Session.Set("SD", bytes);
        }

        protected HttpClient CreateClient()
        {
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;

            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    if (policyErrors != System.Net.Security.SslPolicyErrors.None)
                    {
                        Logger.LogError($"{cert.Subject} has errors: {policyErrors}");
                    }

                    return policyErrors == System.Net.Security.SslPolicyErrors.None;
                };

            return new HttpClient(handler);
        }

        protected ApiResponse<T> ReturnError<T>(string code, string text) where T : class
        {
            return new ApiResponse<T>()
            {
                Failed = true,
                ErrorCode = code,
                ErrorText = text
            };
        }

        protected ApiResponse<T> ReturnError<T>(Exception exception, string defaultCode = nameof(ErrorCodes.UnexpectedError)) where T : class
        {
            string code;
            string text;

            if (exception is ApiResponseException are)
            {
                code = are.ErrorCode;
                text = are.ErrorText;
            }
            else
            {
                code = defaultCode;
                text = exception.Message;
                Logger.LogError(exception, code);
            }

            return new ApiResponse<T>()
            {
                Failed = true,
                ErrorCode = code,
                ErrorText = text
            };
        }
    }


    public class ApiResponse<T>
    {
        public ApiResponse()
        {
        }

        public ApiResponse(Exception e, string defaultCode = null)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            Failed = true;

            if (e is ApiResponseException are)
            {
                ErrorCode = are.ErrorCode;
                ErrorText = are.ErrorText;
            }
            else
            {
                ErrorCode = (String.IsNullOrEmpty(defaultCode)) ? ErrorCodes.UnexpectedError : defaultCode;
                ErrorText = $"[{e.GetType().Name}] {e.Message}";
            }
        }

        public ApiResponse(T result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            Result = result;
        }


        public bool Failed { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorText { get; set; }

        public T Result { get; set; }

        public override string ToString()
        {
            if (Failed)
            {
                return $"[{ErrorCode}] '{ErrorText}'";
            }

            return $"{Result}";
        }
    }

    public class ApiResponseException : ApplicationException
    {
        public ApiResponseException(string code, string text) : base(text)
        {
            ErrorCode = code;
        }

        public ApiResponseException(string code, string text, Exception e) : base(text, e)
        {
            ErrorCode = code;
        }

        public string ErrorCode { get; }

        public string ErrorText { get { return base.Message; } }
    }
}