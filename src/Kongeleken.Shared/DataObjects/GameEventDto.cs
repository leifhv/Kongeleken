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

        //The id of the target for the event (playerid, cardid, gameid...)
        public string TargetId
        {
            get;set;
        }

        public string Value
        {
            get; set;
        }

    }
}
