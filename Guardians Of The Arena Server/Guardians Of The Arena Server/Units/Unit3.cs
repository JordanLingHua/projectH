using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Unit3 : Unit
    {
        public Unit3(int unitID)
            : base(unitID)
        {
            health = 38;
            maxHealth = 38;
            armor = 8;
            damage = 13;
            movementRange = 3;
            movementCost = 1;
            attackCost = 3;
            attackRange = 1;

        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            int xDif = this.currentTile.x - tile.x;
            int yDif = this.currentTile.y - tile.y;

            ArrayList unitsHit = new ArrayList();

            if (tile.CurrentUnit != null)
                unitsHit.Add(tile.CurrentUnit.UniqueID);

            if (this.currentTile.UP == tile)
            {
                if (tile.UP != null)
                {
                    GameBoard.Tile upTile = tile.UP;

                    if (upTile.CurrentUnit != null)
                        unitsHit.Add(upTile.CurrentUnit.UniqueID);



                    tile = upTile.RIGHT;

                    if (tile != null && tile.CurrentUnit != null)
                        unitsHit.Add(tile.CurrentUnit.UniqueID);

                    tile = upTile.LEFT;

                    if (tile != null && tile.CurrentUnit != null)
                        unitsHit.Add(tile.CurrentUnit.UniqueID);
                }
            }
            else if (this.currentTile.DOWN == tile)
            {
                if (tile.DOWN != null)
                {
                    GameBoard.Tile downTile = tile.DOWN;

                    if (downTile.CurrentUnit != null)
                        unitsHit.Add(downTile.CurrentUnit.UniqueID);



                    tile = downTile.RIGHT;

                    if (tile != null && tile.CurrentUnit != null)
                        unitsHit.Add(tile.CurrentUnit.UniqueID);

                    tile = downTile.LEFT;

                    if (tile != null && tile.CurrentUnit != null)
                        unitsHit.Add(tile.CurrentUnit.UniqueID);
                }
            }
            else if (this.currentTile.RIGHT == tile)
            {
                if (tile.RIGHT != null)
                {
                    GameBoard.Tile rightTile = tile.RIGHT;

                    if (rightTile.CurrentUnit != null)
                        unitsHit.Add(rightTile.CurrentUnit.UniqueID);

                    tile = rightTile.UP;

                    if (tile != null && tile.CurrentUnit != null)
                        unitsHit.Add(tile.CurrentUnit.UniqueID);

                    tile = rightTile.DOWN;

                    if (tile != null && tile.CurrentUnit != null)
                        unitsHit.Add(tile.CurrentUnit.UniqueID);
                }
            }
            else if (this.currentTile.LEFT == tile)
            {
                if (tile.LEFT != null)
                {
                    GameBoard.Tile leftTile = tile.LEFT;

                    if (leftTile.CurrentUnit != null)
                        unitsHit.Add(leftTile.CurrentUnit.UniqueID);

                    tile = leftTile.UP;

                    if (tile != null && tile.CurrentUnit != null)
                        unitsHit.Add(tile.CurrentUnit.UniqueID);

                    tile = leftTile.DOWN;

                    if (tile != null && tile.CurrentUnit != null)
                        unitsHit.Add(tile.CurrentUnit.UniqueID);


                }
            }

            return unitsHit;
        }

        public override void LevelUp()
        {
            XP = currentXP % 20;

            switch (Level)
            { }
        }
    }
}
