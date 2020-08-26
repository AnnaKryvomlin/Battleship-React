using System;
using BattleShip.DataAccess.EF;
using BattleShip.DataAccess.Interfaces;
using BattleShip.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BattleShip.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext db;
        private CoordinateRepository coordinateRepository;
        private FieldRepository fieldRepository;
        private GameRepository gameRepository;
        private MoveRepository moveRepository;
        private PlayerGameRepository playerGameRepository;
        private PlayerRepository playerRepository;
        private ShipRepository shipRepository;
        private StatisticRecordRepository statisticRecordRepository;
        private WinnerShipRepository winnerShipRepository;

        public UnitOfWork(DbContextOptions<ApplicationDbContext> options)
        {
            this.db = new ApplicationDbContext(options);
        }

        public IRepository<StatisticsRecord> StatisticsRecords
        {
            get
            {
                if (this.statisticRecordRepository == null)
                {
                    this.statisticRecordRepository = new StatisticRecordRepository(this.db);
                }

                return this.statisticRecordRepository;
            }
        }

        public IRepository<WinnerShip> WinnerShips
        {
            get
            {
                if (this.winnerShipRepository == null)
                {
                    this.winnerShipRepository = new WinnerShipRepository(this.db);
                }

                return this.winnerShipRepository;
            }
        }

        public IRepository<Coordinate> Coordinates
        {
            get
            {
                if (this.coordinateRepository == null)
                {
                    this.coordinateRepository = new CoordinateRepository(this.db);
                }

                return this.coordinateRepository;
            }
        }

        public IRepository<Field> Fields
        {
            get
            {
                if (this.fieldRepository == null)
                {
                    this.fieldRepository = new FieldRepository(this.db);
                }

                return this.fieldRepository;
            }
        }

        public IRepository<Game> Games
        {
            get
            {
                if (this.gameRepository == null)
                {
                    this.gameRepository = new GameRepository(this.db);
                }

                return this.gameRepository;
            }
        }

        public IRepository<Move> Moves
        {
            get
            {
                if (this.moveRepository == null)
                {
                    this.moveRepository = new MoveRepository(this.db);
                }

                return this.moveRepository;
            }
        }

        public IRepository<PlayerGame> PlayerGames
        {
            get
            {
                if (this.playerGameRepository == null)
                {
                    this.playerGameRepository = new PlayerGameRepository(this.db);
                }

                return this.playerGameRepository;
            }
        }

        public IRepository<Player> Players
        {
            get
            {
                if (this.playerRepository == null)
                {
                    this.playerRepository = new PlayerRepository(this.db);
                }

                return this.playerRepository;
            }
        }

        public IRepository<Ship> Ships
        {
            get
            {
                if (this.shipRepository == null)
                {
                    this.shipRepository = new ShipRepository(this.db);
                }

                return this.shipRepository;
            }
        }

        public void Save()
        {
            this.db.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.db.Dispose();
                }

                this.disposed = true;
            }
        }
    }
}