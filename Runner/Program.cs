using EP94.ThinqSharp;
using EP94.ThinqSharp.Clients;
using EP94.ThinqSharp.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;

// In this example Serilog is used, but it can be any logging library
Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console().CreateLogger();

ThinqClient thinqClient;
string passportFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Passport.json");

// A passport can be used to speed up the login process
// In this example the passport is saved in a json file
if (!File.Exists(passportFilePath))
{
    thinqClient = new ThinqClient(LoggerFactory.Create(builder => builder.AddSerilog()));
    // Request the passport and save it for later use
    Passport passport = await thinqClient.GetPassportAsync("your_username", "your_password");
    File.WriteAllText(passportFilePath, JsonConvert.SerializeObject(passport));
}
else
{
    Passport passport = JsonConvert.DeserializeObject<Passport>(File.ReadAllText(passportFilePath));
    // If the passport is provided through the constructor, the GetPassportAsync method doesn't have to be called
    thinqClient = new ThinqClient(passport, LoggerFactory.Create(builder => builder.AddSerilog()));
}
IEnumerable<DeviceClient> deviceClients = await thinqClient.ConnectAsync();

// Turn all airconditioners on
foreach (DeviceClient deviceClient in deviceClients)
{
    if (deviceClient is AcClient acClient)
    {
        await acClient.SetSnapshotValue(snapshot => snapshot.IsOn, true);
        // or
        await acClient.SetOnState(true);
        // or send multiple values
        await acClient.SendMultipleValues(TimeSpan.FromMilliseconds(100))
            .Value(s => s.IsOn, true)
            .Value(s => s.OperationMode, (double)AcMode.Cool)
            .Value(s => s.FanSpeed, (double)AcFanSpeed.High)
            .SendCommandsAsync();
    }
    deviceClient.OnDeviceSnapshotChanged += (sender, args) =>
    {
        Console.WriteLine($"Snapshot of device {deviceClient.DeviceInfo.Alias} has changed: {deviceClient.DeviceSnapshot}");
    };
}    