using BattleShip.BusinessLogic.Interfaces;
using BattleShip.Models.Entities;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShip.WEB.Hubs
{
    public class GameHub : Hub
    {
        private IGameService gameService;

        public GameHub(IGameService gameService)
        {
            this.gameService = gameService;
        }

        public Task JoinGame(int gameId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }

        public void TakeAShot(int playerId, int gameId, int x, int y)
        {
            var coordinate = this.gameService.GetCoordinate(playerId, gameId, x, y);
            int currentPlayerId = this.gameService.MarkCell(coordinate, gameId, playerId);
            if (this.gameService.IsGameCanContinues(gameId, playerId))
            {
                string record = this.gameService.MoveRecord(playerId, gameId, x, y);
                Clients.Caller.SendAsync("TakeAShot", coordinate.ShipId != null, x, y, currentPlayerId);
                Clients.OthersInGroup(gameId.ToString()).SendAsync("ShowAShot", x, y, currentPlayerId);
                Clients.Group(gameId.ToString()).SendAsync("NewRecord", record);
            }

            else
            {
                Clients.Caller.SendAsync("TakeAShot", coordinate.ShipId != null, x, y, currentPlayerId);
                Clients.OthersInGroup(gameId.ToString()).SendAsync("ShowAShot", x, y, currentPlayerId);
                string message = "Вы выиграли!";
                Clients.Caller.SendAsync("Finished", message);
                string mess = "Победа не главное!";
                Clients.OthersInGroup(gameId.ToString()).SendAsync("Finished", mess);
            }
        }
        
        public void TrowUpTheTowel(int playerId, int gameId)
        {
            this.gameService.TrowUpTheTowel(gameId, playerId);
            string message = "Иногда нужно вовремя сдаться...";
            Clients.Caller.SendAsync("StopGame", message);
            string mess = "Вы выиграли!";
            Clients.OthersInGroup(gameId.ToString()).SendAsync("StopGame", mess);
        }
    }
}
