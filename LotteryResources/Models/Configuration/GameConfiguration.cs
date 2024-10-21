namespace LotteryResources.Models.Configuration
{
    public class GameConfiguration
    {
        public decimal GrandPrizeShare { get; set; }
        public decimal SecondTierShare { get; set; }
        public decimal ThirdTierShare { get; set; }
        public decimal SecondTierPool { get; set; }
        public decimal ThirdTierPool { get; set; }
    }
}
