using System;
using System.Collections.Generic;
using System.Text;

namespace Kongeleken.Shared.DataObjects
{
    public class PlayerDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<CardDto> FaceUpCards { get; set; }
        public List<FaceDownCardDto> FaceDownCards { get; set; }
    }
}
