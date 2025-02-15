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
    UserName = "iopuser",
    Password = "iop-opc",
    TopicPrefix = "opcua",
    PublisherId = "opcf-testdata",
    UseNewEncodings = true
};

Console.WriteLine("Use Ctrl-C or Ctrl-Break or exit program.");
await new Publisher(configuration).Connect(token);
