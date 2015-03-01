using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server
{
    public class GameBoard
    {
        private short width = 9;
        private short height = 9;
        private Tile[,] tiles;
        private Dictionary<int, Unit> unitTable;
        private int unitCount;
        private Game gameRef;

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

        public GameBoard(Game gameRef)
        {
            this.gameRef = gameRef;

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
            System.Data.SQLite.SQLiteDataReader reader = dm.getGameSetup(player1Name, player1Setup);

            string returnString = "";

            unitCount = 1;
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
                            unit = new Units.Guardian(unitCount, gameRef);
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
            reader = dm.getGameSetup(player2Name, player2Setup);

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
                            unit = new Units.Guardian(unitCount, gameRef);
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

        //create all the trees and return a string with all of their locations
        public string spawnTrees()
        {
            string returnString = "";

            #region Trees Region

            /*

            Unit tree = new Units.Tree(100);
            tree.CurrentTile = tiles[10, 6];
            tiles[10, 6].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;


            tree = new Units.Tree(101);
            tree.CurrentTile = tiles[10, 5];
            tiles[10, 5].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(102);
            tree.CurrentTile = tiles[10, 4];
            tiles[10, 4].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(103);
            tree.CurrentTile = tiles[0, 4];
            tiles[0, 4].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(104);
            tree.CurrentTile = tiles[0, 5];
            tiles[0, 5].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(105);
            tree.CurrentTile = tiles[0, 6];
            tiles[0, 6].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(106);
            tree.CurrentTile = tiles[4, 4];
            tiles[4, 4].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(107);
            tree.CurrentTile = tiles[3, 4];
            tiles[3, 4].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(108);
            tree.CurrentTile = tiles[4, 6];
            tiles[4, 6].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(109);
            tree.CurrentTile = tiles[3, 6];
            tiles[3, 6].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(110);
            tree.CurrentTile = tiles[6, 6];
            tiles[6, 6].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(111);
            tree.CurrentTile = tiles[7, 6];
            tiles[7, 6].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(112);
            tree.CurrentTile = tiles[6, 4];
            tiles[6, 4].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;


            tree = new Units.Tree(113);
            tree.CurrentTile = tiles[7, 4];
            tiles[7, 4].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y + "\\endSpawnObstacles";

             
             */
 
            #endregion


            Unit tree = new Units.Tree(101);
            tree.CurrentTile = tiles[4, 4];
            tiles[4, 5].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(103);
            tree.CurrentTile = tiles[1, 4];
            tiles[1, 4].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(104);
            tree.CurrentTile = tiles[1, 5];
            tiles[1, 5].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y;

            tree = new Units.Tree(105);
            tree.CurrentTile = tiles[7, 4];
            tiles[7, 4].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y ;

            tree = new Units.Tree(106);
            tree.CurrentTile = tiles[7, 3];
            tiles[7, 3].CurrentUnit = tree;
            unitTable.Add(tree.UniqueID, tree);
            returnString += "\\" + tree.UniqueID + "\\" + tree.CurrentTile.x + "\\" + tree.CurrentTile.y + "\\endSpawnTrees";

            return returnString;
        }

        public string SpawnRocks()
        {
            string returnString = "";
            int rockID = 500;

            Unit rock = new Units.InvulnerableRock(rockID++);
            rock.CurrentTile = tiles[0, 5];
            tiles[0, 5].CurrentUnit = rock;
            unitTable.Add(rock.UniqueID, rock);
            returnString += "\\" + rock.UniqueID + "\\" + rock.CurrentTile.x + "\\" + rock.CurrentTile.y;

            rock = new Units.InvulnerableRock(rockID++);
            rock.CurrentTile = tiles[0, 4];
            tiles[0, 4].CurrentUnit = rock;
            unitTable.Add(rock.UniqueID, rock);
            returnString += "\\" + rock.UniqueID + "\\" + rock.CurrentTile.x + "\\" + rock.CurrentTile.y;

            rock = new Units.InvulnerableRock(rockID++);
            rock.CurrentTile = tiles[2, 5];
            tiles[2, 5].CurrentUnit = rock;
            unitTable.Add(rock.UniqueID, rock);
            returnString += "\\" + rock.UniqueID + "\\" + rock.CurrentTile.x + "\\" + rock.CurrentTile.y;

            rock = new Units.InvulnerableRock(rockID++);
            rock.CurrentTile = tiles[4, 3];
            tiles[4, 3].CurrentUnit = rock;
            unitTable.Add(rock.UniqueID, rock);
            returnString += "\\" + rock.UniqueID + "\\" + rock.CurrentTile.x + "\\" + rock.CurrentTile.y;

            rock = new Units.InvulnerableRock(rockID++);
            rock.CurrentTile = tiles[4, 5];
            tiles[4, 5].CurrentUnit = rock;
            unitTable.Add(rock.UniqueID, rock);
            returnString += "\\" + rock.UniqueID + "\\" + rock.CurrentTile.x + "\\" + rock.CurrentTile.y;

            rock = new Units.InvulnerableRock(rockID++);
            rock.CurrentTile = tiles[8, 3];
            tiles[8, 3].CurrentUnit = rock;
            unitTable.Add(rock.UniqueID, rock);
            returnString += "\\" + rock.UniqueID + "\\" + rock.CurrentTile.x + "\\" + rock.CurrentTile.y;

            rock = new Units.InvulnerableRock(rockID++);
            rock.CurrentTile = tiles[6, 3];
            tiles[6, 3].CurrentUnit = rock;
            unitTable.Add(rock.UniqueID, rock);
            returnString += "\\" + rock.UniqueID + "\\" + rock.CurrentTile.x + "\\" + rock.CurrentTile.y;

            rock = new Units.InvulnerableRock(rockID++);
            rock.CurrentTile = tiles[8, 4];
            tiles[8, 4].CurrentUnit = rock;
            unitTable.Add(rock.UniqueID, rock);
            returnString += "\\" + rock.UniqueID + "\\" + rock.CurrentTile.x + "\\" + rock.CurrentTile.y + "\\endSpawnRocks";

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