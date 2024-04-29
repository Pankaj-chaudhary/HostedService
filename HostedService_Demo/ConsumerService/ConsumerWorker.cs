using Application.Command;
using MediatR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.Clients;
using Service.MessageBroker;

namespace ConsumerService
{
    public class ConsumerWorker : ConsumerBase, IHostedService
    {
        protected override string QueueName => "CUSTOM_HOST.log.message";

        public ConsumerWorker(
            IMediator mediator,
            ConnectionFactory connectionFactory,
            ILogger<ConsumerWorker> logConsumerLogger,
            ILogger<ConsumerBase> consumerLogger,
            ILogger<RabbitMQClientBase> logger) :
            base(mediator, connectionFactory, consumerLogger, logger)
        {
            try
            {
                var consumer = new AsyncEventingBasicConsumer(Channel);
                consumer.Received += OnEventReceived<SaveEmployee>;
                Channel.BasicConsume(queue: QueueName, autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                logConsumerLogger.LogCritical(ex, "Error while consuming message");
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}
