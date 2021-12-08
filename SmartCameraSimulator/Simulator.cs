using CameraSystem.Shared;
using MQTTnet;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using MQTTnet.Extensions.ManagedClient;
using System.Text;
using System.Text.Json;

namespace SmartCameraSimulator;
public class Simulator
{
    IManagedMqttClient _mqttClient;
    private string responseTopic;
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
        Console.WriteLine(telemetry.CardId);
        Console.WriteLine(telemetry.PassDateTime.ToString());
        Console.WriteLine(telemetry.Log);
    }

    private string CreateRandomString()
    {
        StringBuilder sb = new StringBuilder();
        Random random = new Random();
        for (int i = 0; i < 100000; i++)
        {
            sb.Append((char)random.Next(65, 122));
        }
        return sb.ToString();
    }
}
/*
private async void UseRandomPass()
{
    PassageAttempt pass = new PassageAttempt(Guid.NewGuid().ToString(), configuration.TourniquetId, DateTime.Now);
    await MakeApiRequestAndGetAnswer(pass);
    //Из-за подписочной модели следующее действие неявно приходит с помощью события
    //Дальше должен срабатывать метод ParseAnswer
}

private async void UsePassFromInput()
{
    Console.WriteLine("Введите ID пропуска");
    PassageAttempt passageAttempt = new PassageAttempt(Console.ReadLine().Trim(), configuration.TourniquetId, DateTime.Now);
    await MakeApiRequestAndGetAnswer(passageAttempt);
    //Из-за подписочной модели следующее действие неявно приходит с помощью события
    //Дальше должен срабатывать метод ParseAnswer
    //Привет от goto
}

private async Task MakeApiRequestAndGetAnswer(PassageAttempt passageAttempt)
{
    var requestId = new Random().Next();
    var requestTopic = "v1/devices/me/rpc/request/" + requestId.ToString();
    responseTopic = "v1/devices/me/rpc/response/" + requestId.ToString();
    await _mqttClient.SubscribeAsync(responseTopic); //Подписываемся на ответ

    Console.WriteLine();
    Console.WriteLine($"Is Connected: {_mqttClient.IsConnected}");
    Console.WriteLine("Request Topic: " + requestTopic);
    Console.WriteLine("Response Topic: " + responseTopic);

    Console.WriteLine(JsonSerializer.Serialize(new RpcValidatePassRequest()
    {
        Method = "validatePass",
        Params = passageAttempt
    }).Replace("Method", "method").Replace("Params", "params"));

    //Делаем запрос
    await _mqttClient.PublishAsync(requestTopic, JsonSerializer.Serialize(new RpcValidatePassRequest()
    {
        Method = "validatePass",
        Params = passageAttempt
    }).Replace("Method", "method").Replace("Params", "params"));

    Console.WriteLine();

    Task.Delay(1000).Wait();
}


private async Task ParseAnswer(MqttApplicationMessageReceivedEventArgs obj)
{
    Console.WriteLine();
    Console.WriteLine("ParseAnswer: Received answer");
    await _mqttClient.UnsubscribeAsync(responseTopic);
    Console.WriteLine("ReasonCode: " + obj.ReasonCode);
    var message = obj.ApplicationMessage.ConvertPayloadToString();

    Console.WriteLine("Message Contents: " + message);

    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    var passageAnswer = JsonSerializer.Deserialize<PassageAnswer>(message, options);
    TourniquetActOnAnswer(passageAnswer);

    Console.WriteLine();
}

private void TourniquetActOnAnswer(PassageAnswer result)
{
    if (result.Answer)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Проход разрешен.");
        Console.ForegroundColor = ConsoleColor.White;
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Проход запрещен.");
        Console.ForegroundColor = ConsoleColor.White;
    }
}
}*/