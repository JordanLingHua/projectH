using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server
{
    public class Unit : BoardObject
    {
        public ArrayList accessibleTiles;
        public enum Allegiance { ALLY, ENEMY, NEUTRAL }
        private Allegiance allegiance;
        private int movementRange;
        private int movementCost;
        private int attackCost;
        private int health;

        #region Properties Region
        public Allegiance Allegiances
        {
            get { return allegiance; }
            set { allegiance = value; }
        }

        public int MovementRange
        {
            get { return movementRange; }
            set { movementRange = value; }
        }

        public int MovementCost
        {
            get { return movementCost; }
            set { movementCost = value; }
        }

        public int AttackCost
        {
            get { return attackCost; }
            set { movementCost = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        #endregion

        public Unit(int movementRange, int movementCost, int attackCost)
        {
            this.movementRange = movementRange;
            this.movementCost = movementCost;
            this.attackCost = attackCost;
            accessibleTiles = new ArrayList();
        }

        //Recursively Add all the tiles that this unit is able to atravel to
        public void setAccessibleTiles(GameBoard.Tile currentTile, int distance)
        {
            if (distance > 0)
            {
                if ((currentTile.UP != null && currentTile.UP.CurrentUnit == null))
                {
                    setAccessibleTiles(currentTile.UP, distance - 1);
                    accessibleTiles.Add(currentTile.UP);
                }
                if ((currentTile.DOWN != null && currentTile.DOWN.CurrentUnit == null))
                {
                    setAccessibleTiles(currentTile.DOWN, distance - 1);
                    accessibleTiles.Add(currentTile.DOWN);
                }
                if ((currentTile.RIGHT != null && currentTile.RIGHT.CurrentUnit == null))
                {
                    setAccessibleTiles(currentTile.RIGHT, distance - 1);
                    accessibleTiles.Add(currentTile.RIGHT);
                }
                if ((currentTile.LEFT != null && currentTile.LEFT.CurrentUnit == null))
                {
                    setAccessibleTiles(currentTile.LEFT, distance - 1);
                    accessibleTiles.Add(currentTile.LEFT);
                }
            }
        }

        //recursively add all the tiles that unit is able to attack
        public void setAttackTiles(GameBoard.Tile currentTile, int distance)
        {
            if (distance > 0)
            {
                GameBoard.Tile temp = currentTile;
                for (int i = 0; i < distance; i++)
                {
                    if (temp.UP != null)
                    {
                        accessibleTiles.Add(temp.UP);
                        temp = temp.UP;
                    }
                }

                temp = currentTile;
                for (int i = 0; i < distance; i++)
                {
                    if (temp.DOWN != null)
                    {
                        accessibleTiles.Add(temp.DOWN);
                        temp = temp.DOWN;
                    }
                }

                temp = currentTile;
                for (int i = 0; i < distance; i++)
                {
                    if (temp.LEFT != null)
                    {
                        accessibleTiles.Add(temp.LEFT);
                        temp = temp.LEFT;
                    }
                }

                temp = currentTile;
                for (int i = 0; i < distance; i++)
                {
                    if (temp.RIGHT != null)
                    {
                        accessibleTiles.Add(temp.RIGHT);
                        temp = temp.RIGHT;
                    }
                }
            }
        }

        public void ApplyDamage(int damage)
        {
            health -= damage;

            if (health <= 0)
            {
                currentTile.CurrentUnit = null;
                currentTile.Occupation = GameBoard.Tile.Occupied.EMPTY;
                currentTile = null;
            }
        }
    }
}
