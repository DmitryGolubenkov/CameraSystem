using CameraSystem.Shared;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using System.Text;
using System.Text.Json;

namespace SmartCameraSimulator;
public class Simulator
{
    IManagedMqttClient _mqttClient;
    private ApplicationConfiguration configuration;

    public Simulator()
    {
        LoadConfig();
        InitializeMqttClient();
    }

    private void LoadConfig()
    {
        try
        {
            var config = JsonSerializer.Deserialize<ApplicationConfiguration>(File.ReadAllText("appsettings.json"));
            if (config is not null)
            {
                configuration = config;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Не удалось загрузить конфигурацию симулятора с диска: " + ex.Message);
        }
    }

    private async void InitializeMqttClient()
    {

        // Creates a new client
        MqttClientOptionsBuilder builder = new MqttClientOptionsBuilder()
                                                .WithClientId(configuration.CameraId)
                                                .WithCredentials(configuration.Token, "")
                                                .WithTcpServer(configuration.ThingsBoardAddress, 1883);

        // Create client options objects
        ManagedMqttClientOptions options = new ManagedMqttClientOptionsBuilder()
                                .WithAutoReconnectDelay(TimeSpan.FromSeconds(10))
                                .WithClientOptions(builder.Build())
                                .Build();

        // Creates the client object
        _mqttClient = new MqttFactory().CreateManagedMqttClient();

        // Set up handlers
        _mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(OnConnected);
        _mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(OnDisconnected);
        _mqttClient.ConnectingFailedHandler = new ConnectingFailedHandlerDelegate(OnConnectingFailed);
        // Starts a connection with the Broker
        await _mqttClient.StartAsync(options);
    }


    private void OnConnectingFailed(ManagedProcessFailedEventArgs obj) => Console.WriteLine($"Ошибка подключения к ThingsBoard. Исключение: {obj.Exception}");
    private void OnDisconnected(MqttClientDisconnectedEventArgs obj) => Console.WriteLine($"Произведено отключение от ThingsBoard. Причина: {obj.Reason}");
    private void OnConnected(MqttClientConnectedEventArgs obj) => Console.WriteLine($"Подключение к ThingsBoard успешно. Код: {obj.ConnectResult.ResultCode}");

    public void Run()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("1. Отправить сигнал с турникета");
            Console.WriteLine("q. Выход");
            var input = Console.ReadLine();

            if (char.TryParse(input, out char c))
            {
                switch (c)
                {
                    case '1':
                        OnTourniquetSignal();
                        break;

                    case 'q':
                        return;

                    default:
                        Console.WriteLine("Ошибка: введен несуществующий вариант");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Ошибка: неверный ввод.");
                continue;
            }
        }
    }

    private void OnTourniquetSignal()
    {
        CameraTelemetry telemetry = new CameraTelemetry()
        {
            CardId = Guid.NewGuid().ToString(),
            PassDateTime = DateTime.Now,
            Log = CreateRandomString()
        };

        var publishTopic = "v1/devices/camera/telemetry";
        _mqttClient.PublishAsync(publishTopic, JsonSerializer.Serialize(telemetry));
    }

    private string CreateRandomString()
    {
        StringBuilder sb = new StringBuilder();
        Random random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        for (int i = 0; i < 1000; i++)
        {
            sb.Append(chars[random.Next(chars.Length)]);
        }

        return sb.ToString();
    }
}