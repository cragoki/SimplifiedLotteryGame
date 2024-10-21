using LotteryResources.Models.Tickets;
using LotteryResources.Services.Interfaces;
using LotteryResources.Storage.Interfaces;

namespace LotteryResources.Services
{

    public class TicketService : ITicketService
    {
        private readonly ITicketDataStore _ticketDataStore;

        public TicketService(ITicketDataStore ticketDataStore)
        {
            _ticketDataStore = ticketDataStore;
        }

        public void DisplayTicketPrice()
        {
            Console.WriteLine($"* Ticket Price: ${_ticketDataStore.TicketPrice} each");
            Console.WriteLine();
        }

        public TicketRecieptModel ProcessNumberOfPlayerTickets(decimal playerBalance)
        {
            decimal ticketCost = 0m;
            int numberOfTickets = 0;

            bool validationComplete = false;
            while (!validationComplete)
            {
                string? numberOfTicketsInput = Console.ReadLine();
                if (!Int32.TryParse(numberOfTicketsInput, out numberOfTickets))
                {
                    Console.WriteLine("Please enter a valid number.");
                    continue;
                }
                //Validate the number of tickets allowed
                if (!ValidateMaximumTickets(numberOfTickets))
                {
                    Console.WriteLine("Please choose a value less than 10 and greater than 0.");
                    continue;
                }
                //Validate that the player balance is sufficient
                ticketCost = CalculateTicketCost(numberOfTickets);
                if (!ValidateTicketCost(ticketCost, playerBalance))
                {
                    var maxTickets = MaxNumberOfTicketsForBalance(playerBalance);

                    if (maxTickets == 0) 
                    {
                        Console.WriteLine("Insufficient balance to purchase any tickets.");
                        continue;
                    }
                    Console.WriteLine($"Insufficient balance to purchase {numberOfTickets}, purchasing {maxTickets} instead.");
                    //Buy the maximum amount of tickets
                    numberOfTickets = maxTickets;
                }

                validationComplete = true;
            }

            return new TicketRecieptModel() 
            {
                TicketCost = ticketCost,
                NumberOfTickets = numberOfTickets
            };
        }

        public List<Guid> GenerateTickets(int numberOfTickets)
        {
            var result = new List<Guid>();

            for(int i = 0; i < numberOfTickets; i ++)
            {
                result.Add(Guid.NewGuid());
                //Keep running roll of tickets purchased
                _ticketDataStore.TicketPurchased();
            }

            return result;
        }

        public decimal CalculateTicketCost(int numberOfTickets) 
        {
            //Get price of tickets
            var ticketPrice = _ticketDataStore.TicketPrice;

            //Get ticket cost
            return numberOfTickets * ticketPrice;
        }

        public bool ValidateMaximumTickets(int numberOfTickets)
        {
            return _ticketDataStore.MaxTickets >= numberOfTickets && numberOfTickets > 0;
        }

        public bool ValidateTicketCost(decimal ticketCost, decimal playerBalance)
        {
            return ticketCost <= playerBalance;
        }

        public int MaxNumberOfTicketsForBalance(decimal playerBalance)
        { 
            return (int)(playerBalance / _ticketDataStore.TicketPrice);
        }

        public decimal GetTicketRevenue() 
        {
            return _ticketDataStore.TotalTicketRevenue;
        }
    }
}
