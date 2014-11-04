using System;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server
{
    public class GameBoard
    {
        private short width = 16;
        private short height = 16;
        private Tile[,] tiles;

        public Tile[,] Tiles
        {
            get { return tiles; }
        }

        public GameBoard()
        {
            tiles = new Tile[width, height];

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
                    tiles[5, 3].CurrentUnit = new Unit(3,1,1);
                    tiles[6, 3].CurrentUnit = new Unit(3,1,1);
                    tiles[9, 3].CurrentUnit = new Unit(3,1,1);
                    tiles[10, 3].CurrentUnit = new Unit(3,1,1);
                    //ranged units
                    tiles[5, 2].CurrentUnit = new Unit(4,1,2);
                    tiles[6, 2].CurrentUnit = new Unit(4,1,2);
                    tiles[9, 2].CurrentUnit = new Unit(4,1,2);
                    tiles[10, 2].CurrentUnit = new Unit(4,1,2);
                    //guardian
                    tiles[7, 0].CurrentUnit = new Unit(1,3,1);
                    //soulstone
                    tiles[8, 0].CurrentUnit = new Unit(0,0,0);

                    //set player 2 units
                    //melee units
                    tiles[5, 12].CurrentUnit = new Unit(3,1,1);
                    tiles[6, 12].CurrentUnit = new Unit(3,1,1);
                    tiles[9, 12].CurrentUnit = new Unit(3,1,1);
                    tiles[10, 12].CurrentUnit = new Unit(3,1,1);
                    //ranged 
                    tiles[5, 13].CurrentUnit = new Unit(4,1,2);
                    tiles[6, 13].CurrentUnit = new Unit(4,1,2);
                    tiles[9, 13].CurrentUnit = new Unit(4,1,2);
                    tiles[10, 13].CurrentUnit = new Unit(4,1,2);
                    //guardian
                    tiles[7, 15].CurrentUnit = new Unit(1,3,1);
                    //soulstone
                    tiles[8, 15].CurrentUnit = new Unit(0,0,0);
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