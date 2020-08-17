using BattleShip.BusinessLogic.Interfaces;
using BattleShip.DataAccess.Interfaces;
using BattleShip.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BattleShip.BusinessLogic.Services
{
    public class GameService : IGameService
    {
        private IUnitOfWork db;
        public IStatisticsService statisticsService;

        public GameService(IUnitOfWork uow, IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
            this.db = uow;
        }

        public int CreateGame(int playerId)
        {
            Game game = new Game();
            game.CurrentMovePlayerId = playerId;
            // Check if someone wait a game. If yes - add field and player to the game.
            var field = this.db.Fields.GetAll().Where(f => f.GameId == null).FirstOrDefault();
            if (field != null)
            {
                game.Status = "In Process";
                game.PlayerGames.Add(new PlayerGame { GameId = game.Id, PlayerId = field.PlayerId });
                this.db.Games.Create(game);
                this.db.Save();
                field.GameId = game.Id;
                this.db.Fields.Update(field);
            }
            else
            {
                game.Status = "Search";
                this.db.Games.Create(game);
            }

            game.PlayerGames.Add(new PlayerGame { GameId = game.Id, PlayerId = playerId });
            this.db.Save();
            return game.Id;
        }

        public int CreateField(int playerId, int? gameId)
        {
            Field field = new Field();
            field.PlayerId = playerId;
            if (gameId == null)
            {
                var playerGame = this.db.PlayerGames.GetAll().Where(pg => pg.PlayerId != playerId && db.Games.Get(pg.GameId).Status == "Search").FirstOrDefault();
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
            int fieldId = field.Id;
            CreateCoordinats(fieldId);
            return fieldId;
        }

        private void CreateCoordinats(int fieldId)
        {
            for (int x = 1; x <= 10; x++)
                for (int y = 1; y <= 10; y++)
                {
                    Coordinate coordinate = new Coordinate();
                    coordinate.X = x;
                    coordinate.Y = y;
                    coordinate.FieldId = fieldId;
                    this.db.Coordinates.Create(coordinate);
                }
            this.db.Save();
        }

        public void AddShipsToField(int fieldId, List<Ship> ships)
        {
            foreach (var s in ships)
            {
                Ship ship = new Ship();
                ship.Size = s.Coordinates.Count();
                ship.FieldId = fieldId;
                this.db.Ships.Create(ship);
                this.db.Save();
                foreach (var c in s.Coordinates)
                {
                    var coordinate = this.db.Coordinates.GetAll().Where(coord => coord.X == c.X && coord.Y == c.Y && coord.FieldId == fieldId).FirstOrDefault();
                    coordinate.ShipId = ship.Id;
                    this.db.Coordinates.Update(coordinate);                  
                }
            }

            this.db.Save();
        }

        // For private account
        public List<Game> GetPlayerGames(int playerId)
        {
            var player = this.db.Players.Get(playerId);
            var playerGames = this.db.PlayerGames.GetAll().Where(pg => pg.PlayerId == playerId).ToList();
            List<Game> games = new List<Game>();
            foreach(var pg in playerGames)
            {
                if(pg.Game.Status == "In Process")
                    games.Add(pg.Game);
            }

            return games;
        }

        // For moves in the game. 
        public int MarkCell(Coordinate coordinate, int gameId, int playerId)
        {
            var game = GetGame(gameId);
            coordinate.Mark = true;
            this.db.Coordinates.Update(coordinate);
            game.CurrentMovePlayerId = this.db.PlayerGames.GetAll().Where(pg => pg.PlayerId != playerId && pg.GameId == game.Id).FirstOrDefault().PlayerId;
            this.db.Games.Update(game);
            this.db.Save();
            return game.CurrentMovePlayerId;
        }

        // Get coordinate of enemy field for move
        public Coordinate GetCoordinate(int playerId, int gameId, int x, int y)
        {
            var game = GetGame(gameId);
            var coordinate = this.db.Coordinates.GetAll()
                .Where(coord => coord.Field.GameId == gameId && coord.Field.PlayerId != playerId && coord.X == x && coord.Y == y).FirstOrDefault();           
            return coordinate;
        }

        public Game GetGame(int gameId)
        {
            return this.db.Games.Get(gameId);
        }

        private int GetEnemyId(int gameId, int playerId)
        {
            int enemyId = this.db.PlayerGames.GetAll().Where(pg => pg.GameId == gameId && pg.PlayerId != playerId).FirstOrDefault().PlayerId;
            return enemyId;
        }

        // Get coords for player and for player's enemy
        public List<Coordinate> GetCoordinatesForGame(int playerId, int gameId, bool isMine = true)
        {
            int id = isMine ? playerId : GetEnemyId(gameId, playerId);          
            var coordinates = this.db.Coordinates.GetAll().Where(c => c.Field.GameId == gameId && c.Field.PlayerId == id).ToList();
            return coordinates;
        }
       
        // Check that enemy has ships after move
        public bool IsGameCanContinues(int gameId, int playerId)
        {
            int i = 0;
            var enemyCoordinates = GetCoordinatesForGame(playerId, gameId, false);
            i += CheckIfCoordinatesHaveShips(enemyCoordinates);
            if(i != 1)
            {
                AddFinishedStatus(gameId);
                statisticsService.AddFinishedGame(gameId, playerId);
            }

            return i == 1;
        }

        private int CheckIfCoordinatesHaveShips(List<Coordinate> coordinates)
        {
            foreach(var coord in coordinates)
            {
                if (coord.ShipId != null && !coord.Mark)
                    return 1;
            }
            return 0;
        }

        public string MoveRecord(int playerId, int gameId, int x, int y)
        {
            var player = this.db.Players.Get(playerId);
            string record = player.UserName + " сделал ход на y = " + y + ", x = " + x;
            int? shipId = db.Coordinates.GetAll().Where(coord => coord.Field.GameId == gameId && coord.Field.PlayerId != playerId && coord.X == x && coord.Y == y).FirstOrDefault().ShipId;
            if (shipId != null)
            {
                var ship = db.Ships.Get(shipId.Value);
                bool flag = false;
                foreach (var coord in ship.Coordinates)
                {
                    if (!coord.Mark)
                    {
                        flag = true;
                        break;
                    }
                }

                if (flag)
                    record += " (ранил)";
                else
                    record += " (убил)";
            }
            else
                record += " (мимо)";
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
            AddFinishedStatus(gameId);
            statisticsService.AddFinishedGame(gameId, GetEnemyId(gameId, playerId));
        }

        private void AddFinishedStatus(int gameId)
        {
            var game = GetGame(gameId);
            game.Status = "Finished";
            this.db.Games.Update(game);
            this.db.Save();
        }
    }
}
