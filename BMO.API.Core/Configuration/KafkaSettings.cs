namespace BMO.API.Core.Configuration;

# nullable disable

    public class KafkaSettings
    {
        public ConsumerSettings Consumer { get; set; }
    }


    public class ConsumerSettings
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string GroupId { get; set; }
        public string DefaultTopic { get; set; }
        public List<TopicSetting> Topics { get; set; }
    }

    public class TopicSetting
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }