using System;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Unit1 : Unit
    {
        public Unit1(int ID)
            : base(ID)
        {
            health = 18;
            maxHealth = 18;
            armor = 8;
            damage = 20;
            movementRange = 4;
            movementCost = 1;
            attackCost = 2;
            attackRange = 3;
             
        }
    }
}
