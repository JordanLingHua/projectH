using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Tree : Unit
    {
        public Tree(int ID)
            : base(ID)
        {
            health = 1;
            maxHealth = 1;
            armor = 35;
            damage = 0;
            movementRange = 0;
            movementCost = 0;
            attackCost = 0;
            attackRange = 0;

            this.unitAllegiance = Allegiance.NEUTRAL;
        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            throw new NotImplementedException();
        }
        public override void LevelUp()
        {
            throw new NotImplementedException();
        }
    }
}
