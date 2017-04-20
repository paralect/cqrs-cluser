namespace ParalectEventSourcing.Messaging.RabbitMq
{
    public class RabbitMqRoutingConfiguration
    {
        public const string WriteModelQueue = "WriteModelQueue";
        public const string ReadModelQueue = "ReadModelQueue";

        public const string ErrorExchange = "ErrorExchange";
        public const string SuccessExchange = "SuccessExchange";
    }
}