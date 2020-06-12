using System;
using System.Collections.Generic;
using System.Text;

namespace Kongeleken.Shared.Constants
{
    [Flags]
    public enum PlayerFlag
    {
        Drink=1,
        Cheat=2,
        Chicken=4,
        Pointing=8,
        LuckyBastard=16,
        Dealer=32,
        King=65536
    }
}
