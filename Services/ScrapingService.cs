namespace challenge_20220626.Services{

    class ScrapingService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"My Service is running at {DateTime.Now}");
                await Task.Delay(2000, stoppingToken);
            }
        }
    }
}