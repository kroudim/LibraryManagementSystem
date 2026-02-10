using MongoDB.Driver;
using MassTransit;
using Audit.Application.Services;
using Audit.Infrastructure.Consumers;
using Audit.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IMongoClient>(sp => 
    new MongoClient(builder.Configuration.GetConnectionString("MongoDb")));

builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(builder.Configuration["MongoDb:DatabaseName"] ?? "AuditEventsDb");
});

builder.Services.AddScoped<AuditEventService>();
builder.Services.AddHostedService<DataRetentionService>();

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<PartyCreatedConsumer>();
    x.AddConsumer<PartyUpdatedConsumer>();
    x.AddConsumer<PartyDeletedConsumer>();
    x.AddConsumer<RoleAssignedConsumer>();
    x.AddConsumer<RoleRemovedConsumer>();
    x.AddConsumer<BookCreatedConsumer>();
    x.AddConsumer<BookUpdatedConsumer>();
    x.AddConsumer<BookDeletedConsumer>();
    x.AddConsumer<CategoryCreatedConsumer>();
    x.AddConsumer<CategoryUpdatedConsumer>();
    x.AddConsumer<CategoryDeletedConsumer>();
    x.AddConsumer<BookBorrowedConsumer>();
    x.AddConsumer<BookReturnedConsumer>();
    
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });

        cfg.UseMessageRetry(r => r.Exponential(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(2)));

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
