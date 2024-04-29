using Application.Events;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Service.Clients;
using Service.MessageBroker;

namespace Application.Helper
{
    public class Producer : ProducerBase<EmployeeEvent>
    {
        public Producer(ConnectionFactory connectionFactory, ILogger<RabbitMQClientBase> logger, ILogger<ProducerBase<EmployeeEvent>> producerBaseLogger) : base(connectionFactory, logger, producerBaseLogger)
        {
        }

        protected override string ExchangeName => "CUSTOM_HOST.LoggerExchange";
        protected override string RoutingKeyName => "log.message";
        protected override string AppId => "LogProducer";
    }
}
