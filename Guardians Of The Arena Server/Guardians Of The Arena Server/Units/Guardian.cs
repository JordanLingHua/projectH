using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Guardian : Unit
    {
        Game gameRef;

        public Guardian(int ID, Game gameRef)
            : base(ID)
        {
            health = 45;
            maxHealth = 45;
            armor = 0;
            damage = 23;
            movementRange = 2;
            movementCost = 1;
            attackCost = 1;
            attackRange = 1;

            this.gameRef = gameRef;
        }

        public override void Attack(Unit unitAttacking)
        {
            int damageDealt = this.damage;

            unitAttacking.ApplyDamage(this.damage, this);
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

       public override void ApplyDamage(int damage, Unit attackingUnit)
       {

           if (this.level >= 2 && damage > 10)
               damage = 10;

           damage -= armor;

           if (this.level >= 3 && damage > 0)
           {
               //int baseDamage = this.damage;
               //this.damage = 5;
               //gameRef.sendAttackedUnits(AttackTile(attackingUnit.CurrentTile), this);
               //this.damage = baseDamage;             

               attackingUnit.ApplyDamage(5, this);
           }


           this.health -= damage;

           if (health > maxHealth)
               health = maxHealth;

           Console.WriteLine("LOG: Unit {0} deals {1} damage to Unit {2}. Is now at {3} health"
                                , attackingUnit.UniqueID, damage, this.uniqueID, health);


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
