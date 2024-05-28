using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace BookTestProject
{
    public class BookServiceUnitTest
    {
        [Fact]
        public async Task GetBooks()
        {
            var connectionString =
                @"Server=LAPTOP-DKPU3OT5\\THOMASSQLSERVER;Database=aspnet-BookInventory-c7023200-510b-4542-873b-3fce175208e5;User ID=sa;Password=Thomas@00144277;TrustServerCertificate=True;Trusted_Connection=False;MultipleActiveResultSets=False";

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<QmsDbContext>().UseSqlServer(connectionString);
            var qmsDbContext = new QmsDbContext(dbContextOptionsBuilder.Options);

            var logger = LoggerFactory.Create((builder => { })).CreateLogger<AlertService>();

            var alertService = new AlertService(qmsDbContext, logger);

            var request = new GetGroupedActiveAlertsRequestDto()
            {
                SiteId = new Guid("28f6fd50-8d57-41a0-5a06-08d8c0ac240b")
            };

            var response = await alertService.GetGroupedActiveAlerts(request);
            Assert.True(response.Success);
        }
    }
}