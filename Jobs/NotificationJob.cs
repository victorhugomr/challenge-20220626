using Quartz;

namespace challenge_20220626.Jobs{
    class NotificationJob : IJob{
        private readonly ILogger<NotificationJob> _logger;
        public NotificationJob(ILogger<NotificationJob> logger){
            this._logger = logger;
        }
        public Task Execute(IJobExecutionContext context){
            _logger.LogInformation($"Notification Job: Notify User at {DateTime.Now} and Jobtype: {context.JobDetail.JobType}");
            return Task.CompletedTask;
        }
    }
}