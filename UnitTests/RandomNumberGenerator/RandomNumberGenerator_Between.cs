using LotteryResources.Models.Configuration;
using LotteryResources.Services;

namespace UnitTests.RandomNumberGenerator
{
    [TestFixture]
    public class RandomNumberGenerator_Between
    {
        private RandomNumberService _random;
        private PlayerConfiguration _playerConfiguration;

        [SetUp]
        public void SetUp()
        {
            _random = new RandomNumberService();
        }

        [Test]
        public void BetweenValidation()
        {
            var result = _random.Between(10, 15);
            Assert.IsTrue(result >= 10 && result <= 15, "Random number was not within the range of 10 to 15");
        }
    }
}
