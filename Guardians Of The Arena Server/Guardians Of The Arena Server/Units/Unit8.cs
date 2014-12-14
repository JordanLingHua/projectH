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
            damage = -20;
            movementRange = 3;
            movementCost = 1;
            attackCost = 4;
            attackRange = 4; //target based
        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            ArrayList unitsHit = new ArrayList();
            if (tile.CurrentUnit != null)
            {
                unitsHit.Add(tile.CurrentUnit.UniqueID);

                if (level == 2)
                {
                    
                }

            }
            return unitsHit;
        }

        public override void setAttackTiles(GameBoard.Tile currentTile, int distance)
        {
            if (distance > 0)
            {
                if (currentTile.UP != null) 
                {
                    setAccessibleTiles(currentTile.UP, distance - 1);
                    accessibleTiles.Add(currentTile.UP);
                }
                if (currentTile.DOWN != null )
                {
                    setAccessibleTiles(currentTile.DOWN, distance - 1);
                    accessibleTiles.Add(currentTile.DOWN);
                }
                if (currentTile.RIGHT != null )
                {
                    setAccessibleTiles(currentTile.RIGHT, distance - 1);
                    accessibleTiles.Add(currentTile.RIGHT);
                }
                if (currentTile.LEFT != null )
                {
                    setAccessibleTiles(currentTile.LEFT, distance - 1);
                    accessibleTiles.Add(currentTile.LEFT);
                }
            }
        }

        public override void LevelUp()
        {
            XP = currentXP % 20;

            switch (Level)
            {
                case 1:
                    damage = -50;
                    break;
                default:
                    break;
            }
        }
    }
}
