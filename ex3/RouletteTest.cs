using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace roulette
{
    [TestFixture]
    public class RouletteTest
    {
        private readonly Player _player = new Player();
        private readonly Player[] _playerOneToEight = Enumerable.Range(1, 8).Select(_ => new Player()).ToArray();
        private RouletteTable _rouletteTable = new RouletteTable(10000);

        [SetUp]
        public void SetUp()
        {
            _rouletteTable = new RouletteTable(10000);
        }

        [Test]
        public void Player_can_place_bets_on_number_fields()
        {
            _rouletteTable.PlaceBet(_player, 17, 200);

            Assert.AreEqual(new List<Bet>{new Bet(_player, 200)}, _rouletteTable.BetsByField(17));
        }

        [Test]
        public void Player_can_place_bets_on_different_fields_and_with_different_values()
        {
            _rouletteTable.PlaceBet(_player, 17, 200);
            _rouletteTable.PlaceBet(_player, 27, 600);
            _rouletteTable.PlaceBet(_player, 27, 1000);

            Assert.AreEqual(new List<Bet>{new Bet(_player, 200)}, _rouletteTable.BetsByField(17));
            Assert.AreEqual(new List<Bet>{new Bet(_player, 600), new Bet(_player, 1000)}, _rouletteTable.BetsByField(27));
        }

        [Test]
        public void Total_number_of_chips_is_limited_by_the_table_and_the_table_can_refuse_a_bet_if_there_are_too_many_chips()
        {
            RouletteTable rouletteTable = new RouletteTable(300);
            rouletteTable.PlaceBet(_player, 17, 200);

            Assert.Throws<TooManyChipsException>(() => rouletteTable.PlaceBet(_player, 20, 200));
            Assert.AreEqual(new List<Bet>(), rouletteTable.BetsByField(20));
        }

        [Test]
        public void Up_to_eight_players_can_join_a_game()
        {
            foreach (var player in _playerOneToEight)
                _rouletteTable.PlaceBet(player, 20, 100);

            Assert.Throws<TableFullException>(() => _rouletteTable.PlaceBet(_player, 20, 100));
        }

        [Test]
        public void Each_player_is_assigned_a_different_colour()
        {
            foreach (var player in _playerOneToEight)
                _rouletteTable.PlaceBet(player, 20, 100);

            Assert.AreEqual(
                Enum.GetValues(typeof(Colour)),
                _playerOneToEight.Select(player => _rouletteTable.Colour[player]).ToArray()
            );
        }
    }
}
