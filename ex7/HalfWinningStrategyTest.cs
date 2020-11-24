using NUnit.Framework;

namespace roulette
{
    [TestFixture]
    public class HalfWinningStrategyTest
    {
        [Test]
        public void lower_wins_on_low_numbers()
        {
            for (int i = 1; i < 19; i++)
            {
                Assert.IsTrue(new HalfWinningStrategy(1).WinsOn(i), "" + i);
                Assert.IsFalse(new HalfWinningStrategy(2).WinsOn(i), "" + i);
            }
        }

        [Test]
        public void both_upper_and_lower_lose_on_zero()
        {
            Assert.IsFalse(new HalfWinningStrategy(1).WinsOn(0));
            Assert.IsFalse(new HalfWinningStrategy(2).WinsOn(0));
        }

        [Test]
        public void higher_wins_on_high_numbers()
        {
            for (int i = 19; i < 37; i++)
            {
                Assert.IsFalse(new HalfWinningStrategy(1).WinsOn(i), "" + i);
                Assert.IsTrue(new HalfWinningStrategy(2).WinsOn(i), "" + i);
            }
        }
    }
}