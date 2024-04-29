using Amazon.DynamoDBv2;
using Amazon.Runtime;
using Application.Command;
using Application.Events;
using Application.Helper;
using ConsumerService;
using MediatR;
using Persistence.Repositories;
using RabbitMQ.Client;
using Service.Clients;
using Service.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(SaveEmployee)));
builder.Services.AddTransient<IRequestHandler<SaveEmployee, Unit>, SaveEmployeeHandler>();
builder.Services.AddSingleton<IRabbitMqProducer<EmployeeEvent>, Producer>();
builder.Services.AddSingleton(serviceProvider =>
{
    return new ConnectionFactory
    {
        HostName = "localhost",
        Port = 5672,
        UserName = "guest",
        Password = "guest",
        VirtualHost = "CUSTOM_HOST",
        DispatchConsumersAsync = true,
    };
});

var dynamoDbConfig = builder.Configuration.GetSection("DynamoDb");
var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");
builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
{
    var clientConfig = new AmazonDynamoDBConfig
    {
        ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl"),        
    };
    return new AmazonDynamoDBClient(clientConfig);
});
builder.Services.AddHostedService<ConsumerWorker>();

builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

var host = builder.Build();
host.Run();
