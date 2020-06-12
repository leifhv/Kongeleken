using System;
using System.Collections.Generic;
using System.Text;

namespace Kongeleken.Shared.DataObjects
{
    public class GameDto
    {
        public string Id { get; set; }
        public List<PlayerDto> Players { get; set; } = new List<PlayerDto>();

        public List<CardDto> CardDeck { get; set; } = new List<CardDto>();

        public string DealerName { get; set; }
        public bool PlayerIsDealer { get; set; }
        public List<GameActionDto> GameActions{ get; set; } = new List<GameActionDto>();
    }
}
