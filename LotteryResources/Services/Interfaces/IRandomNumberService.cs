namespace LotteryResources.Services.Interfaces
{
    public interface IRandomNumberService
    {
        int Between(int minValue, int maxValue);
    }
}