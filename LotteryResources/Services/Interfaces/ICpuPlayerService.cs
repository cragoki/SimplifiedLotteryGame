using LotteryResources.Models.Players;

namespace LotteryResources.Services
{
    public interface ICpuPlayerService
    {
        int GenerateNumberOfCpuPlayers();
        List<PlayerModel> GeneratePlayers(int numberOfCpuPlayers);
        decimal GetCpuBalance();
    }
}