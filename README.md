# HostedService
A Project to show HostedService consuming messages from RabbitMQ

## Layers
- API layer to produce messages in RabbitMQ.
- Application layer with Request handlers (Mediatr)
- Service layer to talk to RabbitMQ.
- Consumer Service listening to RabbitMQ.
- Persistence layer, updating DynamoDB from queue messages.
