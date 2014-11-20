using System;
using System.Collections;
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
            armor = 8;
            damage = 20;
            movementRange = 4;
            movementCost = 2;
            attackCost = 2;
            attackRange = 3;
             
        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            int xDif = this.currentTile.x - tile.x;
            int yDif = this.currentTile.y - tile.y;

            ArrayList unitsHit = new ArrayList();
           
            if (xDif > 0)
            {
                for (int i = -1; i > xDif; i--)
                {
                    currentTile = currentTile.DOWN;

                    if (currentTile.CurrentUnit != null)
                    {
                        unitsHit.Add(currentTile.CurrentUnit.UniqueID);
                        return unitsHit;
                    }
                }
            }
            else if (xDif > 0)
            {
                for (int i = 1; i < xDif; i++)
                {
                    currentTile = currentTile.DOWN;

                    if (currentTile.CurrentUnit != null)
                    {
                        unitsHit.Add(currentTile.CurrentUnit.UniqueID);
                        return unitsHit;
                    }
                }
            }

            else if (yDif < 0)
            {
                for (int i = 1; i < yDif; i++)
                {
                    currentTile = currentTile.RIGHT;

                    if (currentTile.CurrentUnit != null)
                    {
                        unitsHit.Add(currentTile.CurrentUnit.UniqueID);
                        return unitsHit;
                    }
                }
            }
            else if (yDif > 0)
            {
                for (int i = -1; i > yDif; i--)
                {
                    currentTile = currentTile.LEFT;

                    if (currentTile.CurrentUnit != null)
                    {
                        unitsHit.Add(currentTile.CurrentUnit.UniqueID);
                        return unitsHit;
                    }
                }
            }

            return unitsHit;
        }
    }
}
