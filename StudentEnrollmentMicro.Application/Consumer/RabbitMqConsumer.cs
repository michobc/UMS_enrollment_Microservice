using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using StudentEnrollementMicro.Application.Services;
using StudentEnrollementMicro.Domain.Models;
using StudentEnrollementMicro.Persistence.Data;

namespace StudentEnrollementMicro.Application.Consumer;

public class RabbitMqConsumerService : IHostedService
{
    private readonly ILogger<RabbitMqConsumerService> _logger;
    private readonly RabbitMqService _rabbitMqService;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private IModel _channel;
    private IConnection _connection;

    public RabbitMqConsumerService(ILogger<RabbitMqConsumerService> logger, RabbitMqService rabbitMqService, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _rabbitMqService = rabbitMqService;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMqConsumerService is starting.");

        try
        {
            _connection = _rabbitMqService.GetConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: "class_updates",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogInformation("Received message: {Message}", message);

                try
                {
                    var enrollmentData = JsonConvert.DeserializeObject<Enrollment>(message);
                    await HandleMessageAsync(enrollmentData);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error handling message");
                }
            };

            _channel.BasicConsume(queue: "class_updates",
                autoAck: true,
                consumer: consumer);

            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while starting RabbitMqConsumerService.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel?.Close();
        _connection?.Close();
        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(Enrollment enrollmentData)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<EnrollmentContext>();
            var enrollment = new Enrollment
            {
                ClassId = enrollmentData.ClassId,
                StudentId = enrollmentData.StudentId,
                StudentName = enrollmentData.StudentName,
                CourseName = enrollmentData.CourseName,
                TeacherName = enrollmentData.TeacherName
            };
            dbContext.Enrollments.Add(enrollment);
            await dbContext.SaveChangesAsync();
        }
    }
}