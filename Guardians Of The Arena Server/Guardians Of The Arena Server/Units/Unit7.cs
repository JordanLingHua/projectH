using System;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    class Unit7 : Unit
    {
        public Unit7(int ID)
            : base(ID)
        {
            health = 38;
            maxHealth = 38;
            damage = 10;
            movementRange = 3;
            movementCost = 1;
            attackCost = 1;
            attackRange = 1;
        }
    }
}
