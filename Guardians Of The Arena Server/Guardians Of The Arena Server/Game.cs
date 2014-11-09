using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Guardians_Of_The_Arena_Server
{
    public class Game
    {

        private GameBoard board;

        protected Player player1;
        protected Player player2;
        protected bool gameOver;
        protected Thread gameThread;
        protected Player[] players;

        public enum TURN { PLAYER_1, PLAYER_2 }
        public TURN currentTurn = TURN.PLAYER_1;
        public int currentPlay = 0;


        public bool GameOver
        {
            get { return gameOver; }
        }

        public Thread GameThread
        {
            get { return gameThread; }
        }

        public Game(Server.Client client1, Server.Client client2)
        {

            player1 = new Player(client1);
            player2 = new Player(client2);

            players = new Player[2];
            players[0] = player1;
            players[1] = player2;

            gameOver = false;
            gameThread = new Thread(new ThreadStart(this.gameLoop));

            board = new GameBoard();

            player1.isMyTurn = true;

            client1.sw.WriteLine("startGame\\1");
            client2.sw.WriteLine("startGame\\2");

            Console.WriteLine("Game between " + player1.playerClient.clientName
                    + " and " + player2.playerClient.clientName + " has started.");
        }

        public void gameLoop()
        {
            while (!gameOver)
            {
                readPackets();
            }
        }

        //we will remove the packet from the queue and act accordingly
        public void readPackets()
        {
            Player currentPlayer = player1;

            if (!player1.isMyTurn)
                currentPlayer = player2;

            Queue<string> packetQueue = currentPlayer.playerClient.commandQueue;

            if (packetQueue.Count > 0)
            {
                String command = packetQueue.Dequeue();
                string[] message = command.Split(new string[] { "\\" }, StringSplitOptions.None);

                //move message sends the command, the tile we are coming from and the tile we are going too
                if (message[0].Equals("move"))
                {
                    //if it is the turn of the client that sent this command
                    //AND if the tile we want to move to is in the list off accessible tiles of the object on the starting tile
                    //THEN we send a packet to both clients allow the object to move tiles.
                    if (currentPlayer.isMyTurn)
                    {
                        Unit movingUnit = board.getUnitByID(Int32.Parse(message[1]));
                        GameBoard.Tile startTile = movingUnit.CurrentTile;
                        GameBoard.Tile destinationTile = board.Tiles[Int32.Parse(message[2]), Int32.Parse(message[3])];
                        movingUnit.setAccessibleTiles(startTile, movingUnit.MovementRange);

                        if (currentPlayer.currentMana >= movingUnit.MovementCost
                            && movingUnit.accessibleTiles.Contains(destinationTile))
                        {
                            currentPlayer.currentMana -= movingUnit.MovementCost;

                            Console.WriteLine("LOG: moving unit from tile(" + startTile.x + "," + startTile.y + ") to tile(" + destinationTile.x + "," + destinationTile.y + ")");
                            board.moveUnit(startTile, destinationTile);

                            //Tell the clients to move the unit
                            player2.playerClient.sw.WriteLine("move\\" + message[1] + "\\" + destinationTile.x + "\\" + destinationTile.y);

                            player1.playerClient.sw.WriteLine("move\\" + message[1] + "\\" + destinationTile.x + "\\" + destinationTile.y);
                        }

                        movingUnit.accessibleTiles.Clear();
                    }
                }

                else if (message[0].Equals("attack"))
                {
                    if (currentPlayer.isMyTurn)
                    {
                        Unit attackingUnit = board.getUnitByID(Int32.Parse(message[1]));
                        GameBoard.Tile tileToAttack = board.Tiles[Int32.Parse(message[2]), Int32.Parse(message[3])];
                        attackingUnit.setAttackTiles(attackingUnit.CurrentTile, attackingUnit.AttackRange);

                        if (currentPlayer.currentMana >= attackingUnit.AttackCost
                            && attackingUnit.accessibleTiles.Contains(tileToAttack))
                        {
                            Console.WriteLine("LOG: Unit " + attackingUnit.UniqueID + " is attacking tile(" + tileToAttack.x + ", " + tileToAttack.y + ")");
                            currentPlayer.currentMana -= attackingUnit.AttackCost;

                            if (tileToAttack.CurrentUnit != null)
                                tileToAttack.CurrentUnit.ApplyDamage(attackingUnit.Damage);

                            player1.playerClient.sw.WriteLine("attack\\" + message[1] + "\\" + message[2]);
                            player2.playerClient.sw.WriteLine("attack\\" + message[1] + "\\" + message[2]);
                        }

                        attackingUnit.accessibleTiles.Clear();
                    }
                }

                else if (message[0].Equals("endTurn"))
                {
                    if (currentPlayer.isMyTurn)
                    {
                        switchTurns();
                        currentPlayer.IncreaseMana();
                        player1.playerClient.sw.WriteLine("switchTurns");
                        player2.playerClient.sw.WriteLine("switchTurns");
                    }
                }
            }
        }
        
        public void switchTurns()
        {
            if (currentTurn == TURN.PLAYER_1)
            {
                player1.isMyTurn = false;
                player2.isMyTurn = true;
                currentTurn = TURN.PLAYER_2;
                Console.WriteLine("LOG: Player_1 has ended their turn. It is now Player_2's turn.");
            }
            else
            {
                player1.isMyTurn = true;
                player2.isMyTurn = false;
                currentTurn = TURN.PLAYER_1;
                Console.WriteLine("LOG: Player_2 has ended their turn. It is now Player_1's turn.");
            }

        }

        public class Player
        {
            public Server.Client playerClient;
            public int mana;
            public int currentMana;
            public readonly int maxMana;
            public bool isMyTurn;

            public Player(Server.Client playerClient)
            {
                mana = 1;
                currentMana = mana;
                maxMana = 8;
                this.playerClient = playerClient;
            }

            public void IncreaseMana()
            {
                if (maxMana < 8)
                    mana += 1;

                currentMana = mana;
            }
        }
    }
}
