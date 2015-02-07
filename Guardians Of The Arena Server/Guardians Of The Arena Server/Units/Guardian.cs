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
            movementCost = 1;
            attackCost = 2;
            attackRange = 1;
        }

        public override void Attack(Unit unitAttacking)
        {
            if (this.level >= 3)
            {
                if (unitAttacking.Health < unitAttacking.MaxHealth / 2)
                    unitAttacking.ApplyDamage(1000);
                else
                    unitAttacking.ApplyDamage(damage);
            }
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
           XP = currentXP % 20;

           //switch (Level)
           //{ }
       }

       public override void ApplyDamage(int damage)
       {
           if (this.level >= 2 && damage > 10)
               damage = 10;

           this.health -= damage;
           Console.WriteLine("LOG: Unit " + uniqueID + " takes " + damage + " damage.");

           if (health > maxHealth)
               health = maxHealth;


           if (health <= 0)
           {
               currentTile.CurrentUnit = null;
               currentTile.Occupation = GameBoard.Tile.Occupied.EMPTY;
               currentTile = null;

               Console.WriteLine("LOG: Unit " + uniqueID + " has died.");
           }
       }
    }
}
