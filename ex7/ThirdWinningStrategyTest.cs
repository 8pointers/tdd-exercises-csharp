using NUnit.Framework;

namespace roulette
{
    public class ThirdWinningStrategyTest
    {
        [Test]
        public void Low_wins_on_low_numbers()
        {
            for (int i = 1; i < 13; i++)
            {
                Assert.IsTrue(new ThirdWinningStrategy(1).WinsOn(i));
                Assert.IsFalse(new ThirdWinningStrategy(2).WinsOn(i));
                Assert.IsFalse(new ThirdWinningStrategy(3).WinsOn(i));
            }
        }

        [Test]
        public void Mid_wins_on_mid_numbers()
        {
            for (int i = 13; i < 25; i++)
            {
                Assert.IsFalse(new ThirdWinningStrategy(1).WinsOn(i));
                Assert.IsTrue(new ThirdWinningStrategy(2).WinsOn(i));
                Assert.IsFalse(new ThirdWinningStrategy(3).WinsOn(i));
            }
        }

        [Test]
        public void High_wins_on_high_numbers()
        {
            for (int i = 25; i < 36; i++)
            {
                Assert.IsFalse(new ThirdWinningStrategy(1).WinsOn(i));
                Assert.IsFalse(new ThirdWinningStrategy(2).WinsOn(i));
                Assert.IsTrue(new ThirdWinningStrategy(3).WinsOn(i));
            }
        }

        [Test]
        public void All_three_thirds_lose_on_zero()
        {
            Assert.IsFalse(new ThirdWinningStrategy(1).WinsOn(0));
            Assert.IsFalse(new ThirdWinningStrategy(2).WinsOn(0));
            Assert.IsFalse(new ThirdWinningStrategy(3).WinsOn(0));
        }
    }
}