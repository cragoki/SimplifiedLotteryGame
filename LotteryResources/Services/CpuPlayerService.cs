using LotteryResources.Models.Configuration;
using LotteryResources.Models.Players;
using LotteryResources.Services.Interfaces;
using Microsoft.Extensions.Options;
using System;

namespace LotteryResources.Services
{
    public class CpuPlayerService : ICpuPlayerService
    {
        private readonly CpuConfiguration _cpuConfiguration;
        private readonly IRandomNumberService _random;

        public CpuPlayerService(IOptions<CpuConfiguration> cpuConfiguration, IRandomNumberService random)
        {
            _cpuConfiguration = cpuConfiguration.Value;
            _random = random;
        }

        public int GenerateNumberOfCpuPlayers() 
        {
            return _random.Between(_cpuConfiguration.MinPlayers, _cpuConfiguration.MaxPlayers);
        }

        public List<PlayerModel> GeneratePlayers(int numberOfCpuPlayers)
        {
            var result = new List<PlayerModel>();
            for (int i = 0; i < numberOfCpuPlayers; i++)
            {
                result.Add(new PlayerModel()
                {
                    Name = $"Player {i + 2}",
                    IsCpu = true,
                });
            }

            return result;
        }

        public decimal GetCpuBalance() 
        {
            return _cpuConfiguration.CpuBalance;
        }
    }
}
