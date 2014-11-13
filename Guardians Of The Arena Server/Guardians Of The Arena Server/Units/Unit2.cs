using System;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    class Unit2 : Unit
    {
        public Unit2(int ID)
            : base(ID)
        {
            health = 30;
            maxHealth = 30;
            damage = 0;
            movementRange = 2;
            movementCost = 2;
            attackCost = 4;
            attackRange = 4;
        }
    }
}
