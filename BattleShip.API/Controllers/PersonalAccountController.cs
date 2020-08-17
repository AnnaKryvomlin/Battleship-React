using System;
using System.Collections.Generic;
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
    public class PersonalAccountController : ControllerBase
    {
        IGameService gameService;
        IPlayerService playerService;

        public PersonalAccountController(IGameService gameService, IPlayerService playerService)
        {
            this.gameService = gameService;
            this.playerService = playerService;
        }

        [HttpGet]
        public IActionResult GetGames()
        {
            int playerid = Convert.ToInt32(this.User.FindFirstValue(ClaimTypes.NameIdentifier));
            var games = this.gameService.GetPlayerGames(playerid);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Game, GameForAccountViewModel>()).CreateMapper();
            var gamesView = mapper.Map<IEnumerable<Game>, List<GameForAccountViewModel>>(games);
            return Ok(gamesView);
        }
    }
}
