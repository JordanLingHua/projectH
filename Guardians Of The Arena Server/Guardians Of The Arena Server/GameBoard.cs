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

                    //set player 1 units
                    //melee units
                    Unit unit = new Units.Unit7(0);
                    unit.CurrentTile = tiles[5, 3];
                    tiles[5, 3].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(1);
                    unit.CurrentTile = tiles[6, 3];
                    tiles[6, 3].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(2);
                    unit.CurrentTile = tiles[9, 3];
                    tiles[9, 3].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(3);
                    unit.CurrentTile = tiles[10, 3];
                    tiles[10, 3].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    //ranged units
                    unit = new Units.Unit7(4);
                    unit.CurrentTile = tiles[5, 2];
                    tiles[5, 2].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(5);
                    unit.CurrentTile = tiles[6, 2];
                    tiles[6, 2].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(6);
                    unit.CurrentTile = tiles[9, 2];
                    tiles[9, 2].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(7);
                    unit.CurrentTile = tiles[10, 2];
                    tiles[10, 2].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    //guardian
                    unit = new Units.Unit7(8);
                    unit.CurrentTile = tiles[7, 0];
                    tiles[7, 0].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    
                    //soulstone
                    unit = new Units.Unit7(9);
                    unit.CurrentTile = tiles[8, 0];
                    tiles[8, 0].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    



                    //set player 2 units
                    //melee units
                    unit = new Units.Unit7(10);
                    unit.CurrentTile = tiles[5, 12];
                    tiles[5, 12].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(11);
                    unit.CurrentTile = tiles[6, 12];
                    tiles[6, 12].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(12);
                    unit.CurrentTile = tiles[9, 12];
                    tiles[9, 12].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(13);
                    unit.CurrentTile = tiles[10, 12];
                    tiles[10, 12].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    //ranged 

                    unit = new Units.Unit7(14);
                    unit.CurrentTile = tiles[5, 13];
                    tiles[5, 13].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(15);
                    unit.CurrentTile = tiles[6, 13];
                    tiles[6, 13].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(16);
                    unit.CurrentTile = tiles[9, 13];
                    tiles[9, 13].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    unit = new Units.Unit7(17);
                    unit.CurrentTile = tiles[10, 13];
                    tiles[10, 13].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    

                    //guardian
                    unit = new Units.Unit7(18);
                    unit.CurrentTile = tiles[7, 15];
                    tiles[7, 15].CurrentUnit = unit;
                    unitTable.Add(unit.UniqueID, unit);
                    
                    //soulstone
                    unit = new Units.Unit7(19);
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