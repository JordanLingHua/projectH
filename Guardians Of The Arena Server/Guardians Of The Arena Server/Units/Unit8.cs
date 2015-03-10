using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Unit8 : Unit
    {

        private int levelThreeBonus_Range = 1;
        private int levelThreeBonus_Heal = -10;
        private int unitsHealed = 0;
        private int unitsToHeal = 0;
        private List<Unit> levelThreeBonus_UnitList;

        public Unit8(int ID)
            : base(ID)
        {
            health = 25;
            maxHealth = 25;
            armor = 0;
            damage = -20;
            movementRange = 3;
            movementCost = 1;
            attackCost = 3;
            attackRange = 4; //target based

            levelThreeBonus_UnitList = new List<Unit>();
        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            ArrayList unitsHit = new ArrayList();
            if (tile.CurrentUnit != null)
            {
                unitsHit.Add(tile.CurrentUnit.UniqueID); //first unit in the list will receive the most healing
                
                if (level >= 3)
                {
                    getLevelThreeBonusHealedUnits(tile, levelThreeBonus_Range);

                    foreach (Unit u in levelThreeBonus_UnitList)
                    {
                        //u.ApplyDamage(levelThreeBonus_Heal);
                        unitsHit.Add(u.UniqueID);
                    }

                    levelThreeBonus_UnitList.Clear();
                    
                }
            }

            unitsToHeal = unitsHit.Count;
            return unitsHit;
        }

        public override void Attack(Unit unitAttacking)
        {
            int damageApplied = this.damage;
            

            if (unitsHealed > 0)
                damageApplied = levelThreeBonus_Heal;

            unitAttacking.ApplyDamage(damageApplied, this);

            unitsHealed++;

            if (unitsHealed == unitsToHeal)
                unitsHealed = 0;
        }

        public void getLevelThreeBonusHealedUnits(GameBoard.Tile currentTile, int distance)
        {
            if (distance > 0)
            {
                if (currentTile.UP != null)
                {
                    getLevelThreeBonusHealedUnits(currentTile.UP, distance - 1);

                    if (currentTile.UP.CurrentUnit != null
                        && currentTile.UP.CurrentUnit.unitAllegiance == this.unitAllegiance
                        && !levelThreeBonus_UnitList.Contains(currentTile.UP.CurrentUnit))
                    {

                        levelThreeBonus_UnitList.Add(currentTile.UP.CurrentUnit);
                    }
                }
                if (currentTile.DOWN != null)
                {
                    getLevelThreeBonusHealedUnits(currentTile.DOWN, distance - 1);

                    if (currentTile.DOWN.CurrentUnit != null
                        && currentTile.DOWN.CurrentUnit.unitAllegiance == this.unitAllegiance
                        && !levelThreeBonus_UnitList.Contains(currentTile.DOWN.CurrentUnit))
                    {
                        levelThreeBonus_UnitList.Add(currentTile.DOWN.CurrentUnit);
                    }
                }
                if (currentTile.RIGHT != null)
                {
                    getLevelThreeBonusHealedUnits(currentTile.RIGHT, distance - 1);

                    if (currentTile.RIGHT.CurrentUnit != null
                        && currentTile.RIGHT.CurrentUnit.unitAllegiance == this.unitAllegiance
                        && !levelThreeBonus_UnitList.Contains(currentTile.RIGHT.CurrentUnit))
                    {
                        levelThreeBonus_UnitList.Add(currentTile.RIGHT.CurrentUnit);
                    }
                }
                if (currentTile.LEFT != null)
                {
                    getLevelThreeBonusHealedUnits(currentTile.LEFT, distance - 1);

                    if (currentTile.LEFT.CurrentUnit != null
                        && currentTile.LEFT.CurrentUnit.unitAllegiance == this.unitAllegiance
                        && !levelThreeBonus_UnitList.Contains(currentTile.LEFT.CurrentUnit))
                    {
                        levelThreeBonus_UnitList.Add(currentTile.LEFT.CurrentUnit);
                    }
                }
            }
        }

        public override void setAttackTiles(GameBoard.Tile currentTile, int distance)
        {
            if (distance > 0)
            {
                if (currentTile.UP != null) 
                {
                    setAttackTiles(currentTile.UP, distance - 1);
                    accessibleTiles.Add(currentTile.UP);
                }
                if (currentTile.DOWN != null )
                {
                    setAttackTiles(currentTile.DOWN, distance - 1);
                    accessibleTiles.Add(currentTile.DOWN);
                }
                if (currentTile.RIGHT != null )
                {
                    setAttackTiles(currentTile.RIGHT, distance - 1);
                    accessibleTiles.Add(currentTile.RIGHT);
                }
                if (currentTile.LEFT != null )
                {
                    setAttackTiles(currentTile.LEFT, distance - 1);
                    accessibleTiles.Add(currentTile.LEFT);
                }
            }
        }

        public override void LevelUp()
        {
            XP = currentXP % 20;

            switch (Level)
            {
                case 2:
                    damage = -5000;
                    break;
                default:
                    break;
            }
        }
    }
}
