using System;
using NUnit.Framework;

namespace roulette
{
    [TestFixture]
    public class AccountTest
    {
        private Account _account = new Account();

        [SetUp]
        public void SetUp()
        {
            _account = new Account();
        }

        [Test]
        public void Should_use_credit_to_increase_the_balance()
        {
            // arrange
            const double amount = 123.45;
            // act
            _account.Credit(amount);
            // assert
            Assert.AreEqual(amount, _account.Balance);
        }

        [Test]
        public void Should_not_be_able_to_credit_negative_amounts()
        {
            var actualException = Assert.Throws<ArgumentException>(() => _account.Credit(-1));
            Assert.AreEqual("Credit amount cannot be negative", actualException.Message);
        }
    }
}
