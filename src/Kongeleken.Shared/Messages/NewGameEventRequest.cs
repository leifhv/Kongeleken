using Kongeleken.Shared.DataObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kongeleken.Shared.Messages
{
    public class NewGameEventRequest
    {
        public GameEventDto GameEvent { get; set; }
    }
}
