using System;
//using System.Drawing;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
//using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Data.SQLite;

namespace Guardians_Of_The_Arena_Server
{
    public class Server
    {
        public static DataManager dm;

        protected static int numberOfClients;
        protected static int clientNumbers;

        protected static Random rng;

        protected static TcpListener listener;
        protected static ArrayList socArray;
        protected static LinkedList<Client> clientArray;
        protected static ArrayList clientsToRemove;
        protected static List<String> loginNames;

        public Thread listenerThread;
        public Thread loopThread;

        private Object listLock = new Object();

        private MatchMakingQueue<Client> matchqueue;

        public Server()
        {
            dm = new DataManager();
            numberOfClients = 0;

            loginNames = new List<string>();

            rng = new Random();
            listener = new TcpListener(4188);
            socArray = new ArrayList();
            clientArray = new LinkedList<Client>();
            clientsToRemove = new ArrayList();

            listenerThread = new Thread(new ThreadStart(this.Listen));
            loopThread = new Thread(new ThreadStart(this.loop));

            matchqueue = new MatchMakingQueue<Client>();

        }

        public void Listen()
        {
            listener.Start();
            //loopThread.Start();
            Console.WriteLine("Waiting for clients to connect");

            while (true)
            {

                //accept an incoming request to make a connection then return 
                //the socket(endpoint) associate with the connection 
                Socket soc = listener.AcceptSocket();
                socArray.Add(soc);

                lock (listLock)
                {    

                    Console.WriteLine("Connected: {0}", soc.RemoteEndPoint);//print connection details

                    try
                    {
                        NetworkStream nws = new NetworkStream(soc);
                        StreamReader sr = new StreamReader(nws);//return the stream to read from
                        StreamWriter sw = new StreamWriter(nws); //establish a stream to read to
                        sw.AutoFlush = true;//enable automatic flushing, flush thte wrtie stream after every write command, no need to send buffered data
                        
                        numberOfClients++;

                        Client client = new Client(nws, sr, sw, ++clientNumbers, soc);
                        client.thread.Start();

                        clientArray.AddLast(client);

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

        }

        //check if the socket is still connect;
        bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 && part2)
            {
                Console.WriteLine("NETWORK LOG: Lost connection from socket {0}", s.ToString());
                return false;           
            }
            else
            {
                return true;
            }
        }


        //if a client is disconnected in anyway
        //we must add that client number to the list of available numbers and remove the pointer to this client
        //and inform all other clients that this client has disconnected
        public void RemoveClient(Client client)
        {
            clientsToRemove.Add(client);
            client.thread.Abort();
            numberOfClients--;

            if (loginNames.Contains(client.clientName))
                loginNames.Remove(client.clientName);
            //dm.deleteFromHighScores(client.clientName);

            foreach (Client c in clientArray)
            {
                if (SocketConnected(c.socket))
                    c.sw.WriteLine("hasLoggedOut\\" + client.clientNumber);
            }

            Console.WriteLine("Client " + client.clientNumber + " has disconnected");
            Console.WriteLine("There are now " + numberOfClients + " clients");
        }

        //server loop that checks messages from clients
        public void loop()
        {
            //always run this loop
            while (true)
            {
                try
                {
                    lock (listLock)
                    {

                        //If players are available to start playing, we will send them a message to start playing
                        if (matchqueue.Count >= 2)
                        {
                            Client client1 = matchqueue.dequeue();
                            Client client2 = matchqueue.dequeue();

                            Game newGame = new Game(client1, client2, dm);
                            client1.gameRef = newGame;
                            client2.gameRef = newGame;
                            client1.inGame = true;
                            client2.inGame = true;

                            newGame.GameThread.Start();
                        }

                        //Console.WriteLine("Looping");

                        foreach (Client client in clientArray)
                        {
                            if (!SocketConnected(client.socket))
                                RemoveClient(client);

                            //the server must check if it has recieved any commands from the clients
                            if (client.commandQueue.Count > 0 && !client.inGame)
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
                                    */

                                if (tokens[0].Equals("search"))
                                {
                                    client.boardSetup = Int32.Parse(tokens[1]);
                                    matchqueue.enqueue(client);
                                }
                                else if (tokens[0].Equals("cancelSearch"))
                                {
                                    matchqueue.removeFromQueue(client);
                                }
                                else if (tokens[0].Equals("globalChat"))
                                {
                                    foreach (Client c in clientArray)
                                    {
                                        c.sw.WriteLine("globalChat\\" + client.clientName + "\\" + tokens[1]);                                       
                                    }

                                    Console.WriteLine("CHAT: ( {0} ) {1}", client.clientName, tokens[1]);
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
                                                loginNames.Add(tokens[1]);
                                                client.clientName = tokens[1];
                                                Console.WriteLine(tokens[1] + " has logged in");
                                                //dm.printTable();


                                                foreach (Client clientToSend in clientArray)
                                                {
                                                    //Client clientToSend = clientArray[j];

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
                                                RemoveClient(client);
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
                                            //dm.printTable();

                                            //-------------------------REMOVE COPY AND PASTED CODE! CLEAN UP! ASAP! --------------------------------------------------//

                                            foreach (Client clientToSend in clientArray)
                                            {
                                                //Client clientToSend = clientArray[j];

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
                                        RemoveClient(client);
                                    }

                                }

                                else if (tokens[0].Equals("logout"))
                                {
                                    Console.WriteLine("Client " + client.clientNumber + " wants to log out.");
                                    RemoveClient(client);
                                    //Console.WriteLine("Name about to be removed {0}", tokens[1]);
                                    //loginNames.Remove(tokens[1]);
                                    //dm.deleteFromHighScores(client.clientName);
                                    //dm.sendPacket("remove", client.clientName, client.score);

                                }
                                else if (tokens[0].Equals("getBoardData"))
                                {
                                   SQLiteDataReader reader = dm.getBoardSetup(client.clientName, Int32.Parse(tokens[1]));

                                   string setupString = "boardSetup";

                                   while (reader.Read())
                                   {
                                       setupString += ("\\" + reader["unitType"] + "\\" + reader["x"] + "\\" + reader["y"] + "\\" + reader["onField"]);
                                   }

                                   client.sw.WriteLine(setupString);
                                }
                                else if (tokens[0].Equals("movePiece"))
                                {
                                    int onField = 0;

                                    if (tokens[8].Equals("True"))
                                        onField = 1;

                                    dm.updateSetup(tokens[1]
                                        , Int32.Parse(tokens[2])
                                        , Int32.Parse(tokens[3])
                                        , Int32.Parse(tokens[4])
                                        , Int32.Parse(tokens[5])
                                        , Int32.Parse(tokens[6])
                                        , Int32.Parse(tokens[7])
                                        , onField);
                                }

                                else
                                {
                                    //do nothing
                                }

                            }
                        }

                        foreach(Client c in clientsToRemove)
                        {
                            clientArray.Remove(c);
                        }

                        clientsToRemove.Clear();
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                    Console.WriteLine(e.Message);
                }
            }
        }

    

        //client class that defines all the attributes 
        public class Client
        {
            public NetworkStream nws;
            public StreamReader sr;
            public StreamWriter sw;
            public Socket socket;
            public Queue<string> commandQueue;
            public Thread thread;

            public int clientNumber;
            public string clientName = "";

            public bool clientReady = false;
            public bool spawned = false;
            public bool inGame = false;

            public Game gameRef;
            public int boardSetup = 1;

            //each client must keep track of the network stream between itself and the server
            //the stream reader used by the server to read in client messages
            //the stream writer used by the server to send the client messages
            //an assigned client number
            public Client(NetworkStream nws, StreamReader sr, StreamWriter sw, int clientNumber, Socket socket)
            {
                this.nws = nws;
                this.sr = sr;
                this.sw = sw;
                this.socket = socket;
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
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
