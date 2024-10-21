using LotteryResources.Models.Configuration;
using LotteryResources.Models.Players;
using LotteryResources.Services.Interfaces;
using LotteryResources.Storage.Interfaces;
using Microsoft.Extensions.Options;

namespace LotteryResources.Storage
{
    public class GameDataStore : IGameDataStore
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly IRandomNumberService _random;

        public List<PlayerModel> Players { get; private set; }
        private List<PlayerModel> WinningPlayers { get; set; }

        public GameDataStore(IOptions<GameConfiguration> gameConfiguration, IRandomNumberService random)
        {
            Players = new List<PlayerModel>();
            WinningPlayers = new List<PlayerModel>();
            _gameConfiguration = gameConfiguration.Value;
            _random = random;
        }

        public void AddPlayers(List<PlayerModel> players)
        {
            //If a player doesnt have any tickets, don't add them
            Players.AddRange(players.Where(x => x.Tickets.Any()));
        }

        public void ResetGame()
        {
            Players = new List<PlayerModel>();
        }

        public PlayerModel GenerateGrandPrizeWinner()
        {
            var ticket = SelectRandomTicket();
            var player = Players.Where(x => x.Tickets.Contains(ticket)).First();
            WinningPlayers.Add(player);

            return player;
        }

        public List<PlayerModel> GenerateSecondTierWinners()
        {
            var result = new List<PlayerModel>();
            //Determine number of tickets
            var numberOfTickets = Players.SelectMany(x => x.Tickets).Count();
            var secondTierCount = (int)(numberOfTickets * (_gameConfiguration.SecondTierPool / 100));
            //Create excluded player list to prevent player winning multiple times on one tier
            var excludePlayers = new List<PlayerModel>();
            for (int i = 0; i < secondTierCount; i++)
            {
                var ticket = SelectRandomTicket();
                var player = Players.Where(x => x.Tickets.Contains(ticket)).FirstOrDefault();
                if (player != null)
                {
                    result.Add(player);
                    WinningPlayers.Add(player);
                    excludePlayers.Add(player);
                }
            }

            return result;
        }

        public List<PlayerModel> GenerateThirdTierWinners()
        {
            var result = new List<PlayerModel>();
            //Determine number of tickets
            var numberOfTickets = Players.SelectMany(x => x.Tickets).Count();
            var secondTierCount = (int)(numberOfTickets * (_gameConfiguration.ThirdTierPool / 100));
            //Create excluded player list to prevent player winning multiple times on one tier
            var excludePlayers = new List<PlayerModel>();
            for (int i = 0; i < secondTierCount; i++)
            {
                var ticket = SelectRandomTicket();
                var player = Players.Where(x => x.Tickets.Contains(ticket)).FirstOrDefault();

                if (player != null) 
                {
                    result.Add(player);
                    WinningPlayers.Add(player);
                    excludePlayers.Add(player);
                }
            }

            return result;
        }

        public decimal CalculateGrandPrize(decimal ticketRevenue)
        {
            return ticketRevenue * (_gameConfiguration.GrandPrizeShare / 100);
        }

        public decimal CalculateSecondTier(decimal ticketRevenue)
        {
            return ticketRevenue * (_gameConfiguration.SecondTierShare/100);
        }

        public decimal CalculateThirdTier(decimal ticketRevenue)
        {
            return ticketRevenue * (_gameConfiguration.ThirdTierShare / 100);
        }

        public Guid SelectRandomTicket()
        {
            //Get All Tickets that do not already exist in winning tickets
            var playerList = Players;

            if (WinningPlayers.Count > 0)
            {
                var listOfExcludedNames = WinningPlayers.Select(y => y.Name);
                playerList = playerList.Where(x => !listOfExcludedNames.Contains(x.Name)).ToList();
            }

            var allTickets = playerList.SelectMany(x => x.Tickets).ToList();

            //Return a new Guild in the case where there are no remaining tickets to draw
            if (allTickets.Count() == 0)
                return Guid.NewGuid();

            // Select a random ticket from the list with the random number generator
            int randomIndex = _random.Between(0, allTickets.Count);
            return allTickets[randomIndex];
        }
    }
}
