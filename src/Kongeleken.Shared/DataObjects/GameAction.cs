﻿using Kongeleken.Shared.Constants;
using System;

namespace Kongeleken.Shared.DataObjects
{
    public class GameAction
    {
        public UserAction Action { get; set; }
        public string Description { get; set; }
        public string PlayerId { get; set; }
        public DateTime When { get; set; }
        public GameAction(string playerId, string description, UserAction action)
        {
            this.PlayerId = playerId;
            this.Description = description;
            this.When = DateTime.UtcNow;
            this.Action = action;
        }
    }
}
