using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using BattleShip.API.ViewModels;
using BattleShip.BusinessLogic.Interfaces;
using BattleShip.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BattleShip.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;
        private readonly IMapper mapper;

        public GameController(IGameService service, IMapper mapper)
        {
            this.gameService = service;
            this.mapper = mapper;
        }

        [HttpPost("create_game")]
        public IActionResult CreateGame(ShipViewModel[] shipsView)
        {
            if (shipsView == null)
            {
                return this.BadRequest("No ships in the field");
            }

            int playerid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (playerid == 0)
            {
                return this.Unauthorized();
            }

            int gameId = this.gameService.CreateGame(playerid);
            int fieldId = this.gameService.CreateField(playerid, gameId);
            List<Ship> ships = new List<Ship>();
            foreach (var s in shipsView)
            {
                List<Coordinate> coordinates = new List<Coordinate>();
                for (int y = s.Y; y < s.Y + s.Size; y++)
                {
                    Coordinate coord = new Coordinate
                    {
                        Y = y,
                        X = s.X,
                    };
                    coordinates.Add(coord);
                }

                Ship ship = new Ship { Size = s.Size, Coordinates = coordinates };
                ships.Add(ship);
            }

            this.gameService.AddShipsToField(fieldId, ships);
            return this.Ok();
        }

        [HttpPost("find_game")]
        public IActionResult FindGame([FromBody] ShipViewModel[] shipsView)
        {
            if (shipsView == null)
            {
                return this.BadRequest("No ships in the field");
            }

            int playerid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (playerid == 0)
            {
                return this.Unauthorized();
            }

            int fieldId = this.gameService.CreateField(playerid, null);
            List<Ship> ships = new List<Ship>();
            foreach (var s in shipsView)
            {
                List<Coordinate> coordinates = new List<Coordinate>();
                for (int y = s.Y; y < s.Y + s.Size; y++)
                {
                    Coordinate coord = new Coordinate
                    {
                        Y = y,
                        X = s.X,
                    };
                    coordinates.Add(coord);
                }

                Ship ship = new Ship { Size = s.Size, Coordinates = coordinates };
                ships.Add(ship);
            }

            this.gameService.AddShipsToField(fieldId, ships);
            return this.Ok();
        }

        [HttpGet("my_coords/{id}")]
        public IActionResult GetMyCoordinates(int id)
        {
            int playerid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var coord = this.gameService.GetCoordinatesForGame(playerid, id);
            var coordinates = this.mapper.Map<List<CoordinateView>>(coord);
            return this.Ok(coordinates);
        }

        [HttpGet("enemy_coords/{id}")]
        public IActionResult GetEnemyCoordinates(int id)
        {
            int playerid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var coord = this.gameService.GetCoordinatesForGame(playerid, id, false);
            var coordinates = this.mapper.Map<List<CoordinateView>>(coord);
            return this.Ok(coordinates);
        }

        [HttpGet("get_current_player/{id}")]
        public IActionResult GetCurrentPlayerId(int id)
        {
            var game = this.gameService.GetGame(id);
            int currentMove = game.CurrentMovePlayerId;
            return this.Ok(currentMove);
        }

        [HttpGet("get_records/{id}")]
        public IActionResult Game(int id)
        {
            var records = this.mapper.Map<List<RecordsView>>(this.gameService.GetAllRecords(id));
            return this.Ok(records);
        }
    }
}