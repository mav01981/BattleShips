using Moq;
using Project.BattleShip;
using Project.BattleShip.Models;
using System;
using System.Linq;
using Xunit;
using static Project.BattleShip.Models.Game;

namespace Project.Tests
{
    public class BattleShipTests
    {
        [Fact]
        public void Create_10x10_board()
        {
            Board board = new Board(new BoardDimension(10, 10));

            int numberOfSquares = board.coordinates.Count;

            Assert.Equal(100, numberOfSquares);
        }

        [Fact]
        public void Place_ships_on_board()
        {
            for (int i = 0; i <= 99; i++)
            {
                Game game = new Game(new BoardDimension(10, 10));

                var ships = game.coordinates.Where(x => x.ShipIdentifier > 0)
                    .GroupBy(x => x.ShipIdentifier,
                    (key, group) => new { Id = key, Items = group.ToList() }).ToList();

                Assert.Equal(5, ships.Where(x => x.Id == 1).Select(x => x.Items.Count()).First());
                Assert.Equal(4, ships.Where(x => x.Id == 2).Select(x => x.Items.Count()).First());
                Assert.Equal(4, ships.Where(x => x.Id == 3).Select(x => x.Items.Count()).First());
                Assert.Equal(3, ships.Count());
            }
        }

        [Fact]
        public void Place_ships_in_random_positions_on_board()
        {
            Game game = new Game(new BoardDimension(10, 10));

            var shipsA = game.coordinates.Where(x => x.ShipIdentifier > 0)
                .GroupBy(x => x.ShipIdentifier,
                (key, group) => new { Id = key, Items = group.ToList() })
                .OrderBy(x => x.Id)
                .ToList();

            game = new Game(new BoardDimension(10, 10));

            var shipsB = game.coordinates.Where(x => x.ShipIdentifier > 0)
              .GroupBy(x => x.ShipIdentifier,
              (key, group) => new { Id = key, Items = group.ToList() })
              .OrderBy(x => x.Id)
              .ToList();

            Assert.NotEqual(shipsA[0].Items[0].Column, shipsB[0].Items[0].Column);
        }

        [Fact]
        public void Place_shot_as_a_miss()
        {
            var mockEvent = new Mock<ShipEvent>();

            Game game = new Game(new BoardDimension(10, 10));
            game.RaiseEvent += mockEvent.Object;

            var result = Shot.Hit;
            while (result == Shot.Hit)
            {
                Random random = new Random();
                result = game.TakeShot(game.axis[random.Next(1, 10)], random.Next(1, 10));
            }

            Assert.Equal(Shot.Miss, result);
        }

        [Fact]
        public void Place_shot_as_a_hit()
        {
            var mockEvent = new Mock<ShipEvent>();

            Game game = new Game(new BoardDimension(10, 10));
            game.RaiseEvent += mockEvent.Object;

            var result = Shot.Miss;
            while (result == Shot.Miss)
            {
                Random random = new Random();
                result = game.TakeShot(game.axis[random.Next(1, 10)], random.Next(1, 10));
            }

            Assert.Equal(Shot.Hit, result);
        }

        [Fact]
        public void Ship_hit_count_matches_ship_size()
        {
            var mockEvent = new Mock<ShipEvent>();
            Game game = new Game(new BoardDimension(10, 10));
            game.RaiseEvent += mockEvent.Object;

            while (game.Ships.Where(x => x.IsSunk==false).Count() > 0)
            {
                Random random = new Random();
                game.TakeShot(game.axis[random.Next(1, 10)], random.Next(1, 10));
            }

            for (int i = 0; i <= 99; i++)
            {
                Random random = new Random();
                game.TakeShot(game.axis[random.Next(1, 10)], random.Next(1, 10));
            }

            Assert.Equal(5, game.Ships.Where(x => x.Id == 1 && x.IsSunk == true).Select(x => x.Hits).First());
        }

        [Fact]
        public void Place_shot_as_Win_Game()
        {
            var mockEvent = new Mock<ShipEvent>();

            Game game = new Game(new BoardDimension(10, 10));
            game.RaiseEvent += mockEvent.Object;

            while (game.Ships.Where(x => x.IsSunk == false).Count() == 0)
            {
                Random random = new Random();
                game.TakeShot(game.axis[random.Next(1, 10)], random.Next(1, 10));
            }

            Assert.False(game.Ships.Where(x => x.IsSunk == false).Count() == 0);
        }

        [Fact]
        public void Place_shot_as_Sink_ship()
        {
            var mockEvent = new Mock<ShipEvent>();
            Game game = new Game(new BoardDimension(10, 10));
            game.RaiseEvent += mockEvent.Object;

            while (game.Ships.Where(x => x.IsSunk).Count() == 0)
            {
                Random random = new Random();
                game.TakeShot(game.axis[random.Next(1, 10)], random.Next(1, 10));
            }

            Assert.True(game.Ships.Where(x => x.IsSunk == true).Any());
        }

        [Fact]
        public void Verify_shot_call()
        {
            var mockDimensions = new Mock<IBoardDimension>();
            var mockObject = new Mock<IGame>();

            mockObject.Object.TakeShot('A', 1);

            mockObject.Verify(x => x.TakeShot('A', 1), Times.Once);
        }
    }
}
