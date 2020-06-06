using Kongeleken.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kongeleken.Server.GameLogic
{
    public class Card
    {
        public string Id { get; set; }
        public CardSuit Suit { get; set; }
        public CardValue Value { get; set; }
        public bool IsTurned { get; set; }
    }
}
