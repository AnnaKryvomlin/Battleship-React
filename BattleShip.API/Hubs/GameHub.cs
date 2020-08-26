namespace BattleShip.WEB.Hubs
{
    using System.Threading.Tasks;
    using BattleShip.BusinessLogic.Interfaces;
    using Microsoft.AspNetCore.SignalR;

    public class GameHub : Hub
    {
        private IGameService gameService;

        public GameHub(IGameService gameService)
        {
            this.gameService = gameService;
        }

        public Task JoinGame(int gameId)
        {
            return this.Groups.AddToGroupAsync(this.Context.ConnectionId, gameId.ToString());
        }

        public void TakeAShot(int playerId, int gameId, int x, int y)
        {
            var coordinate = this.gameService.GetCoordinate(playerId, gameId, x, y);
            int currentPlayerId = this.gameService.MarkCell(coordinate, gameId, playerId);
            if (this.gameService.IsGameCanContinues(gameId, playerId))
            {
                string record = this.gameService.MoveRecord(playerId, gameId, x, y);
                this.Clients.Caller.SendAsync("TakeAShot", coordinate.ShipId != null, x, y, currentPlayerId);
                this.Clients.OthersInGroup(gameId.ToString()).SendAsync("ShowAShot", x, y, currentPlayerId);
                this.Clients.Group(gameId.ToString()).SendAsync("NewRecord", record);
            }
            else
            {
                this.Clients.Caller.SendAsync("TakeAShot", coordinate.ShipId != null, x, y, currentPlayerId);
                this.Clients.OthersInGroup(gameId.ToString()).SendAsync("ShowAShot", x, y, currentPlayerId);
                string message = "Вы выиграли!";
                this.Clients.Caller.SendAsync("Finished", message);
                string mess = "Победа не главное!";
                this.Clients.OthersInGroup(gameId.ToString()).SendAsync("Finished", mess);
            }
        }

        public void TrowUpTheTowel(int playerId, int gameId)
        {
            this.gameService.TrowUpTheTowel(gameId, playerId);
            string message = "Иногда нужно вовремя сдаться...";
            this.Clients.Caller.SendAsync("StopGame", message);
            string mess = "Вы выиграли!";
            this.Clients.OthersInGroup(gameId.ToString()).SendAsync("StopGame", mess);
        }
    }
}
