using NUnit.Framework;

namespace roulette
{
    [TestFixture]
    public class FieldTest
    {
        [Test]
        public void forNumber_returns_field_with_that_number_and_single_number_winning_strategy()
        {
            Field f = Field.ForNumber(1);
            Assert.AreEqual("1", f.FieldName);
            Assert.AreEqual(new SingleNumberWinningStrategy(1), f.WinningStrategy);
        }

        [Test]
        public void ODD_has_an_odd_even_winning_strategy()
        {
            Assert.AreEqual(new OddEvenWinningStrategy(true), Field.ODD.WinningStrategy);
        }

        [Test]
        public void EVEN_has_an_odd_even_winning_strategy()
        {
            Assert.AreEqual(new OddEvenWinningStrategy(false), Field.EVEN.WinningStrategy);
        }
    }
}