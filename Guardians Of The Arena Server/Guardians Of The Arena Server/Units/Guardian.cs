using System;
using System.Collections;
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
            armor = 60;
            damage = 23;
            movementRange = 2;
            movementCost = 2;
            attackCost = 1;
            attackRange = 1;
        }

       public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            ArrayList unitsHit = new ArrayList();

            if (tile.CurrentUnit != null)
                unitsHit.Add(tile.CurrentUnit.UniqueID);

            return unitsHit;
        }
       public override void LevelUp()
       {
           throw new NotImplementedException();
       }
    }
}
