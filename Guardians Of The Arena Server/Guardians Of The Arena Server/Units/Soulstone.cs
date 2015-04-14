using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Soulstone : Unit
    {
        private bool invulnerable;

        public bool Invulnerable
        {
            get { return invulnerable; }
            set { invulnerable = value; }
        }

        public Soulstone(int ID)
            : base(ID)
        {
            health = 1;
            maxHealth = 1;
            armor = 0;
            damage = 8;
            movementRange = 0;
            movementCost = 0;
            attackRange = 0;
            attackCost = 0;

            invulnerable = true;
            isSoulStone = true;
        }

        public override void setAccessibleTiles(GameBoard.Tile currentTile, int distance)
        {
            //do nothing
        }


        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            return null;
        }

        public override void ApplyDamage(int damage, Unit attackingUnit)
        {
            if (invulnerable)
                damage = 0;

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

        public override void LevelUp()
        {
            XP = currentXP % 20;

            switch (Level)
            { }
        }
    }
}
