using System;
using System.Collections;
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

        public Game(Server.Client client1, Server.Client client2, DataManager dm)
        {

            player1 = new Player(client1);
            player1.playerAllegiance = Unit.Allegiance.PLAYER_1;
            player2 = new Player(client2);
            player2.playerAllegiance = Unit.Allegiance.PLAYER_2;

            players = new Player[2];
            players[0] = player1;
            players[1] = player2;

            gameOver = false;
            gameThread = new Thread(new ThreadStart(this.gameLoop));

            String setupString = "spawnPieces";

            board = new GameBoard();
            setupString += board.SetBoard(player1.playerClient.clientName, player1.playerClient.boardSetup
                                          , player2.playerClient.clientName, player2.playerClient.boardSetup
                                          , dm);

            player1.isMyTurn = true;

            client1.sw.WriteLine("startGame\\1");
            client2.sw.WriteLine("startGame\\2");


            client1.sw.WriteLine(setupString);
            client2.sw.WriteLine(setupString);

            //We need to send each player the position of each unit on the board

            //player2's setups


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
            foreach (Player currentPlayer in players)
            {

                Queue<string> packetQueue = currentPlayer.playerClient.commandQueue;

                if (packetQueue.Count > 0)
                {
                    String command = packetQueue.Dequeue();
                    string[] message = command.Split(new string[] { "\\" }, StringSplitOptions.None);
                    string log = "NETWORK LOG:";
                    foreach (string s in message)
                    {
                        log += s + "\\";
                    }

                    Console.WriteLine(log);

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

                            if (currentPlayer.playerAllegiance == movingUnit.unitAllegiance                               
                                && currentPlayer.mana >= movingUnit.MovementCost
                                && !movingUnit.Moved
                                && movingUnit.accessibleTiles.Contains(destinationTile))
                            {
                                currentPlayer.mana -= movingUnit.MovementCost;

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

                            if (currentPlayer.playerAllegiance == attackingUnit.unitAllegiance
                                && !attackingUnit.Attacked
                                && currentPlayer.mana >= attackingUnit.AttackCost
                                && attackingUnit.accessibleTiles.Contains(tileToAttack))
                            {
                                Console.WriteLine("LOG: Unit " + attackingUnit.UniqueID + " is attacking tile(" + tileToAttack.x + ", " + tileToAttack.y + ")");
                                currentPlayer.mana -= attackingUnit.AttackCost;

                                //get a list of all the unit IDs that were attacked
                                //apply damage to all units
                                ArrayList unitIDs = attackingUnit.AttackTile(tileToAttack);
                                string sendMessage = "attack\\" + attackingUnit.UniqueID + "\\" + unitIDs.Count;

                                foreach (int id in unitIDs)
                                {
                                    Unit unitHit = board.getUnitByID(id);
                                    unitHit.ApplyDamage(attackingUnit.Damage);
                                    sendMessage += ("\\" + id);
                                }


                                player1.playerClient.sw.WriteLine(sendMessage);
                                player2.playerClient.sw.WriteLine(sendMessage);
                            }

                            //fix this, should only need to perform this once
                            if (board.player1_Guardian.Health <= 0)
                            {
                                board.player1_Soulstone.Invulnerable = false;
                            }
                            if (board.player2_Guardian.Health <= 0)
                            {
                                board.player2_Soulstone.Invulnerable = false;
                            }

                            if (board.player1_Soulstone.Health <= 0)
                            {
                                player1.playerClient.sw.WriteLine("defeat");
                                player2.playerClient.sw.WriteLine("victory");
                                gameOver = true;
                                player1.playerClient.inGame = false;
                                player2.playerClient.inGame = false;
                            }
                            else if (board.player2_Soulstone.Health <= 0)
                            {
                                player1.playerClient.sw.WriteLine("victory");
                                player2.playerClient.sw.WriteLine("defeat");
                                gameOver = true;
                                player1.playerClient.inGame = false;
                                player2.playerClient.inGame = false;
                            }

                            attackingUnit.accessibleTiles.Clear();
                        }
                    }

                    else if (message[0].Equals("endTurn"))
                    {
                        if (currentPlayer.isMyTurn)
                        {
                            int maxMana = switchTurns();
                            player1.playerClient.sw.WriteLine("switchTurns\\{0}", maxMana);
                            player2.playerClient.sw.WriteLine("switchTurns\\{0}", maxMana);
                        }
                    }
                    else if (message[0].Equals("surrender"))
                    {
                        if (currentPlayer.playerAllegiance == Unit.Allegiance.PLAYER_1)
                        {
                            player1.playerClient.sw.WriteLine("defeat");
                            player2.playerClient.sw.WriteLine("victory");
                        }
                        else
                        {
                            player1.playerClient.sw.WriteLine("victory");
                            player2.playerClient.sw.WriteLine("defeat");
                        }

                        gameOver = true;
                        player1.playerClient.inGame = false;
                        player2.playerClient.inGame = false;
                    }
                }
            }
        }
        
        public int switchTurns()
        {
            foreach (KeyValuePair<int, Unit> entry in board.UnitTable)
            {
                entry.Value.Moved = false;
                entry.Value.Attacked = false;
            }

            if (currentTurn == TURN.PLAYER_1)
            {
                player1.IncreaseMana();
                player1.isMyTurn = false;
                player2.isMyTurn = true;
                currentTurn = TURN.PLAYER_2;
                Console.WriteLine("LOG: Player_1 has ended their turn. It is now Player_2's turn.");
                return player2.maxMana;


            }
            else
            {
                player2.IncreaseMana();
                player1.isMyTurn = true;
                player2.isMyTurn = false;
                currentTurn = TURN.PLAYER_1;
                Console.WriteLine("LOG: Player_2 has ended their turn. It is now Player_1's turn.");
                return player1.maxMana;
            }

        }

        public class Player
        {
            public Server.Client playerClient;
            public Unit.Allegiance playerAllegiance;
            public int mana;
            public int maxMana;
            public bool isMyTurn;

            public Player(Server.Client playerClient)
            {
                mana = 2;
                maxMana = 2;
                this.playerClient = playerClient;
            }

            public void IncreaseMana()
            {
                if (maxMana <= 12)
                    maxMana += 2;

                mana = maxMana;
            }
        }
    }
}
