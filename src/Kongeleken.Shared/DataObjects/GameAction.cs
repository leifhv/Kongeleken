using System;

namespace Kongeleken.Shared.DataObjects
{
    public class GameAction
    {
        public string Action { get; }
        public DateTime When { get; }
        public GameAction(string action)
        {
            this.Action = action;
            this.When = DateTime.UtcNow;
        }
    }
}
