using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    class Unit2 : Unit
    {
        private bool focused;
        private Unit unitFocused;
        private int movementSpeedBuff;
        private int baseMovementRange;
        private int attackBuff;
        private int armorBuff;
        private int armorDebuff;

        public Unit2(int ID)
            : base(ID)
        {
            health = 30;
            maxHealth = 30;
            armor = 0;
            damage = 0;
            movementRange = baseMovementRange = 2;
            movementCost = 1;
            attackCost = 4;
            attackRange = 3;

            focused = false;
            unitFocused = null;
            movementSpeedBuff = 3;
            attackBuff = 3;
            armorBuff = 2;
            armorDebuff = 3;
        }

        public override void moveUnit(GameBoard.Tile destination)
        {
            currentTile.CurrentUnit = null;
            destination.CurrentUnit = this;
            currentTile = destination;

            unfocus();

        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            ArrayList unitsHit = new ArrayList();
            Unit unitHit = tile.CurrentUnit;

            if (unitHit != null)
            {
                unfocus();

                unitFocused = unitHit;
                unitsHit.Add(unitHit.UniqueID);
                if (unitHit.unitAllegiance == unitAllegiance)
                {
                    unitHit.MovementRange += movementSpeedBuff;
                    Console.WriteLine("LOG: Unit {0} has increased Movement Range", unitHit.UniqueID);

                    if (this.level >= 2)
                    {
                        unitFocused.Damage += this.attackBuff;

                        if (this.level >= 3)
                        {
                            unitFocused.Armor += this.armorBuff;
                        }
                    }
                }
                else
                {
                    unitHit.Paralyzed = true;
                    Console.WriteLine("LOG: Unit {0} is paralyzed!", unitHit.UniqueID);

                    if (this.level >= 2)
                    {
                        unitFocused.Armor -= this.armorDebuff;
                    }
                }
                focused = true;
            }
            return unitsHit;
        }



        public override void ApplyDamage(int damage, Unit attackingUnit)
        {
            if (damage >= 0)
            {
                unfocus();
            }

            Console.WriteLine("LOG: Unit {0} takes {1} damage. Is now at {2} health", this.UniqueID, damage, this.health);

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

        public override void setAttackTiles(GameBoard.Tile currentTile, int distance)
        {

            if (distance > 0)
            {
                if (currentTile.UP != null)
                {
                    this.setAttackTiles(currentTile.UP, distance - 1);
                    accessibleTiles.Add(currentTile.UP);
                }
                if (currentTile.DOWN != null)
                {
                    this.setAttackTiles(currentTile.DOWN, distance - 1);
                    accessibleTiles.Add(currentTile.DOWN);
                }
                if (currentTile.RIGHT != null)
                {
                    this.setAttackTiles(currentTile.RIGHT, distance - 1);
                    accessibleTiles.Add(currentTile.RIGHT);
                }
                if (currentTile.LEFT != null)
                {
                    this.setAttackTiles(currentTile.LEFT, distance - 1);
                    accessibleTiles.Add(currentTile.LEFT);
                }
            }
        }

        public void unfocus()
        {
            focused = false;

            if (unitFocused != null)
            {
                if (unitFocused.unitAllegiance == unitAllegiance)
                {
                    unitFocused.MovementRange -= movementSpeedBuff;
                    Console.WriteLine("LOG: Unit {0} debuffed.", unitFocused.UniqueID);

                    if (this.level >= 2)
                    {
                        unitFocused.Damage -= this.attackBuff;

                        if (this.level >= 3)
                        {
                            unitFocused.Armor -= armorBuff;
                        }
                    }

                }
                else
                {
                    unitFocused.Paralyzed = false;
                    Console.WriteLine("LOG: Unit {0} no longer paralyzed.", unitFocused.UniqueID);

                    if (this.level >= 2)
                    {
                        unitFocused.Armor += this.armorDebuff;
                    }

                }
            }
        }

        public override void LevelUp()
        {
            XP = currentXP % 20;

            switch (Level)
            {
                case 3:
                    armorDebuff = 4;
                    break;
            }
        }
    }
}
