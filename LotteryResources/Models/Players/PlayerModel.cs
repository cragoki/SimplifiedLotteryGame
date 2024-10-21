using LotteryResources.Models.Tickets;

namespace LotteryResources.Models.Players
{
    public class PlayerModel
    {
        public string Name { get; set; }
        public List<Guid> Tickets { get; set; }
        public bool IsCpu { get; set; }
        public decimal Spent { get; set; }
    }
}
