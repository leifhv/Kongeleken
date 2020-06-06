using Kongeleken.Shared.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kongeleken.Shared.Messages
{
    public class StartNewGameResponse
    {
        public string NewPlayerId { get; set; }
        public GameDto Game { get; set; }
    }
}
