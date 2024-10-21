using LotteryResources.Models.Configuration;
using LotteryResources.Services;
using LotteryResources.Storage;
using Microsoft.Extensions.Options;

namespace UnitTests.TicketTests
{
    [TestFixture]
    public class TicketService_TicketGeneration
    {
        private TicketService _ticketService;

        [SetUp]
        public void SetUp()
        {
            var testConfig = new TicketConfiguration
            {
                TicketPrice = 1
            };
            var options = Options.Create(testConfig);

            var ticketDataStore = new TicketDataStore(options);
            _ticketService = new TicketService(ticketDataStore);
        }

        [Test]
        public void GenerateTickets_TicketCountTest()
        {
            int value = 5;

            var result = _ticketService.GenerateTickets(value);

            Assert.AreEqual(value, result.Count());
        }
    }
}
