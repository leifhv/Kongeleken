using Kongeleken.Shared.Constants;
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
        public List<PlayerFlag> PlayerFlags {get;set;} = new List<PlayerFlag>();

        public DateTime LastContact = DateTime.Now;


        public void AddFlag(PlayerFlag flag)
        {
            PlayerFlags.Add(flag);
        }
        public void ClearFlags()
        {
            PlayerFlags.Clear();
        }
    }
}
