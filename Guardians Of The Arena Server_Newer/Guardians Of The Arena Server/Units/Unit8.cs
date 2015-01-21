using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    public class Unit8 : Unit
    {

        private int levelThreeBonus_Range = 2;
        private int levelThreeBonus_Heal = -10;
        private List<Unit> levelThreeBonus_UnitList;

        public Unit8(int ID)
            : base(ID)
        {
            health = 20;
            maxHealth = 20;
            armor = 35;
            damage = -20;
            movementRange = 3;
            movementCost = 1;
            attackCost = 4;
            attackRange = 4; //target based

            levelThreeBonus_UnitList = new List<Unit>();
        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            ArrayList unitsHit = new ArrayList();
            if (tile.CurrentUnit != null)
            {
                unitsHit.Add(tile.CurrentUnit.UniqueID);

            }
            return unitsHit;
        }

        public override void Attack(Unit unitAttacking)
        {
            unitAttacking.ApplyDamage(this.damage);

            if (level >= 3)
            {
                getLevelThreeBonusHealedUnits(unitAttacking.CurrentTile, levelThreeBonus_Range);

                foreach (Unit u in levelThreeBonus_UnitList)
                {
                    u.ApplyDamage(levelThreeBonus_Heal);
                }

                levelThreeBonus_UnitList.Clear();
            }
        }

        public void getLevelThreeBonusHealedUnits(GameBoard.Tile currentTile, int distance)
        {
            if (distance > 0)
            {
                if (currentTile.UP != null)
                {
                    getLevelThreeBonusHealedUnits(currentTile.UP, distance - 1);

                    if (currentTile.UP.CurrentUnit != null)// && !levelThreeBonus_UnitList.Exists(currentTile.UP.CurrentUnit)) 
                        levelThreeBonus_UnitList.Add(currentTile.UP.CurrentUnit);
                }
                if (currentTile.DOWN != null)
                {
                    getLevelThreeBonusHealedUnits(currentTile.DOWN, distance - 1);

                    if (currentTile.DOWN.CurrentUnit != null) 
                        levelThreeBonus_UnitList.Add(currentTile.DOWN.CurrentUnit);
                }
                if (currentTile.RIGHT != null)
                {
                    getLevelThreeBonusHealedUnits(currentTile.RIGHT, distance - 1);

                    if (currentTile.RIGHT.CurrentUnit != null) 
                        levelThreeBonus_UnitList.Add(currentTile.RIGHT.CurrentUnit);
                }
                if (currentTile.LEFT != null)
                {
                    getLevelThreeBonusHealedUnits(currentTile.LEFT, distance - 1);

                    if (currentTile.LEFT.CurrentUnit != null) 
                        levelThreeBonus_UnitList.Add(currentTile.LEFT.CurrentUnit);
                }
            }
        }

        public override void setAttackTiles(GameBoard.Tile currentTile, int distance)
        {
            if (distance > 0)
            {
                if (currentTile.UP != null) 
                {
                    setAccessibleTiles(currentTile.UP, distance - 1);
                    accessibleTiles.Add(currentTile.UP);
                }
                if (currentTile.DOWN != null )
                {
                    setAccessibleTiles(currentTile.DOWN, distance - 1);
                    accessibleTiles.Add(currentTile.DOWN);
                }
                if (currentTile.RIGHT != null )
                {
                    setAccessibleTiles(currentTile.RIGHT, distance - 1);
                    accessibleTiles.Add(currentTile.RIGHT);
                }
                if (currentTile.LEFT != null )
                {
                    setAccessibleTiles(currentTile.LEFT, distance - 1);
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
