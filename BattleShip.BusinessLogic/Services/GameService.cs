using System.Collections.Generic;
using System.Linq;
using BattleShip.BusinessLogic.Interfaces;
using BattleShip.DataAccess.Interfaces;
using BattleShip.Models.Entities;

namespace BattleShip.BusinessLogic.Services
{
    public class GameService : IGameService
    {
        private readonly IStatisticsService statisticsService;

        private readonly IUnitOfWork db;

        public GameService(IUnitOfWork uow, IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
            this.db = uow;
        }

        public int CreateGame(int playerId)
        {
            Game game = new Game
            {
                CurrentMovePlayerId = playerId
            };

            // Check if someone wait a game. If yes - add field and player to the game.
            var field = this.db.Fields
                .GetAll()
                .Where(f => f.GameId == null && f.PlayerId != playerId)
                .FirstOrDefault();

            this.db.Games.Create(game);
            this.db.Save();
            game.PlayerGames.Add(new PlayerGame { Game = game, PlayerId = playerId });

            if (field != null)
            {
                game.Status = "In Process";
                game.PlayerGames.Add(new PlayerGame { Game = game, PlayerId = field.PlayerId });
                field.Game = game;
                this.db.Fields.Update(field);
            }
            else
            {
                game.Status = "Search";
            }

            this.db.Save();
            return game.Id;
        }

        public int CreateField(int playerId, int? gameId)
        {
            Field field = new Field
            {
                PlayerId = playerId
            };

            if (gameId == null)
            {
                var playerGame = this.db.PlayerGames
                    .GetAll()
                    .Where(pg => pg.PlayerId != playerId && this.db.Games.Get(pg.GameId).Status == "Search")
                    .FirstOrDefault();

                if (playerGame != null)
                {
                    field.GameId = playerGame.GameId;
                    this.db.PlayerGames.Create(new PlayerGame { GameId = playerGame.GameId, PlayerId = playerId });
                    var game = playerGame.Game;
                    game.Status = "In Process";
                    this.db.Games.Update(game);
                }
            }
            else
            {
                field.GameId = gameId;
            }

            this.db.Fields.Create(field);
            this.db.Save();
            this.CreateCoordinats(field.Id);
            return field.Id;
        }

        public void AddShipsToField(int fieldId, List<Ship> ships)
        {
            foreach (var s in ships)
            {
                Ship ship = new Ship
                {
                    Size = s.Coordinates.Count(),
                    FieldId = fieldId
                };
                this.db.Ships.Create(ship);
                foreach (var c in s.Coordinates)
                {
                    var coordinate = this.db.Coordinates.GetAll().Where(coord => coord.X == c.X && coord.Y == c.Y && coord.FieldId == fieldId).FirstOrDefault();
                    coordinate.Ship = ship;
                    this.db.Coordinates.Update(coordinate);
                }
            }

            this.db.Save();
        }

        // For private account
        public List<Game> GetPlayerGames(int playerId)
        {
            return this.db.PlayerGames
                .GetAll()
                .Where(pg => pg.PlayerId == playerId && pg.Game.Status == "In Process")
                .Select(pg => pg.Game)
                .ToList();
        }

        // For moves in the game.
        public int MarkCell(Coordinate coordinate, int gameId, int playerId)
        {
            var game = this.GetGame(gameId);
            coordinate.Mark = true;
            this.db.Coordinates.Update(coordinate);
            game.CurrentMovePlayerId = this.db.PlayerGames
                .GetAll()
                .Where(pg => pg.PlayerId != playerId && pg.GameId == game.Id)
                .FirstOrDefault().PlayerId;
            this.db.Games.Update(game);
            this.db.Save();
            return game.CurrentMovePlayerId;
        }

        // Get coordinate of enemy field for move
        public Coordinate GetCoordinate(int playerId, int gameId, int x, int y)
        {
            var coordinate = this.db.Coordinates.GetAll()
                .Where(coord => coord.Field.GameId == gameId && coord.Field.PlayerId != playerId && coord.X == x && coord.Y == y)
                .FirstOrDefault();
            return coordinate;
        }

        public Game GetGame(int gameId)
        {
            return this.db.Games.Get(gameId);
        }

        // Get coords for player and for player's enemy
        public List<Coordinate> GetCoordinatesForGame(int playerId, int gameId, bool isMine = true)
        {
            int id = isMine ? playerId : this.GetEnemyId(gameId, playerId);
            var coordinates = this.db.Coordinates.GetAll()
                .Where(c => c.Field.GameId == gameId && c.Field.PlayerId == id)
                .ToList();
            return coordinates;
        }

        // Check that enemy has ships after move
        public bool IsGameCanContinues(int gameId, int playerId)
        {
            int i = 0;
            var enemyCoordinates = this.GetCoordinatesForGame(playerId, gameId, false);
            i += this.CheckIfCoordinatesHaveShips(enemyCoordinates);
            if (i != 1)
            {
                this.AddFinishedStatus(gameId);
                this.statisticsService.AddFinishedGame(gameId, playerId);
            }

            return i == 1;
        }

        public string MoveRecord(int playerId, int gameId, int x, int y)
        {
            var player = this.db.Players.Get(playerId);
            string record = player.UserName + " сделал ход на y = " + y + ", x = " + x;
            int? shipId = this.db.Coordinates.GetAll()
                .Where(coord => coord.Field.GameId == gameId && coord.Field.PlayerId != playerId && coord.X == x && coord.Y == y)
                .FirstOrDefault().ShipId;
            if (shipId != null)
            {
                var ship = this.db.Ships.Get(shipId.Value);
                bool flag = ship.Coordinates.Any(coord => !coord.Mark);

                if (flag)
                {
                    record += " (ранил)";
                }
                else
                {
                    record += " (убил)";
                }
            }
            else
            {
                record += " (мимо)";
            }

            Move move = new Move { PlayerMove = record, GameId = gameId };
            this.db.Moves.Create(move);
            this.db.Save();
            return record;
        }

        public List<Move> GetAllRecords(int gameId)
        {
            return this.db.Moves.GetAll().Where(m => m.GameId == gameId).ToList();
        }

        public void TrowUpTheTowel(int gameId, int playerId)
        {
            this.AddFinishedStatus(gameId);
            this.statisticsService.AddFinishedGame(gameId, this.GetEnemyId(gameId, playerId));
        }

        private void AddFinishedStatus(int gameId)
        {
            var game = this.GetGame(gameId);
            game.Status = "Finished";
            this.db.Games.Update(game);
            this.db.Save();
        }

        private int CheckIfCoordinatesHaveShips(List<Coordinate> coordinates)
        {
            return coordinates.Any(coord => coord.ShipId != null && !coord.Mark) ? 1 : 0;
        }

        private int GetEnemyId(int gameId, int playerId)
        {
            return this.db.PlayerGames.GetAll()
                .Where(pg => pg.GameId == gameId && pg.PlayerId != playerId)
                .FirstOrDefault().PlayerId;
        }

        private void CreateCoordinats(int fieldId)
        {
            for (int x = 1; x <= 10; x++)
            {
                for (int y = 1; y <= 10; y++)
                {
                    var coordinate = new Coordinate
                    {
                        X = x,
                        Y = y,
                        FieldId = fieldId
                    };
                    this.db.Coordinates.Create(coordinate);
                              this.db.Save();
                }
            }
        }
    }
}