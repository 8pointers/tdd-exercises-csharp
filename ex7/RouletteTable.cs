using System;
using System.Collections.Generic;

namespace roulette
{
    public class TableFullException : Exception
    {
    }

    public class TooManyChipsException : Exception
    {
    }

    public class NoMoreBetsException : Exception
    {
    }

    public class NotEnoughChipsException : Exception
    {
    }


    public class RouletteTable
    {
        private readonly IList<Bet> EMPTY = new List<Bet>();
        private const int MAX_PLAYERS = 8;
        private const int ROLLING = -1;
        private const int CUT_OFF_TIME = 10000;

        private IDictionary<Field, List<Bet>> betsByFields = new Dictionary<Field, List<Bet>>();

        private int maxChipsOnTable;
        private WalletService walletService;

        private IDictionary<Player, Colour> players = new Dictionary<Player, Colour>();

        private RandomNumberGenerator rng;
        public int BallPosition { get; private set; }

        private IList<Player> waitingForPlayersToComplete = new List<Player>();

        private Timer timer;
        private long ballStartedRolling = 0;

        public void PlaceBet(Player p, int field, int value)
        {
            PlaceBet(p, new Field[] {Field.ForNumber(field)}, value);
        }

        public void PlaceBet(Player p, Field field, int value)
        {
            PlaceBet(p, new Field[] {field}, value);
        }

        public void PlaceBet(Player p, Field[] fields, int value)
        {
            // validate bet
            if (CurrentChipsOnTable() + value > maxChipsOnTable) throw new TooManyChipsException();
            if (!players.ContainsKey(p) && players.Count == MAX_PLAYERS) throw new TableFullException();

            // assign a new colour to the player if required
            if (!players.ContainsKey(p))
                players.Add(p, (Colour) Enum.GetValues(typeof(Colour)).GetValue(players.Count));

            if (IsBallRolling && (timer.TimeInMillis - ballStartedRolling > CUT_OFF_TIME))
                throw new NoMoreBetsException();
            if (!walletService.IsAvailable(p, value)) throw new NotEnoughChipsException();
            walletService.AdjustBalance(p, -1 * value);

            // place bet on all required tables

            foreach (Field field in fields)
            {
                if (!betsByFields.ContainsKey(field))
                {
                    betsByFields.Add(field, new List<Bet>());
                }

                betsByFields[field].Add(new Bet(p, value, GetBetType(fields)));
            }

            if (!waitingForPlayersToComplete.Contains(p)) waitingForPlayersToComplete.Add(p);
        }

        private BetType GetBetType(Field[] fields)
        {
            if (fields.Length == 1) return BetType.SINGLE;
            if (fields.Length == 2) return BetType.SPLIT;
            throw new ArgumentException("Unsupported bet type for " + fields + " fields");
        }

        private int CurrentChipsOnTable()
        {
            int total = 0;
            foreach (List<Bet> bets in betsByFields.Values)
            {
                foreach (Bet bet in bets)
                {
                    total = total + bet.Value;
                }
            }

            return total;
        }

        public IList<Bet> BetsByField(int field)
        {
            return BetsByField(Field.ForNumber(field));
        }

        public IList<Bet> BetsByField(Field field)
        {
            if (!betsByFields.ContainsKey(field)) return EMPTY;
            return betsByFields[field];
        }

        public RouletteTable(int maxChipsOnTable, RandomNumberGenerator generator, Timer timer,
            WalletService walletService)
        {
            this.maxChipsOnTable = maxChipsOnTable;
            this.rng = generator;
            this.timer = timer;
            this.walletService = walletService;
        }

        public IDictionary<Player, Colour> Colour
        {
            get { return players; }
        }

        public void Done(Player p)
        {
            waitingForPlayersToComplete.Remove(p);
            if (waitingForPlayersToComplete.Count == 0) Roll();
        }

        internal void Roll()
        {
            int timeToRoll = rng.Generate(30, 40);
            BallPosition = ROLLING;
            ballStartedRolling = timer.TimeInMillis;
            timer.CallBack(timeToRoll * 1000, delegate()
            {
                BallPosition = rng.Generate(0, 36);
                foreach (Field f in betsByFields.Keys)
                {
                    if (f.WinningStrategy.WinsOn(BallPosition))
                    {
                        foreach (Bet b in betsByFields[f])
                        {
                            walletService.AdjustBalance(b.Player, b.Value * f.PayoutCoefficient / b.NumberOfFields);
                        }
                    }
                }
            });
        }

        public IList<Bet> WinningBets
        {
            get
            {
                List<Bet> winningBets = new List<Bet>();
                foreach (Field f in betsByFields.Keys)
                {
                    if (f.WinningStrategy.WinsOn(BallPosition))
                    {
                        winningBets.AddRange(BetsByField(f));
                    }
                }

                return winningBets;
            }
        }

        public bool IsBallRolling
        {
            get { return BallPosition == ROLLING; }
        }
    }
}