using Kongeleken.Shared.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kongeleken.Server.GameLogic.GameEventHandlers
{
    public class SayGameEventHandler
    {
        public SayGameEventHandler()
        {
        }

        public void Handle(GameEventDto gameEventDto, Game game, Player initiatingPlayer)
        {
            game.AddGameAction(initiatingPlayer.Name, $"{initiatingPlayer.Name} says: "+gameEventDto.Value,Shared.Constants.UserAction.None);
        }
    }
}
