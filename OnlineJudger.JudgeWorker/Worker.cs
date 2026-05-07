using OnlineJudger.Domain.Stores;

namespace OnlineJudger.JudgeWorker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                  //  _logger.LogInformation("Воркер запущен в: {time}", DateTimeOffset.Now);
                }
                using (var scope = _serviceProvider.CreateScope())
                {
                    var queueRepo = scope.ServiceProvider.GetRequiredService<IQueueRepository>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var judgeEngine = scope.ServiceProvider.GetRequiredService<JudgeEngine>();
                    var sub = await queueRepo.GetFirstAsync();
                    if (sub != null)
                    {
                        _logger.LogInformation($"submission с id: {sub.SubmissionId} захвачен в {DateTimeOffset.Now}");
                        await judgeEngine.ProcessAsync(sub.SubmissionId);
                        _logger.LogInformation($"submission с id: {sub.SubmissionId} прошел все тесты в {DateTimeOffset.Now}");
                        queueRepo.Remove(sub);
                        await unitOfWork.SaveChangesAsync();
                        _logger.LogInformation($"submission с id: {sub.SubmissionId} удален из очереди в {DateTimeOffset.Now}");

                    }
                    else
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                }
            }
        }
    }
}
