namespace BattleShip.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using AutoMapper;
    using BattleShip.API.ViewModels;
    using BattleShip.BusinessLogic.Interfaces;
    using BattleShip.Models.Entities;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PersonalAccountController : ControllerBase
    {
        private readonly IGameService gameService;
        private readonly IPlayerService playerService;

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
            return this.Ok(gamesView);
        }
    }
}
