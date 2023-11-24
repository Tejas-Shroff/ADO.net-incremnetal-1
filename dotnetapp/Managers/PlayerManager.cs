using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dotnetapp.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


// namespace dotnetapp.Managers
// {
//     public class PlayerManager
//     {
//         // Write your fuctions here...
//         // DisplayAllPlayers
//         // AddPlayer
//         // EditPlayer
//         // DeletePlayer
//         // ListPlayers
//         // FindPlayer
//         // DisplayAllPlayers


//     }
// }

namespace dotnetapp.Managers
{
    public class PlayerManager
    {
 
        // Domain Driven
 
        List<Player> players = new List<Player>();
        int pCount = 0;
 
        public void AddPlayer()
        {
            string playerName = Console.ReadLine();
            int id = int.Parse(Console.ReadLine());
            int age = int.Parse(Console.ReadLine());
            string categry = Console.ReadLine();
            decimal biddingPrice = decimal.Parse(Console.ReadLine());
 
            Player p = new Player{Name = playerName,Id = pCount++,Age = age,category = categry,BiddingPrice = biddingPrice,};
            players.Add(p);
 
        }
 
        public void FindPlayer()
        {
            int playerid = int.Parse(Console.ReadLine());
            Player fplayer = players.Find(p => p.Id == playerid);
            if(fplayer != null)
            {
                Console.WriteLine($"Name:{fplayer.Name} \nAge:{fplayer.Age} \nCategory: {fplayer.category} \nBidding Price: {fplayer.BiddingPrice}");
            }
        }
 
        public void EditPlayer(int id, string newName, int age, string cat, decimal bprice)
        {  
            Player p = players.Find(p=>p.Id == id);
            if(p != null)
            {
                p.Name = newName;
                p.Age = age;
                p.category = cat;
                p.BiddingPrice = bprice;
            }  
            else
            {
                Console.WriteLine("Player not Found");
            }
        }
 
        //Connected Architecture
 
        string connectionString = "User ID=sa;password=examlyMssql@123; server=localhost;Database=Players;trusted_connection=false;Persist Security Info=False;Encrypt=False";
        public void ListPlayers()
        {
           
            string cmdtext = "select * from Players";
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                Console.WriteLine("Connection Opened Successfully");
                SqlCommand command = new SqlCommand(cmdtext, con);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader["Id"]} --- {reader["Name"]} --- {reader["Age"]} --- {reader["Category"]} --- {reader["BiddingPrice"]}");
                }
                con.Close();
            }
 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
 
        }
 
        public void DeletePlayer(int id)
        {  
            string cmdtext = "delete from Players where Id = @Id";
 
            try
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                Console.WriteLine("Connection Opened Successfully");
                SqlCommand command = new SqlCommand(cmdtext, con);
                command.Parameters.AddWithValue("@Id",id);
                int playerEffected = command.ExecuteNonQuery();
                if(playerEffected > 0)
                {
                    Console.WriteLine("Player Deleted");
                }
                else
                {
                    Console.WriteLine("Player Not Found");
                }
                con.Close();
            }
 
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
       
 
        //Disconnected Architecture
 
        void DisplayPlayersfromDB()
        {
            string cmdtext = "select * from Players";
            SqlConnection connection = null;
            SqlDataAdapter adapter = null;
            DataSet ds = null;
            DataTable playerTable = null;
            try
            {
                ds = new DataSet();
                connection = new SqlConnection(connectionString);
                adapter = new SqlDataAdapter("select * from Player", connection); //only select commands can be given in adapter
                adapter.Fill(ds,"Players");
                playerTable = ds.Tables["Players"];
                Console.WriteLine($"Rows = {playerTable.Rows.Count}");
                Console.WriteLine($"Columns = {playerTable.Columns.Count}");
                foreach(DataRow row in playerTable.Rows)
                {
                    Console.WriteLine($"{row["id"]} -- {row["Name"]} -- {row["Age"]} -- {row["Category"]} -- {row["Bidding Price"]}");
                }
 
            }
            catch (Exception ex)
            {
               
                Console.WriteLine(ex.Message);
            }
 
            return;
        }
        public void AddPlayerToDatabase(Player p)
        {
            string cmdtext = "insert into Players(Name,Age,category,BiddingPrice) values (@name,@age,@category,@bprice)";
            string name = Console.ReadLine();
            int age = int.Parse(Console.ReadLine());
            string c = Console.ReadLine();
            decimal bprice = decimal.Parse(Console.ReadLine());
            SqlConnection connection = null;
            SqlDataAdapter adapter = null;
            DataSet ds = null;
            DataTable playerTable = null;
            SqlConnection con = new SqlConnection(connectionString);
 
            try
            {
                ds = new DataSet();
                connection = new SqlConnection(connectionString);
                adapter = new SqlDataAdapter("select * from Players",connection);
                adapter.Fill(ds,"Player");
                playerTable = ds.Tables["Player"];
                DataRow playerrow = playerTable.NewRow();
                playerrow["Name"] = name;
                playerrow["Age"] = age;
                playerrow["category"] = c;
                playerrow["BiddingPrice"] = bprice;
                playerTable.Rows.Add(playerrow );
                SqlCommandBuilder scb = new SqlCommandBuilder(adapter);
                Console.WriteLine(scb.GetInsertCommand().CommandText);
                adapter.Update(playerTable);
                Console.WriteLine("Row Added");
            }
            catch (Exception ex)
            {
       
                Console.WriteLine(ex.Message);
            }
            finally{
                con.Close();
            }
 
            return;
        }
 
        public void EditPlayerinDB(int playerid,string new_name, int new_age, string new_category, decimal new_biddingPrice)
        {
            string cmdtext = "update Players set Name = @new_name, Age = @new_age, Category = @new_category,BiddingPrice = @new_biddingPrice where Id = @playerid)";
            SqlConnection connection = null;
            SqlDataAdapter adapter = null;
            DataSet ds = null;
            DataTable playerTable = null;
 
            try
            {
                ds = new DataSet();
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(cmdtext,connection);
                adapter = new SqlDataAdapter("select * from Player", connection); //only select commands can be given in adapter
                adapter.Fill(ds,"Players");
                command.Parameters.AddWithValue("@new_name", new_name);
                command.Parameters.AddWithValue("@new_age", new_age);
                command.Parameters.AddWithValue("@new_category", new_category);
                command.Parameters.AddWithValue("@new_bddingPrice", new_biddingPrice);
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
 
                if(rowsAffected>0)
                {
                    Console.WriteLine("Player Updated");
                }
                else{
                    Console.WriteLine("No Player Found");
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
           
 
            return;
        }
 
        public void DeletePlayerfromDB(int playerId)
        {
            string cmdtext = "delete from Players where Id = @playerId)";
            SqlConnection connection = null;https://github.com/login/device
            SqlDataAdapter adapter = null;
            DataSet ds = null;
            DataTable playerTable = null;  
 
            try
            {
                ds = new DataSet();
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(cmdtext,connection);
                adapter = new SqlDataAdapter("select * from Player", connection); //only select commands can be given in adapter
                adapter.Fill(ds,"Players");
                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
 
                if(rowsAffected > 0)
                {
                    Console.WriteLine("Player Deleted");
                }
                else
                {
                    Console.WriteLine("Player Not Found");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
 
    }
}
