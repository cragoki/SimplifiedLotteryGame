using LotteryResources.Models.Configuration;
using LotteryResources.Storage.Interfaces;
using Microsoft.Extensions.Options;

namespace LotteryResources.Storage
{
    public class TicketDataStore : ITicketDataStore
    {
        private readonly TicketConfiguration _ticketConfiguration;
        public decimal TicketPrice { get; private set; }
        public int MaxTickets { get; private set; }
        public decimal TotalTicketRevenue { get; private set; }

        public TicketDataStore(IOptions<TicketConfiguration> ticketConfiguration)
        {
            _ticketConfiguration = ticketConfiguration.Value;

            // Get the initial ticket price from appsettings.json
            if (_ticketConfiguration != null)
            {
                TicketPrice = _ticketConfiguration.TicketPrice;
                MaxTickets = _ticketConfiguration.MaxTickets;
            }
            else
            {
                //Could possibly throw a setup exception here, but for simplicity I'll hard code it in the case where there is something wrong with appsettings
                TicketPrice = 1;
                MaxTickets = 10;
            }
        }

        public void TicketPurchased() 
        {
            TotalTicketRevenue += TicketPrice;
        }
    }
}
