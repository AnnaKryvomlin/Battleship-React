namespace BattleShip.DataAccess.Interfaces
{
    using System;
    using BattleShip.Models.Entities;

    public interface IUnitOfWork : IDisposable
    {
        IRepository<Coordinate> Coordinates { get; }

        IRepository<Field> Fields { get; }

        IRepository<Game> Games { get; }

        IRepository<Move> Moves { get; }

        IRepository<Player> Players { get; }

        IRepository<PlayerGame> PlayerGames { get; }

        IRepository<Ship> Ships { get; }

        IRepository<StatisticsRecord> StatisticsRecords { get; }

        IRepository<WinnerShip> WinnerShips { get; }

        void Save();
    }
}
