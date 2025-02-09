 using UaPublisher;
using UaPubSubCommon;

var cts = new CancellationTokenSource();
CancellationToken token = cts.Token;

// raised when Ctrl-C or Ctrl-Break is pressed.
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true; // Prevents immediate termination
    cts.Cancel();    // Signals cancellation
};

var configuration = new Configuration()
{
    BrokerHost = "iop-gateway-germany.opcfoundation.org",
    BrokerPort = 8883, // 1883,
    UseTls = true,
    UserName = "iopuser",
    Password = "iop-opc",
    TopicPrefix = "opcua-test",
    PublisherId = "opcf-iiot-kit-dotnet",
    UseNewEncodings = true,
};

configuration.ApplicationDescription = new()
{
    ApplicationName = "OPC-F IIoT StarterKit Publisher (.NET)",
    // always use Client unless the application has Server endpoints.
    ApplicationType = Opc.Ua.ApplicationType.Client,
    // a globally unique identifier for the running s/w. does not need to use the publisher id.
    ApplicationUri = $"urn:{Configuration.GetPrivateHostName()}.local:{DateTime.Now.ToString("yyyy-MM")}:{configuration.TopicPrefix}:{configuration.PublisherId}",
    // the product indicates the s/w and h/w running.
    ProductUri = $"https://github.com/OPCFoundation/UA-IIoT-StarterKit"
};

Log.System("Use Ctrl-C or Ctrl-Break or exit program.");
await new Publisher(configuration).Connect(token);
