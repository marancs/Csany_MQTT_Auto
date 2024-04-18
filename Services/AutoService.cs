using MQTTnet.Client;
using System.Text.Json;
using System.Text;
namespace NaviOkt.Services
{
    public partial class AutoService : ObservableObject
    {
        [ObservableProperty]
        List<Auto> autok;

        [ObservableProperty]
        string topic;

        [ObservableProperty]
        bool go;

        private static Random random = new Random();
        public AutoService() { 
        
        }
        public async Task GetAutokAsync()
        {
            Topic = "Auto/Keres";
            Go = false;
            Mqtt m = new(Topic);
            IMqttClient client = await m.Start();
            client.ApplicationMessageReceivedAsync += Client_ApplicationMessageReceivedAsync;
            await m.Publish(client, "all");
            
        }
        private Task Client_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            string Payload = Encoding.UTF8.GetString(arg.ApplicationMessage.PayloadSegment);
            Console.WriteLine("\nPayload: " + Payload);
            Console.WriteLine("\nTopic: " + arg.ApplicationMessage.Topic);
            if (arg.ApplicationMessage.Topic == Topic && Payload.Length > 10)
            {
                Autok = new List<Auto>();
                Console.WriteLine("Megvagy!!!!");
                var doc = JsonDocument.Parse(Payload);
                JsonElement root = doc.RootElement;  // nincs előtag (pl: "dataset:" ...)
                if (root.ValueKind.Equals(JsonValueKind.Array))
                {
                    foreach (var item in root.EnumerateArray())
                    {
                        Console.WriteLine(item.ToString());
                        Auto j = JsonSerializer.Deserialize<Auto>(JsonSerializer.Serialize(item));
                        Autok.Add(j);
                    }
                    Go = true;
                    Console.WriteLine("Go!!!!");
                }

            }
            return Task.CompletedTask;
        }
    }
}
