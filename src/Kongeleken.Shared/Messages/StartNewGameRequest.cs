using System;
using System.Collections.Generic;
using System.Text;

namespace Kongeleken.Shared.Messages
{
    public class StartNewGameRequest
    {
        public string PlayerId { get; set; }
        public string PlayerName { get; set; }
    }
}
