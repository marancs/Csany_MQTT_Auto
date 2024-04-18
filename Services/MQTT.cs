using MQTTnet.Client;
using MQTTnet;
using MQTTnet.Protocol;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NaviOkt.Services
{
    public partial class Mqtt : ObservableObject
    {
        private IMqttClient mqttClient;
        private MqttFactory mqttFactory;
        private string _topic;
        public string Topic
        {
            get
            {
                return _topic;
            }
            set
            {
                SetProperty(ref _topic, value);
            }
        }

        public Mqtt(string topic)
        {
            Topic = topic;
        }

        public async Task<IMqttClient> Start()
        {
            mqttFactory = new MqttFactory();
            Random ran = new Random();
            mqttClient = mqttFactory.CreateMqttClient();
            var mqttClientOptions = new MqttClientOptionsBuilder()
                    .WithTcpServer("almaspite.duckdns.org", 1883)
                    .WithCredentials("marancs", "Mt28335379") // Set username and password
                    .WithClientId("mqtt_" + ran.Next(1000, 10000).ToString())
                    .WithCleanSession()
                    .Build();

            await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);
            var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(
                    f =>
                    {
                        f.WithTopic(Topic);
                    })
                .Build();
            await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
            return mqttClient;
        }


        public async Task Publish(IMqttClient client, string message)
        {
            var mb = new MqttApplicationMessageBuilder()
                .WithTopic(Topic)
                .WithPayload(message)
                .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
                .WithRetainFlag()
                .Build();

            await client.PublishAsync(mb);
        }

        bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Object.Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}