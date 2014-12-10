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
            health = 25;
            maxHealth = 25;
            armor = 8;
            damage = 18;
            movementRange = 4;
            movementCost = 2;
            attackCost = 2;
            attackRange = 4;
             
        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            int xDif = this.currentTile.x - tile.x;
            int yDif = this.currentTile.y - tile.y;

            GameBoard.Tile temp = this.currentTile;

            ArrayList unitsHit = new ArrayList();
           
            if (xDif < 0)
            {
                for (int i = -1; i >= xDif; i--)
                {
                    Console.WriteLine("i:{0} , xDif:{1}", i, xDif);
                    temp = temp.UP;

                    if (temp.CurrentUnit != null)
                    {
                        Console.WriteLine("Unit Hit: {0}", temp.CurrentUnit.UniqueID);
                        unitsHit.Add(temp.CurrentUnit.UniqueID);
                        return unitsHit;
                    }
                }
            }
            else if (xDif > 0)
            {
                for (int i = 1; i <= xDif; i++)
                {
                    Console.WriteLine("i:{0} , yDif:{1}", i, xDif);
                    temp = temp.DOWN;

                    if (temp.CurrentUnit != null)
                    {
                        Console.WriteLine("Unit Hit: {0}", temp.CurrentUnit.UniqueID);
                        unitsHit.Add(temp.CurrentUnit.UniqueID);
                        return unitsHit;
                    }
                }
            }

             if (yDif > 0)
            {
                for (int i = 1; i <= yDif; i++)
                {
                    Console.WriteLine("i:{0} , yDif:{1}", i, yDif);
                    temp = temp.RIGHT;

                    if (temp.CurrentUnit != null)
                    {
                        Console.WriteLine("Unit Hit: {0}", temp.CurrentUnit.UniqueID);
                        unitsHit.Add(temp.CurrentUnit.UniqueID);
                        return unitsHit;
                    }
                }
            }
            else if (yDif < 0)
            {
                for (int i = -1; i >= yDif; i--)
                {
                    Console.WriteLine("i:{0} , yDif:{1}", i, yDif);
                    temp = temp.LEFT;

                    if (temp.CurrentUnit != null)
                    {
                        Console.WriteLine("Unit Hit: {0}", temp.CurrentUnit.UniqueID);
                        unitsHit.Add(temp.CurrentUnit.UniqueID);
                        return unitsHit;
                    }
                }
            }

            return unitsHit;
        }

        public override void LevelUp()
        {
            throw new NotImplementedException();
        }
    }
}
