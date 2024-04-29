using Amazon.DynamoDBv2;
using Application.Command;
using Application.DTO;
using Application.Events;
using Application.Helper;
using MediatR;
using Microsoft.OpenApi.Models;
using Persistence.Repositories;
using RabbitMQ.Client;
using Service.Clients;
using Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(CreateEmployee)));
builder.Services.AddSingleton<IRabbitMqProducer<EmployeeEvent>, Producer>();
builder.Services.AddSingleton(serviceProvider =>
{
    return new ConnectionFactory
    {
        HostName = "localhost",
        Port = 5672,
        UserName = "guest",
        Password = "guest",
        VirtualHost = "CUSTOM_HOST"
    };
});

builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
builder.Services.AddSingleton<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddSingleton<IEmployeeService, EmployeeService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapPost("/employee", async (EmployeeDTO request, IMediator mediator) =>
{
    var createEmployeeCommand = new CreateEmployee(request);
    var response = await mediator.Send(createEmployeeCommand);

    return Results.Created($"employee/{response.Id}", response);
})
.WithName("CreateEmployee")
.WithOpenApi(x => new OpenApiOperation(x)
{
    Summary = "Create Employee Record",
    Description = "An endpoint to create a new employee create event in RabbitMQ.",
    Tags = new List<OpenApiTag> { new() { Name = "Employee" } }
});

app.Run();
