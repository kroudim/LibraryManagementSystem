using Microsoft.EntityFrameworkCore;
using MassTransit;
using Reservation.Application.Services;
using Reservation.Domain.Interfaces;
using Reservation.Infrastructure.Data;
using Reservation.Infrastructure.Repositories;
using Reservation.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ReservationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("ReservationDb"))
        .ConfigureWarnings(w=>w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));


builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<IEventPublisher, EventPublisher>();

builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"] ?? "localhost", "/", h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"] ?? "guest");
            h.Password(builder.Configuration["RabbitMQ:Password"] ?? "guest");
        });

        cfg.UseMessageRetry(r => r.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)));
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();
