using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server.Units
{
    class InvulnerableRock : Unit
    {
        public InvulnerableRock(int ID)
            : base(ID)
        {
            health = 100000;
            maxHealth = 100000;
            armor = 800;
            damage = 5000;
            movementRange = 0;
            movementCost = 0;
            attackCost = 0;
            attackRange = 0;
        }

        public override ArrayList AttackTile(GameBoard.Tile tile)
        {
            throw new NotImplementedException();
        }

        public override void ApplyDamage(int damage, Unit attackingUnit)
        {
            Console.WriteLine("LOG: THIS ROCK IS TOO POWERFUL TO DESTROY!!!!");
        }
        public override void LevelUp()
        {
          
        }
    }
}
