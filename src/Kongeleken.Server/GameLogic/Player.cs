using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kongeleken.Server.GameLogic
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Card CurrentCard { get; set; }
        public List<Card> PreviousCards { get; set; } = new List<Card>();
    }
}
