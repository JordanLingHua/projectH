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
            health = 20;
            maxHealth = 20;
            damage = 18;
            movementRange = 4;
            movementCost = 2;
            attackCost = 2;
            attackRange = 3;
             
        }
    }
}
