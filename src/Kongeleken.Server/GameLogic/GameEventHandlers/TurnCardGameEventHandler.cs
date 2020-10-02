using Kongeleken.Shared.Constants;
using Kongeleken.Shared.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kongeleken.Server.GameLogic.GameEventHandlers
{
    public class TurnCardGameEventHandler
    {
        public void Handle(GameEventDto gameEventDto, Game game, Player initiatingPlayer)
        {
            var player = game.Players.Single(p => p.Id == gameEventDto.PlayerId);

            if (player.CurrentCard == null)
            {
                game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} tried turing his card...but it's no longer there!", UserAction.None);
                return;
            }

            var card = player.CurrentCard;

            if (card.Id != gameEventDto.TargetId)
            {
                var turnCardOwner = game.Players.FirstOrDefault(p => p.CurrentCard.Id == gameEventDto.TargetId);
                if (turnCardOwner != null)
                {
                    game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} tried turing the card belonging to {turnCardOwner.Name}", UserAction.Cheat);
                }
                return;
            }

            card.IsTurned = true;

            game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} turned his card", UserAction.None);

            if (game.Players.All(p => p.CurrentCard.IsTurned))
            {
                var lowestCard = game.Players.Select(p => p.CurrentCard.Value).Min();
                var loosers = game.Players.Where(p => p.CurrentCard.Value == lowestCard).ToList();
                foreach (var loser in loosers)
                {
                    loser.AddFlag(PlayerFlag.Drink);
                    game.GameActions.Add(new GameActionDto(loser.Name, $"Lowest card is {lowestCard}. Loser this round is {loser.Name}.  DRINK!", UserAction.Drink));
                }

                //Handle king
                var playersWithKing = game.Players.Where(p => p.CurrentCard.Value == CardValue.King);
                foreach (var playerWithKing in playersWithKing)
                {
                    playerWithKing.AddFlag(PlayerFlag.King);
                    game.GameActions.Add(new GameActionDto(playerWithKing.Name, $"{playerWithKing.Name} got a king! ***DRINK!***", UserAction.DrinkKing));
                }

                //Handle queen
                var playersWithQueen = game.Players.Where(p => p.CurrentCard.Value == CardValue.Queen);
                foreach (var playerWithQueen in playersWithQueen)
                {
                    var otherPlayersWithPictureCard = game.Players.Where(p => p.CurrentCard.Value == CardValue.Queen
                    || p.CurrentCard.Value == CardValue.Jack
                    || p.CurrentCard.Value == CardValue.King).Where(p => p != playerWithQueen).ToList();

                    if (otherPlayersWithPictureCard.Count > 0)
                    {
                        otherPlayersWithPictureCard.ForEach(p => p.AddFlag(PlayerFlag.Drink));
                        var playerNames = string.Join(",", otherPlayersWithPictureCard.Select(l => l.Name));
                        game.AddGameAction(playerWithQueen.Name, $"{playerWithQueen.Name} got a queen! {playerNames} must DRINK!", UserAction.DrinkQueen);
                    }
                }

                //Handle jack
                var playersWithJack = game.Players.Where(p => p.CurrentCard.Value == CardValue.Jack);
                foreach (var playerWithJack in playersWithJack)
                {
                    var playersExceptCurrent = game.Players.Where(p => p != playerWithJack).ToList();
                    playersExceptCurrent.ForEach(p => p.AddFlag(PlayerFlag.Drink));

                    var playerNames = string.Join(",", playersExceptCurrent.Select(l => l.Name));
                    game.GameActions.Add(new GameActionDto(playerWithJack.Name, $"{playerWithJack.Name} got a jack! {playerNames} must DRINK!", UserAction.DrinkJack));
                }
                //TODO:
                //If a player receives a 6 of hearts he is to be given three new cards and all players must act according to these cards before a new round is started.
                //If a player receives a 6 of diamonds the player to the left of him is to be given three new cards and all players must act according  the these cards before a new round is started.
            }

        }

    }
}
