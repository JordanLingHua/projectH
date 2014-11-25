using System;
using System.Collections;
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
            armor = 35;
            damage = 10;
            movementRange = 2;
            movementCost = 2;
            attackCost = 4;
            attackRange = 4;
        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            ArrayList unitsHit = new ArrayList();
            unitsHit.Add(tile.CurrentUnit.UniqueID);
            return unitsHit;
        }
    }
}
