using LotteryResources.Models.Configuration;
using LotteryResources.Models.Players;
using LotteryResources.Services;
using LotteryResources.Storage;
using Microsoft.Extensions.Options;

namespace UnitTests.GameTests
{
    [TestFixture]
    public class GameDataStore_GameLogic
    {
        private GameDataStore _gameDataStore;
        private GameConfiguration _gameConfiguration;

        [SetUp]
        public void SetUp()
        {
            _gameConfiguration = new GameConfiguration
            {
                GrandPrizeShare = 50,
                SecondTierShare = 30,
                ThirdTierShare = 10,
                SecondTierPool = 10,
                ThirdTierPool = 20
            };

            var options = Options.Create(_gameConfiguration);

            _gameDataStore = new GameDataStore(options, new RandomNumberService());

            var playerList = new List<PlayerModel>()
            {
                new PlayerModel() { Name = "Player 1", IsCpu = false, Tickets = new List<Guid>(){Guid.NewGuid()} },
                new PlayerModel() { Name = "Player 2", IsCpu = true, Tickets = new List<Guid>(){Guid.NewGuid()} },
                new PlayerModel() { Name = "Player 3", IsCpu = true, Tickets = new List<Guid>(){Guid.NewGuid()} },
                new PlayerModel() { Name = "Player 4", IsCpu = true, Tickets = new List<Guid>(){Guid.NewGuid()} },
                new PlayerModel() { Name = "Player 5", IsCpu = true, Tickets = new List<Guid>(){Guid.NewGuid()} },
                new PlayerModel() { Name = "Player 6", IsCpu = true, Tickets = new List<Guid>(){Guid.NewGuid()} },
                new PlayerModel() { Name = "Player 7", IsCpu = true, Tickets = new List<Guid>(){Guid.NewGuid()} },
                new PlayerModel() { Name = "Player 8", IsCpu = true, Tickets = new List<Guid>(){Guid.NewGuid()} },
                new PlayerModel() { Name = "Player 9", IsCpu = true, Tickets = new List<Guid>(){Guid.NewGuid()} },
                new PlayerModel() { Name = "Player 10", IsCpu = true, Tickets = new List<Guid>(){Guid.NewGuid()} },
                new PlayerModel() { Name = "Player 11", IsCpu = true, Tickets = new List<Guid>(){Guid.NewGuid()} },
            };

            _gameDataStore.AddPlayers(playerList);
        }

        [Test]
        public void SelectRandomTicket_NoWinners()
        {
            var ticket = _gameDataStore.SelectRandomTicket();

            //Now get a new random ticket which should return a Guid assigned to a player
            var player = _gameDataStore.Players.Where(x => x.Tickets.Contains(ticket)).FirstOrDefault();

            Assert.IsNotNull(player);
        }

        [Test]
        public void SelectRandomTicket_WithWinners_SecondTier()
        {
            //Populate the winning players list with all players
            for (int i = 0; i < _gameDataStore.Players.Count(); i++)
            {
                _gameDataStore.GenerateSecondTierWinners();
            }

            //Now get a new random ticket which should return a Guid not assigned to a player
            var ticket = _gameDataStore.SelectRandomTicket();

            var player = _gameDataStore.Players.Where(x => x.Tickets.Contains(ticket)).FirstOrDefault();

            Assert.IsNull(player, "All Players should be in the winning player list");
        }

        [Test]
        public void SelectRandomTicket_WithWinners_ThirdTier()
        {
            //Populate the winning players list with all players
            for (int i = 0; i < _gameDataStore.Players.Count(); i++)
            {
                _gameDataStore.GenerateThirdTierWinners();
            }

            //Now get a new random ticket which should return a Guid not assigned to a player
            var ticket = _gameDataStore.SelectRandomTicket();

            var player = _gameDataStore.Players.Where(x => x.Tickets.Contains(ticket)).FirstOrDefault();

            Assert.IsNull(player, "All Players should be in the winning player list");
        }

        [Test]
        public void CalculateGrandPrize()
        {
            decimal revenue = 10m;
            decimal expected = revenue * (_gameConfiguration.GrandPrizeShare / 100);
            decimal result = _gameDataStore.CalculateGrandPrize(revenue);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CalculateSecondTier()
        {
            decimal revenue = 10m;
            decimal expected = revenue * (_gameConfiguration.SecondTierShare / 100);
            decimal result = _gameDataStore.CalculateSecondTier(revenue);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void CalculateThirdTier()
        {
            decimal revenue = 10m;
            decimal expected = revenue * (_gameConfiguration.ThirdTierShare / 100);
            decimal result = _gameDataStore.CalculateThirdTier(revenue);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GenerateSecondTierWinners_NumberOfPlayers()
        {
            var numberOfTickets = _gameDataStore.Players.SelectMany(x => x.Tickets).Count();
            var secondTierCount = (int)(numberOfTickets * (_gameConfiguration.SecondTierPool / 100));
            var secondTierPlayers = _gameDataStore.GenerateSecondTierWinners();

            Assert.AreEqual(secondTierCount, secondTierPlayers.Count());
        }

        [Test]
        public void GenerateThirdTierWinners_NumberOfPlayers()
        {
            var numberOfTickets = _gameDataStore.Players.SelectMany(x => x.Tickets).Count();
            var secondTierCount = (int)(numberOfTickets * (_gameConfiguration.ThirdTierPool / 100));
            var thirdTierPlayers = _gameDataStore.GenerateThirdTierWinners();

            Assert.AreEqual(secondTierCount, thirdTierPlayers.Count());
        }
    }
}
