namespace ParalectEventSourcing.Messaging.RabbitMq
{
    public class RabbitMqConnectionSettings
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }

        public RabbitMqConnectionSettings()
        {
#if DEBUG
            UserName = "guest";
            Password = "guest";
            VirtualHost = "/";
            HostName = "localhost";
            Port = 5672;
#else
            UserName = "guest";
            Password = "guest";
            VirtualHost = "/";
            HostName = "rabbit";
            Port = 5672;
#endif
        }
    }
}
