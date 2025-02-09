using UaPubSubCommon;
using UaSubscriber;

try
{
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
        BrokerPort = 1883,
        UserName = "iopuser",
        Password = "iop-opc",
        TopicPrefix = "opcua-test",
        PublisherId = "opcf-iiot-kit-requestor",
        UseNewEncodings = true
    };

    Log.System("Use Ctrl-C or Ctrl-Break or exit program.");
    await new Subscriber(configuration, false).Connect(token);
}
catch (AggregateException e)
{
    Log.Error($"[{e.GetType().Name}] {e.Message}");

    foreach (var ie in e.InnerExceptions)
    {
        Log.Warning($">>> [{ie.GetType().Name}] {ie.Message}");
    }

    Environment.Exit(3);
}
catch (Exception e)
{
    Log.Error($"[{e.GetType().Name}] {e.Message}");

    Exception ie = e.InnerException;

    while (ie != null)
    {
        Log.Warning($">>> [{ie.GetType().Name}] {ie.Message}");
        ie = ie.InnerException;
    }

    Log.System($"========================");
    Log.Info($"{e.StackTrace}");
    Log.System($"========================");

    Environment.Exit(3);
}