using System;
//using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Data.SQLite;

namespace Guardians_Of_The_Arena_Server
{
    class Server
    {
        public static DataManager dm;

        protected static int numberOfClients;

        protected static Random rng;

        protected static TcpListener listener;

        protected static Socket[] socArray;
        protected static Client[] clientArray;
        protected static List<String> loginNames;


        protected static Stopwatch uniClock;
        protected static Stopwatch commandClock;

        protected static float TopBorderPosition;
        protected static float BottomBorderPosition;
        protected static float RightBorderPosition;
        protected static float LeftBorderPosition;

        protected static bool playing = false;

        protected ServerLoop loop;

        public Thread listenerThead;


        public Server()
        {

            dm = new DataManager();
            numberOfClients = 0;

            loginNames = new List<string>();
 

            TopBorderPosition = RightBorderPosition = 5.0f;
            BottomBorderPosition = LeftBorderPosition = -5.0f;

            rng = new Random();
            listener = new TcpListener(4188);
            socArray = new Socket[5];
            clientArray = new Client[5];

            listenerThead = new Thread(new ThreadStart(this.Listen));

            uniClock = new Stopwatch();
            commandClock = new Stopwatch();

            loop = new ServerLoop(this);
            loop.loopThread.Start();

        }



        public void Listen()
        {
            listener.Start();

            while (true)
            {

                //accept an incoming request to make a connection then return 
                //the socket(endpoint) associate with the connection 
                Socket soc = listener.AcceptSocket();
                socArray[numberOfClients] = soc;


                Console.WriteLine("Connected: {0}", soc.RemoteEndPoint);//print connection details
                try
                {
                    NetworkStream nws = new NetworkStream(soc);
                    StreamReader sr = new StreamReader(nws);//return the stream to read from
                    StreamWriter sw = new StreamWriter(nws); //establish a stream to read to
                    sw.AutoFlush = true;//enable automatic flushing, flush thte wrtie stream after every write command, no need to send buffered data

                    int clientNumber = numberOfClients;

                    Client client = new Client(this, nws, sr, sw, clientNumber);
                    client.thread.Start();

                    clientArray[numberOfClients++] = client ;

                    //loop.playerConnected();

                    Console.WriteLine("Client " + client.clientNumber + " has connected");
                    Console.WriteLine("There are now " + numberOfClients + " clients");
                    client.sw.WriteLine("client\\" + client.clientNumber);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
            }

        }


        //if a client is disconnected in anyway
        //we must add that client number to the list of available numbers and remove the pointer to this client
        //and inform all other clients that this client has disconnected
        public void RemoveClient(Client client)
        {
            client.thread.Abort();
            numberOfClients--;


            Console.WriteLine("Name about to be removed {0}", client.clientName);
            loginNames.Remove(client.clientName);
            //dm.deleteFromHighScores(client.clientName);

            foreach (Client c in clientArray)
            {
                c.sw.WriteLine("disconnected\\" + client.clientNumber);
            }

            Console.WriteLine("There are now " + numberOfClients + " clients");
        }

        public class ServerLoop
        {
            private Server serverRef;
            public Thread loopThread;

            public ServerLoop(Server serverRef)
            {
                this.serverRef = serverRef;
                loopThread = new Thread(new ThreadStart(this.loop));
                uniClock.Start();
            }

            //recursively check if all clients are ready
            //public bool ClientsReady(LinkedList<Client> clients, LinkedListNode<Client> currentClient)
            //{


            //    if (currentClient.Next == null)
            //        return currentClient.Value.clientReady;
            //    else
            //        return currentClient.Value.clientReady && ClientsReady(clientArray, currentClient.Next);
            //}


            //if the game has already started we do not need to check if the clients are ready
            //simply tell the client that has just connected the game info
            public void loadGameState(Client client)
            {
                
            }

            //server loop that checks messages from clients
            public void loop()
            {

                //always run this loop
                while (true)
                {
                    try
                    {
                        //if all connected clients are read, we can start the game
                        //if (numberOfClients > 1 && ClientsReady(clientArray, clientArray.First))
                        //{
                        //    //create all the pellets on the server

                        //    foreach (Client c in clientArray)
                        //    {
                        //        c.clientReady = false;

                        //        //let all the clients know about each other
                        //        foreach (Client c2 in clientArray)
                        //        {
                        //            c2.sw.WriteLine("connected\\" + c.clientNumber);

                        //        }

                        //        //now that all the clients have the initial game state
                        //        //each client can start playing
                        //        c.sw.WriteLine("start");
                        //    }

                        //    //now that the players have started playing
                        //    //the server should now start checking if any collisions have occured 
                        //    //collisionThread.Start();
                        //    commandClock.Start();
                        //    playing = true;
                        //}

                        //check each client in the array
                        //if they have commands waiting in the queue, dequeue and execute that command



                        for (int i = 0; i < numberOfClients; i++)
                        {
                            Client client = clientArray[i];

                            if (client == null)
                                Console.WriteLine("CLIENT " + i + " IS NULL BECAUSE OF THREADING");

                            //the server must check if it has recieved any commands from the clients
                            if (client.commandQueue.Count > 0)
                            {
                                //each command is a string that will be delimited by \\
                                string command = client.commandQueue.Dequeue();

                                //each message will be broken into an array of strings
                                string[] tokens = command.Split(new string[] { "\\" }, StringSplitOptions.None);

                                /*
                                 *the commands are as follows
                                 *play - tells the server that the client is ready to start playing the game
                                 *userInfo - tells the server that the client is sending login information
                                 *      -check sthe database and either logins the clients, creates new user info,
                                 *       denies the login attemps
                                 *velocity - broadcasts the sending client's updated velocity
                                 *position - broadcasts the sending client's updated position
                                 *score - broadcasts the correct scores to each client
                                 *lag - adds a delay when sending messages between client and server (for testing purposes)
                                 */

                                if (tokens[0].Equals("play"))
                                {
                                    if (playing)
                                    {
                                        loadGameState(client);
                                    }
                                    else
                                    {
                                        client.clientReady = true;
                                        Console.WriteLine("client " + client.clientNumber + " is ready");
                                    }
                                }


                                //
                                else if (tokens[0].Equals("userInfo"))
                                {

                                    if (!loginNames.Contains(tokens[1]))
                                    {

                                        if (dm.existsInTable(tokens[1]))
                                        {
                                            if (tokens[2].Equals(dm.getUserPassword(tokens[1])))
                                            {
                                                client.sw.WriteLine("loginSucceed\\" + tokens[1]);
                                                client.sw.WriteLine("hasLoggedIn\\" + tokens[1]);
                                                client.clientName = tokens[1];
                                                Console.WriteLine(tokens[1] + " has logged in");
                                                dm.printTable();


                                                for (int j = 0; j < numberOfClients; j++)
                                                {
                                                    Client clientToSend = clientArray[j];

                                                    if (client.clientNumber != clientToSend.clientNumber)
                                                    {
                                                        client.sw.WriteLine("hasLoggedIn\\" + clientToSend.clientName);
                                                        clientToSend.sw.WriteLine("hasLoggedIn\\" + client.clientName);
                                                    }
                                                }
                                               
                                            }
                                            else
                                            {
                                                client.sw.WriteLine("loginFail");
                                                Console.WriteLine(tokens[1] + " entered incorrect password");
                                            }
                                        }

                                        else
                                        {
                                            dm.insertIntoPlayer(tokens[1], tokens[2]);
                                            loginNames.Add(tokens[1]);
                                            client.sw.WriteLine("loginSucceed\\" + tokens[1]);
                                            client.sw.WriteLine("hasLoggedIn\\" + tokens[1]);
                                            client.clientName = tokens[1];
                                            // dm.insertIntoHighScores(client.clientName, client.score);
                                            dm.printTable();

    //-------------------------REMOVE COPY AND PASTED CODE! CLEAN UP! ASAP! --------------------------------------------------//

                                            for (int j = 0; j < numberOfClients; j++)
                                            {
                                                Client clientToSend = clientArray[j];

                                                if (client.clientNumber != clientToSend.clientNumber)
                                                {
                                                    client.sw.WriteLine("hasLoggedIn\\" + clientToSend.clientName);
                                                    clientToSend.sw.WriteLine("hasLoggedIn\\" + client.clientName);
                                                }
                                            }
                                        }


                                    }
                                    else
                                    {
                                        client.sw.WriteLine("alreadyLoggedIn\\" + tokens[1]);
                                    }

                                }

                                else if (tokens[0].Equals("logout"))
                                {
                                    serverRef.RemoveClient(client);
                                    //Console.WriteLine("Name about to be removed {0}", tokens[1]);
                                    //loginNames.Remove(tokens[1]);
                                    //dm.deleteFromHighScores(client.clientName);
                                    //dm.sendPacket("remove", client.clientName, client.score);

                                }
                                else if (tokens[0].Equals("disconnect"))
                                {
                                    //dm.sendPacket("remove", client.clientName, client.score);
                                }
                                else
                                {
                                    //do nothing
                                }

                            }
                        }

                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.StackTrace);
                        Console.WriteLine(e.Message);

                    }
                }
            }



        }

        //client class that defines all the attributes 
        public class Client
        {
            private Server serverRef;
            public NetworkStream nws;
            public StreamReader sr;
            public StreamWriter sw;
            public Queue<string> commandQueue;
            public Thread thread;

            public int clientNumber;
            public string clientName;

            public bool clientReady = false;
            public bool spawned = false;

            //each client must keep track of the network stream between itself and the server
            //the stream reader used by the server to read in client messages
            //the stream writer used by the server to send the client messages
            //an assigned client number
            public Client(Server serverRef, NetworkStream nws, StreamReader sr, StreamWriter sw, int clientNumber)
            {
                this.serverRef = serverRef;
                this.nws = nws;
                this.sr = sr;
                this.sw = sw;
                this.clientNumber = clientNumber;

                //each client has a thread that the server uses to alternate reading in messages from
                thread = new Thread(new ThreadStart(this.Service));
                commandQueue = new Queue<string>();
            }


            //we will collisions based on the position of the walls
            //if any of those conditions are true then it is impossible for a collision to have occured




            //checks if the client has sent any messages
            public void Service()
            {

                try
                {
                    while (true)
                    {

                        string data = sr.ReadLine();

                        lock (commandQueue)
                        {
                            commandQueue.Enqueue(data);
                        }
                    }
                }

                catch (Exception e)
                {
                    //Console.WriteLine(e.InnerException);
                    //If the steam closes we will catch the exception and remove the client from the game
                    //properly
                    //Server.RemoveClient(this);
                    //Console.WriteLine(e.StackTrace);
                    serverRef.RemoveClient(this);
                    Console.WriteLine(e.Message + " Client " + this.clientNumber + " has disconnected");

                }
            }
        }
    }
}
