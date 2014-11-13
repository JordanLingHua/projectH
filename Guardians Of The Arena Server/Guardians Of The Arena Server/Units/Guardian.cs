using System;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Guardian : Unit
    {
        public Guardian(int ID)
            : base(ID)
        {
            health = 45;
            maxHealth = 45;
            damage = 23;
            movementRange = 2;
            movementCost = 3;
            attackCost = 1;
            attackRange = 1;
        }
    }
}
