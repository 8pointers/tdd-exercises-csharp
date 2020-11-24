using System.Collections.Generic;
using NUnit.Framework;

namespace roulette
{
    [TestFixture]
    public class RouletteTableTest
    {
        Player player;
        RouletteTable rouletteTable;
        TestRandomNumberGenerator rng;
        TestTimer timer;

        [SetUp]
        public void setUp()
        {
            player = new Player();
            rng = new TestRandomNumberGenerator();
            timer = new TestTimer();
            rouletteTable = new RouletteTable(10000, rng, timer);
        }

        [Test]
        public void player_can_place_bets_on_number_fields()
        {
            rouletteTable.PlaceBet(player, 17, 200);
            Assert.AreEqual(new Bet(player, 200), rouletteTable.BetsByField(17)[0]);
        }

        [Test]
        public void player_can_place_bets_on_different_fields_and_with_different_values()
        {
            rouletteTable.PlaceBet(player, 17, 200);
            rouletteTable.PlaceBet(player, 2, 600);
            rouletteTable.PlaceBet(player, 30, 1000);
            Assert.AreEqual(new Bet(player, 200), rouletteTable.BetsByField(17)[0]);
            Assert.AreEqual(new Bet(player, 600), rouletteTable.BetsByField(2)[0]);
            Assert.AreEqual(new Bet(player, 1000), rouletteTable.BetsByField(30)[0]);
        }

        [Test]
        public void
            Total_number_of_chips_is_limited_by_the_table_and_the_table_can_refuse_a_bet_if_there_are_too_many_chips()
        {
            RouletteTable rt = new RouletteTable(300, rng, timer);
            Player p = new Player();
            rt.PlaceBet(p, 17, 200);
            bool exceptionThrown = false;
            try
            {
                rt.PlaceBet(new Player(), 20, 200);
            }
            catch (TooManyChipsException)
            {
                exceptionThrown = true;
            }

            // the following should be true
            Assert.AreEqual(0, rt.BetsByField(20).Count);
            Assert.IsTrue(exceptionThrown);
        }

        [Test]
        public void Up_to_eight_players_can_join_a_game()
        {
            Player[] players = new Player[8];
            for (int i = 0; i < 8; i++)
            {
                players[i] = new Player();
                rouletteTable.PlaceBet(players[i], 20, 100);
            }

            Player nine = new Player();
            Assert.Throws<TableFullException>(() => rouletteTable.PlaceBet(nine, 20, 100));
        }

        [Test]
        public void Each_player_is_assigned_a_different_colour()
        {
            Player[] players = new Player[8];
            for (int i = 0; i < 8; i++)
            {
                players[i] = new Player();
                rouletteTable.PlaceBet(players[i], 20, 100);
            }

            for (int i = 0; i < players.Length - 1; i++)
            for (int j = i + 1; j < players.Length; j++)
                Assert.IsTrue(rouletteTable.Colour[players[i]] != rouletteTable.Colour[players[j]]);
        }

        [Test]
        public void Player_can_place_bet_on_ODD_and_EVEN()
        {
            rouletteTable.PlaceBet(player, Field.ODD, 10);
            Assert.AreEqual(new Bet(player, 10), rouletteTable.BetsByField(Field.ODD)[0]);
        }

        [Test]
        public void Player_can_place_split_bets_which_cover_both_fields()
        {
            rouletteTable.PlaceBet(player, new Field[] {Field.ForNumber(1), Field.ForNumber(2)}, 10);
            Assert.AreEqual(new Bet(player, 10, BetType.SPLIT), rouletteTable.BetsByField(Field.ForNumber(1))[0]);
            Assert.AreEqual(new Bet(player, 10, BetType.SPLIT), rouletteTable.BetsByField(Field.ForNumber(2))[0]);
        }

        [Test]
        public void Ball_starts_rolling_when_all_players_signal_done()
        {
            //arrange
            Player p2 = new Player();
            Player p3 = new Player();
            rouletteTable.PlaceBet(player, Field.ForNumber(1), 10);
            rouletteTable.PlaceBet(p2, Field.ForNumber(2), 10);
            rouletteTable.PlaceBet(p3, Field.ForNumber(3), 10);
            //act
            rouletteTable.Done(player);
            rouletteTable.Done(p2);
            rouletteTable.Done(p3);
            Assert.IsTrue(rouletteTable.IsBallRolling);
        }

        [Test]
        public void Ball_rolls_for_a_random_time_between_30_and_40_seconds()
        {
            rng.NextOutcome = 33;
            rouletteTable.Roll();
            timer.MoveTime(33001);
            Assert.IsFalse(rouletteTable.IsBallRolling);
        }

        [Test]
        public void Game_gets_a_random_outcome_when_the_ball_stops_rolling()
        {
            //act
            rng.NextOutcome = 13;
            rouletteTable.StopRolling();
            //assert
            Assert.AreEqual(13, rouletteTable.BallPosition);
        }

        [Test]
        public void Winning_bets_contain_all_bets_with_field_winning_strategy_covering_ball_position()
        {
            //arrange
            Player p2 = new Player();
            Player p3 = new Player();
            rouletteTable.PlaceBet(player, new Field {FieldName = "A", WinningStrategy = new TestWinningStrategy(2, false)}, 1);
            rouletteTable.PlaceBet(p2, new Field {FieldName = "B", WinningStrategy = new TestWinningStrategy(2, true)}, 2);
            rouletteTable.PlaceBet(p3, new Field {FieldName = "C", WinningStrategy = new TestWinningStrategy(2, false)}, 3);
            rng.NextOutcome = 2;
            //act
            rouletteTable.StopRolling();
            //assert
            IList<Bet> winningBets = new List<Bet>();
            winningBets.Add(new Bet(p2, 2));
            Assert.AreEqual(winningBets, rouletteTable.WinningBets);
        }

        [Test]
        public void players_can_place_bets_when_the_ball_starts_rolling_up_to_10_seconds()
        {
            rouletteTable.Roll();
            timer.MoveTime(5000);
            rouletteTable.PlaceBet(player, Field.ForNumber(1), 1);
        }

        [Test]
        public void Players_cannot_place_bets_when_the_ball_starts_rolling_after_10_seconds()
        {
            rng.NextOutcome = 30000;
            rouletteTable.Roll();
            timer.MoveTime(10001);
            Assert.Throws<NoMoreBetsException>(() => rouletteTable.PlaceBet(player, Field.ForNumber(1), 1));
        }
    }
}
