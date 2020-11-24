using System;

namespace roulette
{
    public class Account
    {
        public double Balance { get; private set; }

        public void Credit(double amount)
        {
            if (amount < 0) throw new ArgumentException("Credit amount cannot be negative");
            Balance += amount;
        }
    }
}
