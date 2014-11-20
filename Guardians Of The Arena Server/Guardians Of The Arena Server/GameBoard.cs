using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server
{
    public class GameBoard
    {
        private short width = 16;
        private short height = 16;
        private Tile[,] tiles;
        private Dictionary<int, Unit> unitTable;

        //need to know what is happening with the guardian and soulstone at all times
        //so we will have to keep track of them
        public Units.Guardian player1_Guardian;
        public Units.Soulstone player1_Soulstone;
        public Units.Guardian player2_Gaurdian;
        public Units.Soulstone player2_Soulsone;

        public Tile[,] Tiles
        {
            get { return tiles; }
        }

        public Dictionary<int, Unit> UnitTable
        {
            get { return unitTable; }
        }

        public GameBoard()
        {
            tiles = new Tile[width, height];
            unitTable = new Dictionary<int, Unit>();

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tiles[i, j] = new Tile();
                }
            }

            //set neighbors appropriately
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    GameBoard.Tile tile = tiles[i, j];

                    //set tile id e.g. 5,2
                    tile.x = i;
                    tile.y = j;

                    if (i != 0)
                    {
                        tile.DOWN = tiles[i - 1, j];
                    }
                    if (i != width - 1)
                    {
                        tile.UP = tiles[i + 1, j];
                    }
                    if (j != 0)
                    {
                        tile.RIGHT = tiles[i, j - 1];
                    }
                    if (j != height - 1)
                    {
                        tile.LEFT = tiles[i, j + 1];
                    }
                }
            }


            setGameBoard(1);

            

        }

        public void setGameBoard(int board)
        {
            switch (board)
            {
                case 1 :
                    //set trees
                    #region Trees Region
                    tiles[0, 7].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[2, 2].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[3, 7].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[4, 7].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[15, 7].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[13, 7].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[12, 7].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[11, 7].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[0, 8].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[2, 8].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[3, 8].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[4, 8].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[15, 8].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[13, 8].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[12, 8].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[11, 8].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[7, 7].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[7, 8].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[8, 7].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    tiles[8, 8].Occupation = Tile.Occupied.ENVIRONMENT_OBJECT;
                    #endregion

                    //set player 1 units
                    //melee units
                    #region Player 1 Melee Units Region
                    Unit unit = new Units.Unit7(0);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                    unit.CurrentTile = tiles[5, 3];
                    tiles[5, 3].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(1);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                    unit.CurrentTile = tiles[6, 3];
                    tiles[6, 3].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(2);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                    unit.CurrentTile = tiles[9, 3];
                    tiles[9, 3].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(3);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                    unit.CurrentTile = tiles[10, 3];
                    tiles[10, 3].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    #endregion

                    //ranged units
                    #region Player1 Ranged Units Region
                    unit = new Units.Unit1(4);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                    unit.CurrentTile = tiles[5, 2];
                    tiles[5, 2].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit1(5);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                    unit.CurrentTile = tiles[6, 2];
                    tiles[6, 2].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit1(6);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                    unit.CurrentTile = tiles[9, 2];
                    tiles[9, 2].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    
                    //THIS IS NOW A TEMPLAR
                    unit = new Units.Unit3(7);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                    unit.CurrentTile = tiles[10, 2];
                    tiles[10, 2].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    #endregion

                    //guardian
                    unit = new Units.Guardian(8);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                    player1_Guardian = (Units.Guardian)unit;
                    unit.CurrentTile = tiles[7, 0];
                    tiles[7, 0].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    
                    //soulstone
                    unit = new Units.Soulstone(9);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                    player1_Soulstone = (Units.Soulstone)unit;
                    unit.CurrentTile = tiles[8, 0];
                    tiles[8, 0].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    //set player 2 units
                    //melee units
                    #region Player 2 Melee Units
                    unit = new Units.Unit7(10);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                    unit.CurrentTile = tiles[5, 12];
                    tiles[5, 12].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(11);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                    unit.CurrentTile = tiles[6, 12];
                    tiles[6, 12].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(12);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                    unit.CurrentTile = tiles[9, 12];
                    tiles[9, 12].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(13);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                    unit.CurrentTile = tiles[10, 12];
                    tiles[10, 12].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    #endregion

                    #region Player2 Ranged Units Region
                    unit = new Units.Unit1(14);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                    unit.CurrentTile = tiles[5, 13];
                    tiles[5, 13].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit1(15);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                    unit.CurrentTile = tiles[6, 13];
                    tiles[6, 13].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit1(16);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                    unit.CurrentTile = tiles[9, 13];
                    tiles[9, 13].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    
                    //THIS IS NOW A TEMPLAR
                    unit = new Units.Unit3(17);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                    unit.CurrentTile = tiles[10, 13];
                    tiles[10, 13].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    
                    #endregion

                    //guardian
                    unit = new Units.Guardian(18);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                    player2_Gaurdian = (Units.Guardian)unit;
                    unit.CurrentTile = tiles[7, 15];
                    tiles[7, 15].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    
                    //soulstone
                    unit = new Units.Soulstone(19);
                    unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                    player2_Soulsone = (Units.Soulstone)unit;
                    unit.CurrentTile = tiles[8, 15];
                    tiles[8, 15].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    
                    break;
            }
        }

        public void moveUnit(Tile start, Tile destination)
        {
            Unit unitToMove = start.CurrentUnit;
            start.CurrentUnit = null;
            destination.CurrentUnit = unitToMove;
            unitToMove.CurrentTile = destination;
        }

        public Unit getUnitByID(int ID)
        {
            if (unitTable.ContainsKey(ID))
                return unitTable[ID];

            return null;
        }

        public class Tile
        {
            public enum Occupied { UNIT, ENVIRONMENT_OBJECT, EMPTY };
            private Occupied occupation = Occupied.EMPTY;
            private Unit currentUnit;

            public Tile LEFT, RIGHT, DOWN, UP;
            //ID
            public int x, y;

            public Occupied Occupation
            {
                get { return occupation; }
                set { occupation = value; }
            }

            public Unit CurrentUnit
            {
                get { return currentUnit; }
                set { currentUnit = value; }
            }

            public Tile()
            {

            }


        }

    }
}