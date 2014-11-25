using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server
{
    public abstract class Unit : BoardObject
    {
        public ArrayList accessibleTiles;
        public enum Allegiance { PLAYER_1, PLAYER_2, NEUTRAL }
        private Allegiance allegiance;
        protected int movementRange;
        protected int movementCost;
        protected int attackRange;
        protected int attackCost;
        protected int health;
        protected int maxHealth;
        protected int uniqueID;
        protected int damage;
        protected int armor;
        protected bool alreadyMoved = false;
        protected bool alreadyAttacked = false;

        #region Properties Region
        public Allegiance unitAllegiance
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

        public int AttackRange
        {
            get { return attackRange; }
            set { attackRange = value;}
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

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public int UniqueID
        {
            get { return uniqueID; }
        }

        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }

        public int Armor
        {
            get { return armor; }
            set { armor = value; }
        }

        public bool Moved
        {
            get { return alreadyMoved; }
            set { alreadyMoved = value; }
        }

        public bool Attacked
        {
            get { return alreadyAttacked; }
            set { alreadyAttacked = value; }
        }

        #endregion

        public Unit(int ID)
        {
            uniqueID = ID;
            accessibleTiles = new ArrayList();
        }

        //Recursively Add all the tiles that this unit is able to atravel to
        public void setAccessibleTiles(GameBoard.Tile currentTile, int distance)
        {
            if (distance > 0)
            {
                if ((currentTile.UP != null && (currentTile.UP.CurrentUnit == null 
                                            || (currentTile.UP.CurrentUnit.unitAllegiance == unitAllegiance))))
                {
                    setAccessibleTiles(currentTile.UP, distance - 1);
                    accessibleTiles.Add(currentTile.UP);
                }
                if ((currentTile.DOWN != null && (currentTile.DOWN.CurrentUnit == null
                                                || (currentTile.DOWN.CurrentUnit.unitAllegiance == unitAllegiance))))
                {
                    setAccessibleTiles(currentTile.DOWN, distance - 1);
                    accessibleTiles.Add(currentTile.DOWN);
                }
                if ((currentTile.RIGHT != null && (currentTile.RIGHT.CurrentUnit == null
                                                || (currentTile.RIGHT.CurrentUnit.unitAllegiance == unitAllegiance))))
                {
                    setAccessibleTiles(currentTile.RIGHT, distance - 1);
                    accessibleTiles.Add(currentTile.RIGHT);
                }
                if ((currentTile.LEFT != null && (currentTile.LEFT.CurrentUnit == null
                                                || currentTile.LEFT.CurrentUnit.unitAllegiance == unitAllegiance)))
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

        public virtual void ApplyDamage(int damage)
        {
            this.health -= damage;
            Console.WriteLine("LOG: Unit " + uniqueID + " takes " + damage + " damage.");


            if (health <= 0)
            {
                currentTile.CurrentUnit = null;
                currentTile.Occupation = GameBoard.Tile.Occupied.EMPTY;
                currentTile = null;

                Console.WriteLine("LOG: Unit " + uniqueID + " has died.");
            }
        }

        public abstract ArrayList AttackTile(GameBoard.Tile tile);
    }
}
