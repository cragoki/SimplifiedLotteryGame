using LotteryResources.Models.Configuration;
using LotteryResources.Models.Players;
using LotteryResources.Services;
using LotteryResources.Services.Interfaces;
using LotteryResources.Storage;
using Microsoft.Extensions.Options;

namespace UnitTests.GameTests
{
    [TestFixture]
    public class CpuPlayerService_ManagePlayers
    {
        private CpuPlayerService _cpuService;
        private CpuConfiguration _cpuConfiguration;

        [SetUp]
        public void SetUp()
        {

            _cpuConfiguration = new CpuConfiguration
            {
                MaxPlayers = 15,
                MinPlayers = 10,
                CpuBalance = 10
            };
            var options = Options.Create(_cpuConfiguration);

            _cpuService = new CpuPlayerService(options, new RandomNumberService());
        }

        [Test]
        public void RandomNumberOfPlayers()
        {
            var result = _cpuService.GenerateNumberOfCpuPlayers();

            Assert.IsTrue(result >= _cpuConfiguration.MinPlayers && result <= _cpuConfiguration.MaxPlayers, "Number of players outside min and max range");
        }

        [Test]
        public void GeneratePlayers()
        {
            int value = 10;
            var result = _cpuService.GeneratePlayers(value);

            Assert.AreEqual(value, result.Count());
        }
    }
}
