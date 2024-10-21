using LotteryResources.Services;
using LotteryResources.Services.Interfaces;

namespace SimplifiedLotteryGame
{
    public class SimplifiedLotteryGame
    {
        private readonly IPlayerService _playerService;
        private readonly ITicketService _ticketService;
        private readonly ICpuPlayerService _cpuPlayerService;
        private readonly IGameService _gameService;
        private readonly IRandomNumberService _random;


        public SimplifiedLotteryGame(IPlayerService playerService, ITicketService ticketService, ICpuPlayerService cpuPlayerService, IGameService gameService, IRandomNumberService random)
        {
            _playerService = playerService;
            _ticketService = ticketService;
            _cpuPlayerService = cpuPlayerService;
            _random = random;
            _gameService = gameService;
        }

        public void Execute()
        {
            //Generate welcome and display player balance
            _playerService.DisplayWelcome();

            //Display ticket price
            _ticketService.DisplayTicketPrice();

            //Ask user how many tickets they want to buy
            decimal playerBalance = _playerService.PromptTicketPurchase();

            //Validate ticket purchase is valid
            var ticketReciept = _ticketService.ProcessNumberOfPlayerTickets(playerBalance);

            //Generate Tickets for player
            var playerTickets = _ticketService.GenerateTickets(ticketReciept.NumberOfTickets);

            //Update Player Balance
            _playerService.UpdateBalance(false, ticketReciept.TicketCost);

            //Generate Player Model for Game Storage
            var humanPlayer = _playerService.GeneratePlayerForGame(playerTickets, ticketReciept.TicketCost);
            //Generate CPU Players seperately
            var numberOfCpuPlayers = _cpuPlayerService.GenerateNumberOfCpuPlayers();
            var allPlayers = _cpuPlayerService.GeneratePlayers(numberOfCpuPlayers);
            //Generate tickets for Cpu Players
            foreach (var cpu in allPlayers)
            {
                bool affordable = false;
                int numberOfTickets = 0;
                while (!affordable)
                {
                    //Doing this in the case the cost of tickets is increased, the cpu balance will always default to 10 for now.
                    numberOfTickets = _random.Between(1, 10);
                    var cost = _ticketService.CalculateTicketCost(numberOfTickets);
                    if (cost <= _cpuPlayerService.GetCpuBalance()) 
                    {
                        affordable = true;
                    }
                }
                cpu.Tickets = new List<Guid>();
                cpu.Tickets.AddRange(_ticketService.GenerateTickets(numberOfTickets));
            }

            allPlayers.Add(humanPlayer);
            //Add all players to game state
            _gameService.AddPlayers(allPlayers);

            //List Cpu Players
            _gameService.ListCpuPlayers();

            //Generate the results based on the CPU players and tickets purchased
            _gameService.ProcessResults(_ticketService.GetTicketRevenue());
        }
    }
}
