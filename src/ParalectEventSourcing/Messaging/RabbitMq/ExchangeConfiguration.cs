namespace ParalectEventSourcing.Messaging.RabbitMq
{
    public class ExchangeConfiguration
    {
        public const string WriteModelExchange = "WriteModelExchange";
        public const string ReadModelExchange = "ReadModelExchange";
        public const string ErrorExchange = "ErrorExchange";
        public const string SuccessExchange = "SuccessExchange";
    }
}