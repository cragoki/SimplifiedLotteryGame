using LotteryResources.Services.Interfaces;

namespace LotteryResources.Services
{
    public class RandomNumberService : IRandomNumberService
    {
        private readonly Random _random;

        // Private constructor to prevent instantiation
        public RandomNumberService() { }

        public int Between(int minValue, int maxValue)
        {
            // Thread Safe
            return Random.Shared.Next(minValue, maxValue);
        }
    }
}
