using System;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Soulstone : Unit
    {
        public Soulstone(int ID)
            : base(ID)
        {
            health = 99999999;
            maxHealth = 99999999;
            armor = 35;
            damage = 8;
            movementRange = 0;
            movementCost = 0;
            attackRange = 0;
            attackCost = 0;
        }
    }
}
