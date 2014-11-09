﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Guardian : Unit
    {
        public Guardian(int ID)
            : base(ID)
        {
            health = 40;
            maxHealth = 40;
            armor = 60;
            damage = 8;
            movementRange = 1;
            movementCost = 3;
            attackCost = 1;
            attackRange = 1;
        }
    }
}
