using Kongeleken.Shared.Constants;
using Kongeleken.Shared.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kongeleken.Server.GameLogic
{    
    public class Game
    {
        public string Id { get; set; }
        
        public GameState State { get; set; }

        public List<Player> Players { get; set; } = new List<Player>();

        public CardDeck CardDeck { get; set; } = new CardDeck();
        
        public string DealerPlayerId { get; set; }

        private List<string> GameActions = new List<string>();

        public void AddGameAction(string gameActionDescription)
        {
            GameActions.Add(gameActionDescription);
            if(GameActions.Count> 10)
            {
                GameActions.RemoveAt(0);
            }
        }

        public List<string> GetGameActions()
        {
            return GameActions.ToList();
        }
    }
}
