using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server
{
    public class GameBoard
    {
        private short width = 11;
        private short height = 11;
        private Tile[,] tiles;
        private Dictionary<int, Unit> unitTable;

        //need to know what is happening with the guardian and soulstone at all times
        //so we will have to keep track of them
        public Units.Guardian player1_Guardian;
        public Units.Soulstone player1_Soulstone;
        public Units.Guardian player2_Guardian;
        public Units.Soulstone player2_Soulstone;

        public Tile[,] Tiles
        {
            get { return tiles; }
        }

        public Dictionary<int, Unit> UnitTable
        {
            get { return unitTable; }
        }

        public short Width
        {
            get { return width; }
        }

        public short Height
        {
            get { return height; }
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
        }
        
        public string SetBoard(string player1Name, int player1Setup, string player2Name, int player2Setup, DataManager dm)
        {
            //we will create the GameBoard using the custom setups of each player
            System.Data.SQLite.SQLiteDataReader reader = dm.getBoardSetup(player1Name, player1Setup);

            string returnString = "";

            int unitCount = 0;
            while (reader.Read())
            {
                
                int unitType = Int32.Parse(reader["unitType"].ToString());
                int tileX = Int32.Parse(reader["x"].ToString());
                int tileY = Int32.Parse(reader["y"].ToString());
                Unit unit;

                returnString += "\\" + unitType + "\\" + unitCount + "\\" + tileX + "\\" + tileY;

                switch (unitType)
                {
                    case 1:
                        {
                            unit = new Units.Unit1(unitCount);
                            break;
                        }
                    case 2:
                        {
                            unit = new Units.Unit2(unitCount);
                            break;
                        }
                    case 3:
                        {
                            unit = new Units.Unit3(unitCount);
                            break;
                        }
                    case 7:
                        {
                            unit = new Units.Unit7(unitCount);
                            break;
                        }
                    case 8:
                        {
                            unit = new Units.Unit8(unitCount);
                            break;
                        }
                    case 10:
                        {
                            unit = new Units.Guardian(unitCount);
                            player1_Guardian = (Units.Guardian)unit;
                            break;
                        }
                    case 11:
                        {
                            unit = new Units.Soulstone(unitCount);
                            player1_Soulstone = (Units.Soulstone)unit;
                            break;
                        }
                    default:
                        {
                            unit = new Units.Unit1(unitCount);
                            break;
                        }


                }

                unit.unitAllegiance = Unit.Allegiance.PLAYER_1;
                unit.CurrentTile = tiles[tileX, tileY];
                tiles[tileX, tileY].CurrentUnit = unit;
                unitTable.Add(unit.UniqueID, unit);

                unitCount++;
            }

            returnString += "\\EndPlayer1";

            //We will now set the board for player 2's units
            reader = dm.getBoardSetup(player2Name, player2Setup);

            while (reader.Read())
            {
                
                int unitType = Int32.Parse(reader["unitType"].ToString());
                int tileX = (width - 1) - Int32.Parse(reader["x"].ToString());
                int tileY = (height - 1) - Int32.Parse(reader["y"].ToString());
                Unit unit;

                returnString += "\\" + unitType + "\\" + unitCount + "\\" + tileX + "\\" + tileY;

                switch (unitType)
                {
                    case 1:
                        {
                            unit = new Units.Unit1(unitCount);
                            break;
                        }
                    case 2:
                        {
                            unit = new Units.Unit2(unitCount);
                            break;
                        }
                    case 3:
                        {
                            unit = new Units.Unit3(unitCount);
                            break;
                        }
                    case 7:
                        {
                            unit = new Units.Unit7(unitCount);
                            break;
                        }
                    case 8:
                        {
                            unit = new Units.Unit8(unitCount);
                            break;
                        }
                    case 10:
                        {
                            unit = new Units.Guardian(unitCount);
                            player2_Guardian = (Units.Guardian)unit;
                            break;
                        }
                    case 11:
                        {
                            unit = new Units.Soulstone(unitCount);
                            player2_Soulstone = (Units.Soulstone)unit;
                            break;
                        }
                    default:
                        {
                            unit = new Units.Unit1(unitCount);
                            break;
                        }


                }

                unit.unitAllegiance = Unit.Allegiance.PLAYER_2;
                unit.CurrentTile = tiles[tileX, tileY];
                tiles[tileX, tileY].CurrentUnit = unit;
                unitTable.Add(unit.UniqueID, unit);

                unitCount++;
            }

            returnString += "\\EndPlayer2";

            return returnString;
        }

        public void moveUnit(Tile start, Tile destination)
        {
            Unit unitToMove = start.CurrentUnit;
            destination.CurrentUnit = unitToMove;
            unitToMove.CurrentTile = destination;
            unitToMove.Moved = true;
            start.CurrentUnit = null;
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