using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
        private IGameService gameService;

        public GameController(IGameService service)
        {
            this.gameService = service;
        }

        [HttpPost("create_game")]
        public IActionResult CreateGame(ShipViewModel[] shipsView)
        {
            if (shipsView == null)
                return BadRequest("No ships in the field");
            int playerid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (playerid == 0)
                return Unauthorized();
            int gameId =  this.gameService.CreateGame(playerid);
            int fieldId = this.gameService.CreateField(playerid, gameId);
            List<Ship> ships = new List<Ship>();
            foreach(var s in shipsView)
            {
                List<Coordinate> coordinates = new List<Coordinate>();
                for(int x = s.X; x < s.X + s.Size; x++)
                {
                    Coordinate coord = new Coordinate
                    {
                        Y = s.Y,
                        X = x
                    };
                    coordinates.Add(coord);
                }

                Ship ship = new Ship { Size = s.Size, Coordinates = coordinates };
                ships.Add(ship);
            }
            this.gameService.AddShipsToField(fieldId, ships);
            return Ok();
        }

        [HttpPost("find_game")]
        public IActionResult FindGame([FromBody] ShipViewModel[] shipsView)
        {
            if (shipsView == null)
                return BadRequest("No ships in the field");
            int playerid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (playerid == 0)
                return Unauthorized();
            int fieldId = this.gameService.CreateField(playerid, null);
            List<Ship> ships = new List<Ship>();
            foreach (var s in shipsView)
            {
                List<Coordinate> coordinates = new List<Coordinate>();
                for (int x = s.X; x < s.X + s.Size; x++)
                {
                    Coordinate coord = new Coordinate
                    {
                        Y = s.Y,
                        X = x
                    };
                    coordinates.Add(coord);
                }

                Ship ship = new Ship { Size = s.Size, Coordinates = coordinates };
                ships.Add(ship);
            }
            this.gameService.AddShipsToField(fieldId, ships);
            return Ok();
        }

        [HttpGet("my_coords/{id}")]
        public IActionResult GetMyCoordinates(int id)
        {
            int playerid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Coordinate, CoordinateView>()
            .ForMember("HaveShip", opt => opt.MapFrom(c => c.Ship != null))).CreateMapper();
            var coord = this.gameService.GetCoordinatesForGame(playerid, id);
            var coordinates = mapper.Map<IEnumerable<Coordinate>, List<CoordinateView>>(coord);
            return Ok(coordinates);
        }

        [HttpGet("enemy_coords/{id}")]
        public IActionResult GetEnemyCoordinates(int id)
        {
            int playerid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Coordinate, CoordinateView>()
            .ForMember("HaveShip", opt => opt.MapFrom(c => c.Ship != null))).CreateMapper();
            var coord = this.gameService.GetCoordinatesForGame(playerid, id, false);
            var coordinates = mapper.Map<IEnumerable<Coordinate>, List<CoordinateView>>(coord);
            return Ok(coordinates);
        }

        [HttpGet("get_current_player/{id}")]
        public IActionResult GetCurrentPlayerId(int id)
        {
            var game = this.gameService.GetGame(id);
            int currentMove = game.CurrentMovePlayerId;
            return Ok(currentMove);
        }

        [HttpGet("get_records/{id}")]
        public IActionResult Game(int id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Move, RecordsView>()).CreateMapper();
            var records = mapper.Map<IEnumerable<Move>, List<RecordsView>>(this.gameService.GetAllRecords(id));
            return Ok(records);
        }

    }
}
