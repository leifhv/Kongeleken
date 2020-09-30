using Kongeleken.Shared.Constants;
using Kongeleken.Shared.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kongeleken.Server.GameLogic.GameEventHandlers
{
    public class DealGameEventHandler
    {
        public void Handle(GameEventDto gameEventDto, Game game, Player initiatingPlayer)
        {
            if (game.Players.Any(p => p.CurrentCard != null && !p.CurrentCard.IsTurned))
            {
                game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} tried dealing, but the round is not finished yet", UserAction.None);
                return;
            }

            if (initiatingPlayer.Id != game.DealerPlayerId)
            {
                game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} tried dealing, but he's not the current dealer!!!", UserAction.None);
                return;
            }

            if (game.CardDeck.Count < game.Players.Count)
            {
                game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} tried dealing, but he's running out of cards in the deck", UserAction.None);
                return;
            }

            foreach (var player in game.Players)
            {
                player.ClearFlags();
                player.CurrentCard = game.CardDeck.First();
                player.CurrentCard.IsTurned = false;
                game.CardDeck.RemoveAt(0);
            }
            game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} dealt cards", UserAction.None);
        }

    }
}
