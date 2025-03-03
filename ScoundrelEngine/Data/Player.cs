using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoundrelCore.Data
{
    public class Player
    {
        public int Health { get; set; } = 20;
        public Card? Weapon { get; set; }
        public Card? PreviousMonsterForWeapon { get; set; }
    }
}
