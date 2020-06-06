using Kongeleken.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kongeleken.Shared.DataObjects
{
    public class FaceDownCardDto
    {
        public string Id { get; set; }
    }

    public class CardDto : FaceDownCardDto
    {
        public CardSuit Suit { get; set; }
        public CardValue Value { get; set; }
    }

        
}
