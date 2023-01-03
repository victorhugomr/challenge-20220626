using Quartz;

namespace challenge_20220626.Jobs
{
    public class LoggerJob : IJob
    {
        private readonly ILogger<LoggerJob> _logger;
        public LoggerJob(ILogger<LoggerJob> logger)
        {
            this._logger = logger;
        }
        public Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation($"Log Job: at {DateTime.Now} and Jobtype: {context.JobDetail.JobType}");
            return Task.CompletedTask;
        }
    }
}