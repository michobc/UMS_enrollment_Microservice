using Microsoft.EntityFrameworkCore;
using StudentEnrollementMicro.Application.Consumer;
using StudentEnrollementMicro.Application.Services;
using StudentEnrollementMicro.Persistence.Data;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<EnrollmentContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("StudentEnrollmentMicro.API"));
});

// Register RabbitMQ service
builder.Services.AddSingleton<RabbitMqService>();
builder.Services.AddHostedService<RabbitMqConsumerService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();