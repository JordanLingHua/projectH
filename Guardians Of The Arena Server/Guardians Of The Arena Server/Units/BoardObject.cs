using System;
using System.Collections.Generic;
using System.Text;

namespace Guardians_Of_The_Arena_Server
{
    public abstract class BoardObject
    {
        protected GameBoard.Tile currentTile;

        public GameBoard.Tile CurrentTile
        {
            get { return currentTile; }
            set { currentTile = value; }
        }


    }
}
