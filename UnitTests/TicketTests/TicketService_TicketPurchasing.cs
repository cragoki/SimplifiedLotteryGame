using LotteryResources.Models.Configuration;
using LotteryResources.Services;
using LotteryResources.Storage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace UnitTests.TicketTests
{
    [TestFixture]
    public class TicketService_TicketPurchasing
    {
        private TicketService _ticketService;
        private TicketConfiguration _ticketConfiguration;

        [SetUp]
        public void SetUp()
        {
            _ticketConfiguration = new TicketConfiguration
            {
                TicketPrice = 1,
                MaxTickets = 10
            };
            var options = Options.Create(_ticketConfiguration);

            var ticketDataStore = new TicketDataStore(options);
            _ticketService = new TicketService(ticketDataStore);
        }

        [Test]
        public void CalculateTicketCost_ZeroValue()
        {
            int value = 3; 
            var result = _ticketService.CalculateTicketCost(value);

            Assert.AreEqual((_ticketConfiguration.TicketPrice * value), result);
        }

        [Test]
        public void CalculateTicketCost()
        {
            int value = 0;
            var result = _ticketService.CalculateTicketCost(value);

            Assert.AreEqual((_ticketConfiguration.TicketPrice * value), result);
        }

        [Test]
        public void ValidateMaximumTickets_ReturnsTrue()
        {
            int value = 5;
            var result = _ticketService.ValidateMaximumTickets(value);

            Assert.IsTrue(result);
        }
        [Test]
        public void ValidateMaximumTickets_IsZero_ReturnsFalse()
        {
            int value = 0;
            var result = _ticketService.ValidateMaximumTickets(value);

            Assert.IsFalse(result);
        }
        [Test]
        public void ValidateMaximumTickets_GreaterThanMax_ReturnsTrue()
        {
            int value = 11;
            var result = _ticketService.ValidateMaximumTickets(value);

            Assert.IsFalse(result);
        }
        [Test]
        public void ValidateTicketCost_ReturnsTrue()
        {
            int cost = 5;
            int balance = 10;

            var result = _ticketService.ValidateTicketCost(cost, balance);

            Assert.IsTrue(result, "Balance is greater than cost");
        }
        [Test]
        public void ValidateTicketCost_ReturnsFalse ()
        {
            int cost = 50;
            int balance = 10;

            var result = _ticketService.ValidateTicketCost(cost, balance);

            Assert.IsFalse(result, "Cost is greater than balance");
        }

        [Test]
        public void MaxNumberOfTicketsForBalance()
        {
            int balance = 3;

            var result = _ticketService.MaxNumberOfTicketsForBalance(balance);

            Assert.AreEqual(balance, result);
        }

        [Test]
        public void GetTicketRevenue()
        {
            int value = 5;
            var tickets = _ticketService.GenerateTickets(value);
            var result = _ticketService.GetTicketRevenue();
            Assert.AreEqual(5, result);
        }
    }
}
