using System;
using System.Data.SQLite;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;
using System.Text;


namespace Guardians_Of_The_Arena_Server
{
    public class DataManager
    {
        public static SQLiteConnection userDatabase;
        IPEndPoint RemoteEndPoint;
        Socket s;



        public DataManager()
        {
            RemoteEndPoint = new IPEndPoint(IPAddress.Any, 9000);
            s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


            if (!File.Exists("GOA_User_Database.sqlite"))
            {
                createDatabase();
            }

            connectToDatabase();
            
            clearTable();
            //fillPlayerTable();
            //printTable();
            //dropTable();
            //createTables();

        }

        // Creates an empty database

        void createDatabase()
        {
            try
            {

               SQLiteConnection.CreateFile("GOA_User_Database.sqlite");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // Creates a connection with our database

        void connectToDatabase()
        {
            userDatabase = new SQLiteConnection("Data Source=GOA_User_Database.db;Version=3;");
            userDatabase.Open();
        }

        // Create two tables

        // a table named 'playerInfo' with two columns: nae (a string of max 20 characters) and password(a string of max 50 characters)
        // a table named 'highscores' with two columns: name (a string of max 20 characters) and score (an int)

        void createTables()
        {

            string sql = "create table playerInfo (name varchar(20), password varchar(50))";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            sql = "create table highScores (name varchar(20), score int)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE unitSetups(name VARCHAR(20), setupName VARCHAR(50), setupID TINYINT, unitType TINYINT, x TINYINT, y TINYINT, onField BIT)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE tooltips(name VARCHAR(20), tooltipID TINYINT, showTip BIT)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

     
        }

        // Clear the tables

        public void clearTable()
        {
            string sql = "DELETE FROM playerInfo";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            SQLiteDataReader reader = command.ExecuteReader();

            sql = "DELETE FROM highScores";
            command = new SQLiteCommand(sql, userDatabase);
            reader = command.ExecuteReader();

            sql = "DELETE FROM unitSetups";
            command = new SQLiteCommand(sql, userDatabase);
            reader = command.ExecuteReader();

            sql = "DELETE FROM tooltips";
            command = new SQLiteCommand(sql, userDatabase);
            reader = command.ExecuteReader();
        }

        public void dropTable()
        {

            string sql = "DROP TABLE unitSetups; DROP TABLE playerInfo; DROP TABLE highScores; DROP TABLE tooltips;";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            SQLiteDataReader reader = command.ExecuteReader();
            
        }

        // print the tables to the console

        public void printTable()
        {
            string sql = "select * from playerInfo";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tpassword: " + reader["password"]);
        }

        // check to see if a value exist in the table

        public bool existsInTable(string name)
        {
            string sql = "SELECT count(*) FROM playerInfo WHERE name=:Name";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.Parameters.AddWithValue(":Name", name);
            int count = Convert.ToInt32(command.ExecuteScalar());

            return (count != 0);
        }

        // get the user's password from the playerInfo database

        public string getUserPassword(string name)
        {
            string sql = "select password from playerInfo where name=" + "'" + name + "'";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            SQLiteDataReader reader = command.ExecuteReader();
            return reader["password"].ToString();
        }




        public void getTableEntry(string name)
        {
            string sql = "select * from playerInfo where name=" + "'" + name + "'";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            SQLiteDataReader reader = command.ExecuteReader();
            Console.WriteLine(reader["name"] + " : " + reader["password"]);

        }

        // insert a player into the playerInfo table

        public void insertIntoPlayer(string name, string password)
        {
            string sql = "select * from playerInfo where name =:name";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.Parameters.AddWithValue(":name", name);
            SQLiteDataReader dataReader = command.ExecuteReader();

            // if the name is in the database

            if (dataReader.Read())
            {
                Console.WriteLine("found: " + name);
                if (password.Equals(dataReader["password"]))
                {
                    Console.WriteLine("you have entered the correct password");
                }
                else
                {
                    Console.WriteLine("Invalid passowrd");
                }
            }
            else
            {
                sql = "insert into playerInfo (name, password) values(@param1, @param2)";
                command = new SQLiteCommand(sql, userDatabase);
                command.Parameters.Add(new SQLiteParameter("@param1", name));
                command.Parameters.Add(new SQLiteParameter("@param2", password));
                command.ExecuteNonQuery();

               createDefaultUnitSetup(name);

            }

        }

        public void insertIntoHighScores(string name, int score)
        {
            string sql = "insert into highScores (name, score) values(@param1, @param2)";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.Parameters.Add(new SQLiteParameter("@param1", name));
            command.Parameters.Add(new SQLiteParameter("@param2", score));
            command.ExecuteNonQuery();
        }

        public void deleteFromHighScores(string name)
        {
            string sql = "delete from highScores WHERE name='" + name + "'";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();
        }

        public void printHighTable()
        {

            string sql = "select * from highScores";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tScore: " + reader["score"]);
        }

        public void updateHighScores(string name, int score)
        {
            string sql = "UPDATE highScores SET score=@newScore WHERE name=@name";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@newScore", score);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception fail)
            {
                Console.WriteLine(fail.Message);
            }
            // if the name is in the database
        }


        // Inserts some values in the highscores table.
        // As you can see, there is quite some duplicate code here, we'll solve this in part two.

        void fillPlayerTable()
        {
            string sql = "insert into playerInfo  (name, password) values ('Jay', '39ec785d60a1b23bfda9944b9138bbcf')";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();
            sql = "insert into playerInfo  (name, password) values ('Me', 3232 )";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();
            sql = "insert into playerInfo  (name, password) values ('Not me', 30011)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();
        }

        public void createDefaultUnitSetup(string name)
        {
            string sql = "";
            int pageNumber = -1;
            string pageName = "";

            sql =  "INSERT INTO tooltips (name, tooltipID, showTip) VALUES ('" + name +"' , 1, 1);";
            sql += "INSERT INTO tooltips (name, tooltipID, showTip) VALUES ('" + name + "' , 2, 1);";
            sql += "INSERT INTO tooltips (name, tooltipID, showTip) VALUES('" + name + "' , 3, 1);";

            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            ///////////////////PAGE 1/////////////////////
            pageNumber = 1;
            pageName = "Siege";
            sql = "";
            ////////////// ON BOARD UNITS ////////////////
            //unit type 2 at 2, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ", 2, 2, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 3 at 2, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 2, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();
           
            //unit type 3 at 3, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 3, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();                

            //unit type 3 at 4, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 4, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 10 at 1, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," +  pageNumber + ",10 , 1, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();
               
            //unit type 11 at 2, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",11 , 2, 0, 1)";
            command = new SQLiteCommand(sql, userDatabase);

            command.ExecuteNonQuery();
            //unit type 1 at 3, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 3, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);

            command.ExecuteNonQuery();
            //unit type 1 at 4, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 4, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //////////////////////// OFF BOARD PIECES///////////////////////
            //unit type 7 at 0, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 0, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //pieces off the board
            //unit type 7 at 1, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 1, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 2, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 2, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 3, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 3, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 8 at 4, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 8, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 8 at 5, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",8 , 8, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            ///////////////////PAGE 2/////////////////////
            pageNumber = 2;
            pageName = "Frontline";
            sql = "";
            ////////////// ON BOARD UNITS ////////////////
            //unit type 7 at 1, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ", 7, 1, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 3 at 2, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 2, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 3, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 3, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 5, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 5, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 10 at 4, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",10 , 4, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 11 at 4, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",11 , 4, 0, 1)";
            command = new SQLiteCommand(sql, userDatabase);

            command.ExecuteNonQuery();
            //unit type 3 at 6, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 6, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);

            command.ExecuteNonQuery();
            //unit type 7 at 7, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 7, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //////////////////////// OFF BOARD PIECES///////////////////////
            //unit type 1 at 0, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 0, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //pieces off the board
            //unit type 1 at 1, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 1, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 2 at 2, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",2 , 2, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 3 at 3, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 3, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 8 at 4, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",8 , 4, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 8 at 5, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",8 , 5, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            ///////////////////PAGE 3/////////////////////
            pageNumber = 3;
            pageName = "Aggro";
            sql = "";
            ////////////// ON BOARD UNITS ////////////////
            //unit type 7 at 2, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ", 7, 2, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 3 at 3, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 3, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 10 at 4, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",10 , 4, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 3 at 5, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 5, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 6, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 6, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 1 at 3, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 3, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);

            command.ExecuteNonQuery();
            //unit type 1 at 5, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 5, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);

            command.ExecuteNonQuery();
            //unit type 11 at 4, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",11 , 4, 0, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //////////////////////// OFF BOARD PIECES///////////////////////
            //unit type 2 at 0, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",2 , 0, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //pieces off the board
            //unit type 3 at 1, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 1, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 2, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 2, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 3, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 3, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 8 at 4, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",8 , 4, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 8 at 5, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",8 , 5, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            ///////////////////PAGE 4/////////////////////
            pageNumber = 4;
            pageName = "Midrange";
            sql = "";
            ////////////// ON BOARD UNITS ////////////////
            //unit type 3 at 3, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ", 3, 3, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 4, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 4, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 3 at 5, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 5, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 10 at 4, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",10 , 4, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 2 at 5, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",2 , 5, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 1 at 6, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 6, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);

            command.ExecuteNonQuery();
            //unit type 8 at 5, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",8 , 5, 0, 1)";
            command = new SQLiteCommand(sql, userDatabase);

            command.ExecuteNonQuery();
            //unit type 11 at 4, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",11 , 4, 0, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //////////////////////// OFF BOARD PIECES///////////////////////
            //unit type 1 at 0, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 0, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //pieces off the board
            //unit type 3 at 1, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 1, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 2, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 2, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 3, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 3, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 4, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 4, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 8 at 5, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",8 , 5, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            ///////////////////PAGE 5/////////////////////
            pageNumber = 5;
            pageName = "Turtle";
            sql = "";
            ////////////// ON BOARD UNITS ////////////////
            //unit type 10 at 0, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ", 10, 0, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 3 at 1, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 1, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 3 at 2, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 2, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 3, 2
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 3, 2, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 8 at 0, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",8 , 0, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 8 at 1, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",8 , 1, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);

            command.ExecuteNonQuery();
            //unit type 1 at 2, 1
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 2, 1, 1)";
            command = new SQLiteCommand(sql, userDatabase);

            command.ExecuteNonQuery();
            //unit type 11 at 1, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",11 , 1, 0, 1)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //////////////////////// OFF BOARD PIECES///////////////////////
            //unit type 1 at 0, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",1 , 0, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //pieces off the board
            //unit type 2 at 1, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",2 , 1, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 3 at 2, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",3 , 2, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 3, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 3, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 4, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 4, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();

            //unit type 7 at 5, 0
            sql = "   INSERT INTO unitSetups  ";
            sql += "   (name, setupName, setupID, unitType, x , y, onField)    ";
            sql += "   VALUES  ";
            sql += "   ('" + name + "', '" + pageName + "'," + pageNumber + ",7 , 5, 0, 0)";
            command = new SQLiteCommand(sql, userDatabase);
            command.ExecuteNonQuery();



        }

        public SQLiteDataReader getBoardSetup(string name, int setupID)
        {
            string sql =    "SELECT setupName, unitType, x, y, onField ";
            sql +=          "FROM unitSetups ";
            sql +=          "WHERE name = @name AND setupID = @setupID";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@setupID", setupID);
            return command.ExecuteReader();

            
        }

        public SQLiteDataReader getSetupNames(string name)
        {
            string sql = "SELECT DISTINCT setupName FROM unitSetups WHERE name = @name ORDER BY setupID";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.Parameters.AddWithValue("@name", name);
            return command.ExecuteReader();
        }

        public void updateSetupName(string playerName, int setupID, string setupName)
        {
            string sql = "UPDATE unitSetups SET setupName = @setupName WHERE name = @name AND setupID = @setupID ";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.Parameters.AddWithValue("@name", playerName);
            command.Parameters.AddWithValue("@setupID", setupID);
            command.Parameters.AddWithValue("@setupName", setupName);
            command.ExecuteNonQuery();
        }

        public SQLiteDataReader getGameSetup(string name, int setupID)
        {
            string sql = "SELECT unitType, x, y ";
            sql += "FROM unitSetups ";
            sql += "WHERE name = @name AND setupID = @setupID AND onField = 1";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@setupID", setupID);
            return command.ExecuteReader();
            
        }

         public void updateSetup(string name, int setupID, int unitType, int oldX, int oldY
                                , int newX, int newY, int oldOnField, int newOnField)
        {
            
            String sql   =     "UPDATE unitSetups ";
            sql         += "SET x = @newX, y = @newY, onField = @newOnField ";
            sql         += "WHERE name = @name AND setupID = @setupID AND x = @oldX AND y = @oldY AND onField = @oldOnField AND unitType = @unitType";
            SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@newX", newX);
            command.Parameters.AddWithValue("@newY", newY);
            command.Parameters.AddWithValue("@unitType", unitType);
            command.Parameters.AddWithValue("@oldX", oldX);
            command.Parameters.AddWithValue("@oldY", oldY);
            command.Parameters.AddWithValue("@oldOnField", oldOnField);
            command.Parameters.AddWithValue("@newOnField", newOnField);
            command.Parameters.AddWithValue("@setupID", setupID);
            command.ExecuteNonQuery();

            //sql = "SELECT * FROM unitSetups";
            //command = new SQLiteCommand(sql, userDatabase);
            //SQLiteDataReader reader = command.ExecuteReader();

            //while (reader.Read())
            //    Console.WriteLine(reader["setupID"] + " " + reader["unitType"] + " " + reader["x"] + " " + reader["y"]);
        }

         public SQLiteDataReader getTooltips(string name, int tooltipID)
         {
             string sql = "SELECT tooltipID, showTip FROM tooltips WHERE name = @name AND tooltipID = @tooltipID";
             SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
             command.Parameters.AddWithValue("@name", name);
             command.Parameters.AddWithValue("@tooltipID", tooltipID);
             return command.ExecuteReader();
         }

         public void updateTooltip(string name, int tooltipID)
         {
             string sql = "UPDATE tooltips SET showTip = 0 WHERE name = @name AND tooltipID = @ID;";
             SQLiteCommand command = new SQLiteCommand(sql, userDatabase);
             command.Parameters.AddWithValue("@name", name);
             command.Parameters.AddWithValue("@ID", tooltipID);
             command.ExecuteNonQuery();
         }

        public void sendPacket(string action, string name, int score)
        {
            Byte[] data = Encoding.ASCII.GetBytes(string.Format("{0}/{1}/{2}", action, name, score));
            s.SendTo(data, data.Length, SocketFlags.None, RemoteEndPoint);
        }


    }
}

