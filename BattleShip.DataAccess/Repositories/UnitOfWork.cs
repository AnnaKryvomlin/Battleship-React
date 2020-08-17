using BattleShip.DataAccess.EF;
using BattleShip.Models.Entities;
using BattleShip.DataAccess.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BattleShip.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;
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
                    this.statisticRecordRepository = new StatisticRecordRepository(db);
                return this.statisticRecordRepository;
            }
        }

        public IRepository<WinnerShip> WinnerShips
        {
            get
            {
                if (this.winnerShipRepository == null)
                    this.winnerShipRepository = new WinnerShipRepository(db);
                return this.winnerShipRepository;
            }
        }

        public IRepository<Coordinate> Coordinates
        {
            get
            {
                if (this.coordinateRepository == null)
                    this.coordinateRepository = new CoordinateRepository(db);
                return this.coordinateRepository;
            }
        }

        public IRepository<Field> Fields
        {
            get
            {
                if (this.fieldRepository == null)
                    this.fieldRepository = new FieldRepository(db);
                return this.fieldRepository;
            }
        }

        public IRepository<Game> Games
        {
            get
            {
                if (this.gameRepository == null)
                    this.gameRepository = new GameRepository(db);
                return this.gameRepository;
            }
        }

        public IRepository<Move> Moves
        {
            get
            {
                if (this.moveRepository == null)
                    this.moveRepository = new MoveRepository(db);
                return this.moveRepository;
            }
        }

        public IRepository<PlayerGame> PlayerGames
        {
            get
            {
                if (this.playerGameRepository == null)
                    this.playerGameRepository = new PlayerGameRepository(db);
                return this.playerGameRepository;
            }
        }

        public IRepository<Player> Players 
        {
            get
            {
                if (this.playerRepository == null)
                    this.playerRepository = new PlayerRepository(db);
                return this.playerRepository;
            }
        }
        
        public IRepository<Ship> Ships
        {
            get
            {
                if (this.shipRepository == null)
                    this.shipRepository = new ShipRepository(db);
                return this.shipRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
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
