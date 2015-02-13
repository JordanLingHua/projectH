using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    class Unit7 : Unit
    {
        private int attackCountPerTurn = 0;

        public Unit7(int ID)
            : base(ID)
        {
            health = 38;
            maxHealth = 38;
            armor = 0;
            damage = 10;
            movementRange = 3;
            movementCost = 1;
            attackCost = 1;
            attackRange = 1;
        }

        public override void Attack(Unit unitAttacking)
        {
            attackCountPerTurn++;

            if (level >= 2)
            {
                if (unitAttacking.unitAllegiance != this.unitAllegiance)
                {
                    this.ApplyDamage(-10, this);
                }
            }

            if (level >= 3)
            {
                if (attackCountPerTurn < 2)
                {
                    this.Attacked = false;
                }
                else
                {
                    attackCountPerTurn = 0;
                }
            }

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

            switch(level)
            {
                case 3:
                    alreadyAttacked = false;
                    attackCountPerTurn = 1;
                    break;
            }
        }

    }
}
