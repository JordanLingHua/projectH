using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data.SQLite;

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

        public bool isAI_Game = false;
        public enum TURN { PLAYER_1, PLAYER_2 }
        public TURN currentTurn = TURN.PLAYER_1;
        public int currentPlay = 0;

        private DataManager dm;


        public bool GameOver
        {
            get { return gameOver; }
        }

        public Thread GameThread
        {
            get { return gameThread; }
        }

        public Game(Server.Client client1, Server.Client client2, DataManager dm, bool isAI_Game)
        {
            this.dm = dm;
            this.isAI_Game = isAI_Game;
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

            board = new GameBoard(this);
            setupString += board.SetBoard(player1.playerClient.clientName, player1.playerClient.boardSetup
                                          , player2.playerClient.clientName, player2.playerClient.boardSetup
                                          , dm);

            player1.isMyTurn = true;

            if (!isAI_Game)
            {
                client1.sw.WriteLine("startGame\\1");
                client2.sw.WriteLine("startGame\\2");
            }
            else
            {
                client1.sw.WriteLine("startAI");
            }

            string treeString = board.spawnTrees();
            string rockString = board.SpawnRocks();

            client1.sw.WriteLine("spawnTrees{0}", treeString);
            client1.sw.WriteLine("spawnRocks{0}", rockString);
            if (!isAI_Game) { 
                client2.sw.WriteLine("spawnTrees{0}", treeString);
                client2.sw.WriteLine("spawnRocks{0}", rockString);
            }

            client1.sw.WriteLine(setupString);
            if (!isAI_Game) { client2.sw.WriteLine(setupString); }

            //We need to send each player the position of each unit on the board

            //player2's setups
            //send the tooltips
            SQLiteDataReader reader = dm.getTooltips(client1.clientName, 3);
            string tooltip = "showTip\\" + reader["tooltipID"];
            string showTip = "" + reader["showTip"];

            if (showTip.Equals("True"))
                client1.sw.WriteLine(tooltip);

            reader = dm.getTooltips(client2.clientName, 3);
            tooltip = "showTip\\" + reader["tooltipID"];
            showTip = "" + reader["showTip"];

            if (showTip.Equals("True"))
                client2.sw.WriteLine(tooltip);



            Console.WriteLine("Game between " + player1.playerClient.clientName
                    + " and " + player2.playerClient.clientName + " has started.");

            //Console.WriteLine(" It is {0} 's turn", client1.clientName);
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
                    String command = packetQueue.Peek();
                    string[] message = command.Split(new string[] { "\\" }, StringSplitOptions.None);
                    string log = "NETWORK LOG:";
                    foreach (string s in message)
                    {
                        log += s + "\\";
                    }

                    Console.WriteLine(log);

                    if (message[0].Equals("dontDisplayTip"))
                    {
                        dm.updateTooltip(currentPlayer.playerClient.clientName, Int32.Parse(message[1]));
                        packetQueue.Dequeue();
                    }
                    else if (message[0].Equals("surrender"))
                    {
                        if (isAI_Game)
                        {
                            player1.playerClient.sw.WriteLine("defeat");
                        }
                        else
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
                        }

                        player1.playerClient.sw.WriteLine("enableMatchmaking");
                        player2.playerClient.sw.WriteLine("enableMatchmaking");
                        gameOver = true;
                        player1.playerClient.inGame = false;
                        player2.playerClient.inGame = false;
                    }
                    else if (currentPlayer.isMyTurn)
                    {
                        packetQueue.Dequeue();
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

                                if (currentPlayer.playerAllegiance == movingUnit.unitAllegiance)
                                {
                                    if (currentPlayer.mana >= movingUnit.MovementCost)
                                    {
                                        if (!movingUnit.Paralyzed)
                                        {
                                            if (!movingUnit.Moved)
                                            {
                                                if (movingUnit.accessibleTiles.Contains(destinationTile))
                                                {
                                                    currentPlayer.mana -= movingUnit.MovementCost;

                                                    Console.WriteLine("LOG: moving unit from tile (" + startTile.x + "," + startTile.y + ") to tile(" + destinationTile.x + "," + destinationTile.y + ")");
                                                    movingUnit.moveUnit(destinationTile);
                                                    //Tell the clients to move the unit
                                                    if (!isAI_Game) { player2.playerClient.sw.WriteLine("move\\" + message[1] + "\\" + destinationTile.x + "\\" + destinationTile.y); }
                                                    player1.playerClient.sw.WriteLine("move\\" + message[1] + "\\" + destinationTile.x + "\\" + destinationTile.y);
                                                }
                                                else
                                                {
                                                    Console.WriteLine("LOG: Tile is not in range.");

                                                    if (destinationTile.CurrentUnit == null)
                                                    {
                                                        Console.WriteLine("LOG: WTF");
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("LOG: {0} ", destinationTile.CurrentUnit);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("LOG: Unit has already moved");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("LOG: Unit is paralyzed");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("LOG: Player does not have enough mana.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("LOG: Unit is not of same allegiance.");
                                }

                                movingUnit.accessibleTiles.Clear();
                            }
                            else
                            {
                                Console.WriteLine("LOG: It is not this player's turn");
                            }
                        }


                        else if (message[0].Equals("attack"))
                        {
                            if (currentPlayer.isMyTurn)
                            {
                                Unit attackingUnit = board.getUnitByID(Int32.Parse(message[1]));
                                GameBoard.Tile tileToAttack = board.Tiles[Int32.Parse(message[2]), Int32.Parse(message[3])];
                                attackingUnit.setAttackTiles(attackingUnit.CurrentTile, attackingUnit.AttackRange);

                                if (currentPlayer.playerAllegiance == attackingUnit.unitAllegiance)
                                {
                                    if (!attackingUnit.Paralyzed)
                                    {
                                        if (!attackingUnit.Attacked)
                                        {
                                            if (currentPlayer.mana >= attackingUnit.AttackCost)
                                            {
                                                if (attackingUnit.accessibleTiles.Contains(tileToAttack))
                                                {

                                                    Console.WriteLine("LOG: Unit {0} is attacking tile ({1}, {2})"
                                                                        , attackingUnit.UniqueID
                                                                        , tileToAttack.x
                                                                        , tileToAttack.y);
                                                    currentPlayer.mana -= attackingUnit.AttackCost;

                                                    //get a list of all the unit IDs that were attacked
                                                    //apply damage to all units
                                                    ArrayList unitIDs = attackingUnit.AttackTile(tileToAttack);
                                                    attackingUnit.Attacked = true;

                                                    sendAttackedUnits(unitIDs, attackingUnit, tileToAttack.x, tileToAttack.y);

                                                }


                                                else
                                                {
                                                    Console.WriteLine("LOG: Enemy is not in range.");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("LOG: Player does not have enough mana to attack.");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("LOG: Unit has already attacked");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("LOG: Unit is paralyzed. Cannot Attack.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("LOG: Unit is not of same allegiance.");
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
                                    if (!isAI_Game) { player2.playerClient.sw.WriteLine("victory"); }
                                    gameOver = true;
                                    player1.playerClient.inGame = false;
                                    player2.playerClient.inGame = false;

                                     if (isAI_Game)
                                     {
                                         player1.playerClient.sw.WriteLine("enableMatchmaking");
                                         player2.playerClient.sw.WriteLine("enableMatchmaking");
                                     }

                                }
                                else if (board.player2_Soulstone.Health <= 0)
                                {
                                    player1.playerClient.sw.WriteLine("victory");
                                    if (!isAI_Game) { player2.playerClient.sw.WriteLine("defeat"); }
                                    gameOver = true;
                                    player1.playerClient.inGame = false;
                                    player2.playerClient.inGame = false;

                                    if (isAI_Game)
                                    {
                                        player1.playerClient.sw.WriteLine("enableMatchmaking");
                                        player2.playerClient.sw.WriteLine("enableMatchmaking");
                                    } 
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
                                if (!isAI_Game) { player2.playerClient.sw.WriteLine("switchTurns\\{0}", maxMana); }
                            }
                        }
                    }
                }
            }
        }

        //if the player disconnects midgame, the other player should be declared the winner
        public void surrenderByDisconnect(Server.Client disconnectedClient)
        {
            if (!isAI_Game)
            {
                Server.Client winner = (disconnectedClient == player1.playerClient) ? player2.playerClient : player1.playerClient;
                winner.sw.WriteLine("victory");
                gameOver = true;
                player1.playerClient.inGame = false;
                player2.playerClient.inGame = false;
            }
        }

        public void sendAttackedUnits(ArrayList unitIDs, Unit attackingUnit, int tileToAttackX, int tileToAttackY)
        {

            string sendMessage = "attack\\" + attackingUnit.UniqueID + "\\" + unitIDs.Count;
            foreach (int id in unitIDs)
            {
                Unit unitHit = board.getUnitByID(id);
                attackingUnit.Attack(unitHit);
                sendMessage += ("\\" + id);

                if (id < 500 && !board.getUnitByID(id).IsSoulStone)
                    attackingUnit.addXP();
            }

            sendMessage += "\\" + tileToAttackX + "\\" + tileToAttackY;

            player1.playerClient.sw.WriteLine(sendMessage);
            if (!isAI_Game)
            {

                player2.playerClient.sw.WriteLine(sendMessage);
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
            public Queue<string> commandQueue;

            public Player(Server.Client playerClient)
            {
                mana = 2;
                maxMana = 2;
                this.playerClient = playerClient;
            }

            public void IncreaseMana()
            {
                if (maxMana < 8)
                    maxMana += 2;

                mana = maxMana;
            }
        }
    }
}
