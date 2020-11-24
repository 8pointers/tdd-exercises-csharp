namespace roulette
{
    public interface WalletService
    {
        void AdjustBalance(Player p, int amount);

        bool IsAvailable(Player p, int amount);
    }
}