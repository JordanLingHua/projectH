using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Unit8 : Unit
    {
        public Unit8(int ID)
            : base(ID)
        {
            health = 20;
            maxHealth = 20;
            armor = 35;
            damage = -18;
            movementRange = 3;
            movementCost = 3;
            attackCost = 6;
            attackRange = 4; //target based
        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            ArrayList unitsHit = new ArrayList();
            unitsHit.Add(tile.CurrentUnit.UniqueID);
            return unitsHit;
        }
    }
}
