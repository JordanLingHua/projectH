using System;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    class Unit3 : Unit
    {
        public Unit3(int ID)
            : base(ID)
        {
            health = 38;
            maxHealth = 38;
            damage = 13;
            movementRange = 3;
            movementCost = 2;
            attackCost = 3;
            attackRange = 1;
        }
    }
}
