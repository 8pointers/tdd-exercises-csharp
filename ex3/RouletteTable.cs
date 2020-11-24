using System;
using System.Collections.Generic;
using System.Linq;

namespace roulette
{
    public class TableFullException : Exception
    {
    }

    public class TooManyChipsException : Exception
    {
    }

    public class RouletteTable
    {
        private const int MaxPlayers = 8;
        private static readonly IList<Bet> NoBets = new List<Bet>();

        private readonly int _maxChipsOnTable;
        private readonly IDictionary<int, List<Bet>> _betsByFields = new Dictionary<int, List<Bet>>();
        public IDictionary<Player, Colour> Colour { get; } = new Dictionary<Player, Colour>();

        public RouletteTable(int maxChipsOnTable)
        {
            _maxChipsOnTable = maxChipsOnTable;
        }

        private int CurrentChipsOnTable()
        {
            return _betsByFields.Values.SelectMany(bets => bets).Aggregate(0, (total, bet) => total + bet.Value);
        }

        public void PlaceBet(Player p, int field, int value)
        {
            if (CurrentChipsOnTable() + value > _maxChipsOnTable)
                throw new TooManyChipsException();
            if (!Colour.ContainsKey(p) && Colour.Count == MaxPlayers)
                throw new TableFullException();
            if (!Colour.ContainsKey(p))
                Colour.Add(p, (Colour)Colour.Count);
            if (!_betsByFields.ContainsKey(field))
                _betsByFields.Add(field, new List<Bet>());
            _betsByFields[field].Add(new Bet(p, value));
        }

        public IList<Bet> BetsByField(int field)
        {
            return _betsByFields.ContainsKey(field) ? _betsByFields[field] : NoBets;
        }
    }
}
