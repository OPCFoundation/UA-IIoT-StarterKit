namespace UnsDashboard.Server.Model
{
    public class Server
    {
        public int? Id { get; set; }

        public string EndpointUrl { get; set; }

        public int? SecurityMode { get; set; }

        public string SecurityProfileUri { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
