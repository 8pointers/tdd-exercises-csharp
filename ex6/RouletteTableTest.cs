using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace roulette
{
    [TestFixture]
    public class RouletteTableTest
    {
        Player p;
        RouletteTable rt;
        Mock<RandomNumberGenerator> rngMock;
        TestTimer timer;

        [SetUp]
        public void setUp()
        {
            p = new Player();
            rngMock = new Mock<RandomNumberGenerator>();
            timer = new TestTimer();
            rt = new RouletteTable(10000, rngMock.Object, timer);
        }

        [Test]
        public void player_can_place_bets_on_number_fields()
        {
            rt.PlaceBet(p, 17, 200);
            Assert.AreEqual(new Bet(p, 200), rt.BetsByField(17)[0]);
        }

        [Test]
        public void player_can_place_bets_on_different_fields_and_with_different_values()
        {
            rt.PlaceBet(p, 17, 200);
            rt.PlaceBet(p, 2, 600);
            rt.PlaceBet(p, 30, 1000);
            Assert.AreEqual(new Bet(p, 200), rt.BetsByField(17)[0]);
            Assert.AreEqual(new Bet(p, 600), rt.BetsByField(2)[0]);
            Assert.AreEqual(new Bet(p, 1000), rt.BetsByField(30)[0]);
        }

        [Test]
        public void
            Total_number_of_chips_is_limited_by_the_table_and_the_table_can_refuse_a_bet_if_there_are_too_many_chips()
        {
            RouletteTable rt = new RouletteTable(300, rngMock.Object, timer);
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
                rt.PlaceBet(players[i], 20, 100);
            }

            Player nine = new Player();
            Assert.Throws<TableFullException>(() => rt.PlaceBet(nine, 20, 100));
        }

        [Test]
        public void Each_player_is_assigned_a_different_colour()
        {
            Player[] players = new Player[8];
            for (int i = 0; i < 8; i++)
            {
                players[i] = new Player();
                rt.PlaceBet(players[i], 20, 100);
            }

            for (int i = 0; i < players.Length - 1; i++)
            for (int j = i + 1; j < players.Length; j++)
                Assert.IsTrue(rt.Colour[players[i]] != rt.Colour[players[j]]);
        }

        [Test]
        public void Player_can_place_bet_on_ODD_and_EVEN()
        {
            rt.PlaceBet(p, Field.ODD, 10);
            Assert.AreEqual(new Bet(p, 10), rt.BetsByField(Field.ODD)[0]);
        }

        [Test]
        public void Player_can_place_split_bets_which_cover_both_fields()
        {
            rt.PlaceBet(p, new Field[] {Field.ForNumber(1), Field.ForNumber(2)}, 10);
            Assert.AreEqual(new Bet(p, 10, BetType.SPLIT), rt.BetsByField(Field.ForNumber(1))[0]);
            Assert.AreEqual(new Bet(p, 10, BetType.SPLIT), rt.BetsByField(Field.ForNumber(2))[0]);
        }

        [Test]
        public void Ball_starts_rolling_when_all_players_signal_done()
        {
            //arrange
            Player p2 = new Player();
            Player p3 = new Player();
            rt.PlaceBet(p, Field.ForNumber(1), 10);
            rt.PlaceBet(p2, Field.ForNumber(2), 10);
            rt.PlaceBet(p3, Field.ForNumber(3), 10);
            //act
            rt.Done(p);
            rt.Done(p2);
            rt.Done(p3);
            Assert.IsTrue(rt.IsBallRolling);
        }

        [Test]
        public void Ball_rolls_for_a_random_time_between_30_and_40_seconds()
        {
            rngMock.Setup(rng => rng.Generate(30, 40)).Returns(33); //rng.NextOutcome = 33;
            rngMock.Setup(rng => rng.Generate(0, 36)).Returns(20); //rng.NextOutcome = 33;
            rt.Roll();
            timer.MoveTime(33001);
            Assert.IsFalse(rt.IsBallRolling);
        }

        [Test]
        public void Game_gets_a_random_outcome_when_the_ball_stops_rolling()
        {
            rngMock.Setup(rng => rng.Generate(0, 36)).Returns(13); //rng.NextOutcome = 13;
            rt.StopRolling();
            Assert.AreEqual(13, rt.BallPosition);
        }

        [Test]
        public void Winning_bets_contain_all_bets_with_field_winning_strategy_covering_ball_position()
        {
            Mock<WinningStrategy> wins = new Mock<WinningStrategy>();
            Mock<WinningStrategy> loses = new Mock<WinningStrategy>();
            //arrange
            wins.Setup(d => d.WinsOn(It.IsAny<int>())).Returns(true);
            loses.Setup(d => d.WinsOn(It.IsAny<int>())).Returns(false);

            Player p2 = new Player();
            Player p3 = new Player();
            rt.PlaceBet(p, new Field {FieldName = "A", WinningStrategy = loses.Object}, 1);
            rt.PlaceBet(p2, new Field {FieldName = "B", WinningStrategy = wins.Object}, 2);
            rt.PlaceBet(p3, new Field {FieldName = "C", WinningStrategy = loses.Object}, 3);
            //act
            rt.StopRolling();
            //assert
            IList<Bet> winningBets = new List<Bet>();
            winningBets.Add(new Bet(p2, 2));
            Assert.AreEqual(winningBets, rt.WinningBets);
        }

        [Test]
        public void players_can_place_bets_when_the_ball_starts_rolling_up_to_10_seconds()
        {
            rt.Roll();
            timer.MoveTime(5000);
            rt.PlaceBet(p, Field.ForNumber(1), 1);
        }

        [Test]
        public void Players_cannot_place_bets_when_the_ball_starts_rolling_after_10_seconds()
        {
            rngMock.Setup(d => d.Generate(30, 40)).Returns(33); //rng.NextOutcome = 30;
            rt.Roll();
            timer.MoveTime(10001);
            Assert.Throws<NoMoreBetsException>(() => rt.PlaceBet(p, Field.ForNumber(1), 1));
        }
    }
}