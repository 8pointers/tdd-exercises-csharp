using System;
using System.Collections.Generic;
using System.Linq;

namespace roulette
{
    public class TableFullException : Exception
    {}

    public class TooManyChipsException : Exception
    {}

    public class RouletteTable
    {
        private const int MaxPlayers = 8;
        private static readonly IList<Bet> NoBets = new List<Bet>();
        private readonly IDictionary<Field, List<Bet>> _betsByFields = new Dictionary<Field, List<Bet>>();
        private readonly int _maxChipsOnTable;

        public RouletteTable(int maxChipsOnTable)
        {
            _maxChipsOnTable = maxChipsOnTable;
        }

        public IDictionary<Player, Colour> Colour { get; } = new Dictionary<Player, Colour>();

        private static BetType GetBetType(Field[] fields)
        {
            return fields.Length switch
            {
                1 => BetType.SINGLE,
                2 => BetType.SPLIT,
                _ => throw new ArgumentException("Unsupported bet type for " + fields + " fields")
            };
        }

        public void PlaceBet(Player player, Field[] fields, int value)
        {
            var currentChipsOnTable = _betsByFields.Values.SelectMany(bets => bets).Sum(bet => bet.Value);
            if (currentChipsOnTable + value > _maxChipsOnTable)
                throw new TooManyChipsException();
            if (!Colour.ContainsKey(player) && Colour.Count == MaxPlayers)
                throw new TableFullException();

            if (!Colour.ContainsKey(player))
                Colour.Add(player, (Colour)Colour.Count);
            foreach (var field in fields)
            {
                if (!_betsByFields.ContainsKey(field))
                    _betsByFields[field] = new List<Bet>();
                _betsByFields[field].Add(new Bet(player, value, GetBetType(fields)));
            }
        }

        public void PlaceBet(Player player, Field field, int value)
        {
            PlaceBet(player, new[] {field}, value);
        }

        public void PlaceBet(Player player, int field, int value)
        {
            PlaceBet(player, new[] {Field.ForNumber(field)}, value);
        }

        public IList<Bet> BetsByField(Field field)
        {
            return _betsByFields.ContainsKey(field) ? _betsByFields[field] : NoBets;
        }

        public IList<Bet> BetsByField(int field)
        {
            return BetsByField(Field.ForNumber(field));
        }
    }
}
