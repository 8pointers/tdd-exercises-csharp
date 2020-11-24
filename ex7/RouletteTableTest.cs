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
        Mock<RandomNumberGenerator> rng;
        TestTimer timer;
        Mock<WalletService> walletService;

        [SetUp]
        public void SetUp()
        {
            p = new Player();
            rng = new Mock<RandomNumberGenerator>();
            timer = new TestTimer();
            walletService = new Mock<WalletService>();
            walletService.Setup(ws => ws.IsAvailable(It.IsAny<Player>(), It.IsAny<int>())).Returns(true);
            rt = new RouletteTable(10000, rng.Object, timer, walletService.Object);
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
            RouletteTable rt = new RouletteTable(300, rng.Object, timer, walletService.Object);
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
            rng.Setup(d => d.Generate(30, 40)).Returns(33); //rng.NextOutcome = 33;
            rng.Setup(d => d.Generate(0, 36)).Returns(20); //rng.NextOutcome = 33;
            rt.Roll();
            timer.MoveTime(33001);
            Assert.IsFalse(rt.IsBallRolling);
        }

        [Test]
        public void Game_gets_a_random_outcome_when_the_ball_stops_rolling()
        {
            rng.Setup(g => g.Generate(0, 36)).Returns(13); //rng.NextOutcome = 13;
            //act
            rng.Setup(g => g.Generate(30, 40)).Returns(33);
            rt.Roll();
            timer.MoveTime(34000);
            //assert
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
            rng.Setup(g => g.Generate(30, 40)).Returns(33);

            Player p2 = new Player();
            Player p3 = new Player();
            rt.PlaceBet(p, new Field {FieldName = "A", WinningStrategy = loses.Object, PayoutCoefficient = 36}, 1);
            rt.PlaceBet(p2, new Field {FieldName = "B", WinningStrategy = wins.Object, PayoutCoefficient = 36}, 2);
            rt.PlaceBet(p3, new Field {FieldName = "C", WinningStrategy = loses.Object, PayoutCoefficient = 36}, 3);
            //act
            rt.Roll();
            timer.MoveTime(34000);
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
            rng.Setup(d => d.Generate(30, 40)).Returns(33); //rng.NextOutcome = 30;
            rt.Roll();
            timer.MoveTime(10001);
            Assert.Throws<NoMoreBetsException>(() => rt.PlaceBet(p, Field.ForNumber(1), 1));
        }

        [Test]
        public void System_will_deduct_any_chips_placed_on_the_table_from_the_available_amount()
        {
            rt.PlaceBet(p, Field.ForNumber(10), 10);
            walletService.Verify(ws => ws.AdjustBalance(p, -10));
        }

        [Test]
        public void If_a_player_attempts_to_place_more_chips_on_the_table_than_available_the_system_refuses_the_bet()
        {
            walletService.Setup(d => d.IsAvailable(p, 10)).Returns(false);
            Assert.Throws<NotEnoughChipsException>(() => rt.PlaceBet(p, Field.ForNumber(10), 10));
        }

        [Test]
        public void System_will_automatically_pay_out_winning_bets_with_multiplier_on_field()
        {
            var wins = new Mock<WinningStrategy>();
            wins.Setup(w => w.WinsOn(It.IsAny<int>())).Returns(true);
            rt.PlaceBet(p, new Field {FieldName = "B", WinningStrategy = wins.Object, PayoutCoefficient = 3}, 2);
            rng.Setup(r => r.Generate(30, 40)).Returns(33);
            rt.Roll();
            timer.MoveTime(34000);
            walletService.Verify(w => w.AdjustBalance(p, -2));
            walletService.Verify(w => w.AdjustBalance(p, 6));
        }

        [Test]
        public void System_will_not_pay_out_anything_for_losing_bets()
        {
            var loses = new Mock<WinningStrategy>();
            loses.Setup(w => w.WinsOn(It.IsAny<int>())).Returns(false);
            rt.PlaceBet(p, new Field {FieldName = "B", WinningStrategy = loses.Object, PayoutCoefficient = 3}, 2);
            rng.Setup(r => r.Generate(30, 40)).Returns(33);
            rt.Roll();
            timer.MoveTime(34000);
            walletService.Verify(w => w.AdjustBalance(p, It.IsAny<int>()), Times.Once());
        }

        [Test]
        public void multibets_paid_out_divided_by_number_of_fields()
        {
            var wins = new Mock<WinningStrategy>();
            wins.Setup(w => w.WinsOn(It.IsAny<int>())).Returns(true);
            var loses = new Mock<WinningStrategy>();
            loses.Setup(w => w.WinsOn(It.IsAny<int>())).Returns(false);
            rt.PlaceBet(p, new Field[]
            {
                new Field {FieldName = "1", WinningStrategy = wins.Object, PayoutCoefficient = 36},
                new Field {FieldName = "2", WinningStrategy = loses.Object, PayoutCoefficient = 36}
            }, 1);
            rng.Setup(r => r.Generate(30, 40)).Returns(33);
            rt.Roll();
            timer.MoveTime(34000);
            walletService.Verify(w => w.AdjustBalance(p, -1));
            walletService.Verify(w => w.AdjustBalance(p, 18));
        }
    }
}