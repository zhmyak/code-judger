using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnlineJudger.Infrastructure.Persistance;

namespace IntegrationTests
{
    public class SqlServerDatabaseFixture : IAsyncLifetime
    {
        private DbContextOptions<OnlineJudgeContext> _options;
        private string _connectionString;
        public OnlineJudgeContext Context { get; private set; }

        public async Task DisposeAsync()
        {
            Context?.Dispose();
        }

        public async Task InitializeAsync()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            _connectionString = config.GetConnectionString("SqlServer") ?? throw new Exception();
            _options = new DbContextOptionsBuilder<OnlineJudgeContext>().UseSqlServer(_connectionString).Options;
            Context = new OnlineJudgeContext(_options);
        }
    }
}
