using Kongeleken.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kongeleken.Shared.DataObjects
{
    public class GameEventDto
    {
        public string GameId { get; set; }
        public GameEventType EventType
        {
            get;set;
        }

        public string PlayerId
        {
            get;set;
        }
    }
}
