using LotteryResources.Models.Configuration;
using LotteryResources.Models.Players;
using LotteryResources.Services.Interfaces;
using LotteryResources.Storage.Interfaces;

namespace LotteryResources.Services
{
    public class GameService : IGameService
    {
        private readonly IGameDataStore _gameDataStore;
        private readonly GameConfiguration gameConfiguration;

        public GameService(IGameDataStore gameDataStore)
        {
            _gameDataStore = gameDataStore;
        }


        public void ListCpuPlayers()
        {
            var cpuPlayers = _gameDataStore.Players.Where(x => x.IsCpu);
            Console.WriteLine();
            Console.WriteLine($"{cpuPlayers.Count()} other CPU players have also purchased tickets.");
            Console.WriteLine();

            //List all CPU Players and the number of tickets they purchased
            foreach (var player in cpuPlayers)
            {
                Console.WriteLine($"{player.Name} - {player.Tickets.Count()} tickets");
            }
        }

        public void ProcessResults(decimal ticketRevenue)
        {
            Console.WriteLine();
            Console.WriteLine("Ticket Draw Results:");
            Console.WriteLine();
            decimal winnings = 0;

            //Grand Prize Calculations
            var grandPrizeWinner = _gameDataStore.GenerateGrandPrizeWinner();
            var grandPrize = _gameDataStore.CalculateGrandPrize(ticketRevenue);
            Console.WriteLine($"Grand Prize: {grandPrizeWinner.Name} wins ${Math.Round(grandPrize, 2)}");
            winnings += grandPrize;

            //Second Tier Calculations
            var secondTierWinners = _gameDataStore.GenerateSecondTierWinners();
            var secondTierPrize = _gameDataStore.CalculateSecondTier(ticketRevenue);
            var secondTierShared = (secondTierPrize / secondTierWinners.Count());

                //Generate the list of names, by removing 'Player ' from the strings, and joining them with ,
            var secondTierWinnersSplit = string.Join(", ", secondTierWinners.Select(p => p.Name.Replace("Player ", "")));
            Console.WriteLine($"Second Tier: Players {secondTierWinnersSplit} win ${Math.Round(secondTierShared, 2)} each!");
            winnings += secondTierPrize;

            //Third Tier calculations
            var thirdTierWinners = _gameDataStore.GenerateThirdTierWinners();
            var thirdTierPrize = _gameDataStore.CalculateThirdTier(ticketRevenue);
            var thirdTierShared = (thirdTierPrize / thirdTierWinners.Count());
            var thirdTierWinnersSplit = string.Join(", ", thirdTierWinners.Select(p => p.Name.Replace("Player ", "")));
            Console.WriteLine($"Third Tier: Players {thirdTierWinnersSplit} win ${Math.Round(thirdTierShared, 2)} each!");
            winnings += thirdTierPrize;

            Console.WriteLine();
            Console.WriteLine("Congratulations to the winners!");
            Console.WriteLine();

            //Determine house revenue
            Console.WriteLine($"House Revenue: ${GetHouseRevenue(ticketRevenue, winnings)}");
            Console.WriteLine();
        }

        public void AddPlayers(List<PlayerModel> players)
        {
            _gameDataStore.AddPlayers(players);
        }

        public decimal GetHouseRevenue(decimal ticketRevenue, decimal winnings) 
        {
            return ticketRevenue - winnings;
        }

    }
}
