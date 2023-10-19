using OLX.Models.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OLX.DA.User
{
    public class Chat
    {
        string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public void InsertInMap( ChatMappingModel mappingModel/*int Buyerid, int Sellerid, int advertiseid*/)
        {
            
            int Buyerid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            SqlConnection sqlConnection = new SqlConnection(cs);
            string checkQuery = "SELECT COUNT(*) FROM ChatMaping WHERE BuyerId = @buyerid AND SellerId = @sellerid AND advertiseid = @advertiseid";

            SqlCommand checkCmd = new SqlCommand(checkQuery, sqlConnection);
            checkCmd.Parameters.AddWithValue("@buyerid", Buyerid);
            checkCmd.Parameters.AddWithValue("@sellerid", mappingModel.Sellerid);
            checkCmd.Parameters.AddWithValue("@advertiseid", mappingModel.advertiseid);

            sqlConnection.Open();
            int existingRecordCount = (int)checkCmd.ExecuteScalar();
            sqlConnection.Close();


            if (existingRecordCount == 0) {
                string query = "insert into ChatMaping (BuyerId,SellerId,advertiseid)" +
                       " values (@buyerid,@sellerid,@advertiseid)";

                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@buyerid", Buyerid);
                cmd.Parameters.AddWithValue("@sellerid", mappingModel.Sellerid);
                cmd.Parameters.AddWithValue("@advertiseid", mappingModel.advertiseid);
                sqlConnection.Open();
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }

        }


        public int getMapid( int advertiseid,int buyerid,int sellerid)
        {
            SqlConnection connection = new SqlConnection(cs);
            string q = "select MapId from ChatMaping where AdvertiseId=@AdvertiseId and BuyerId=@buyerid and SellerId=@sellerid ";
            SqlCommand cmd = new SqlCommand(q,connection);
            cmd.Parameters.AddWithValue("@AdvertiseId", advertiseid);
            cmd.Parameters.AddWithValue("@buyerid", buyerid);
            cmd.Parameters.AddWithValue("@sellerid", sellerid);
            connection.Open();
          object count=cmd.ExecuteScalar();
            connection.Close();
            if (count != null)
            {
                return (int)count;
            }
            else
              
            return 0;
        }

        public bool EnterMessage(int mapid,string message,bool buyorsell )
        {

            //int mappingid = getMapid(mapid);
            SqlConnection connection = new SqlConnection(cs);
            string q = "insert into Chats(MapId, BuyOrSellId, Chat)values(@MapId, @buyorsell, @Chat)";
            SqlCommand cmd = new SqlCommand(q, connection);
            cmd.Parameters.AddWithValue("@MapId", mapid);
            cmd.Parameters.AddWithValue("@Chat", message);
            cmd.Parameters.AddWithValue("@buyorsell", buyorsell);

            connection.Open();
           int res= cmd.ExecuteNonQuery();
            connection.Close();
            if (res != 0)
            {
                return true;
            }
            else
                return false;

        }
        public List<ChatMappingModel> show()
        {
            List<ChatMappingModel> mappingModels = new List<ChatMappingModel>();
            SqlConnection connection = new SqlConnection(cs);
            string query = "select BuyerId,SellerId from ChatMaping";
            SqlCommand sqlCommand = new SqlCommand(query,connection);
            connection.Open();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            while (dr.Read())
            {
                ChatMappingModel chatMappingModel = new ChatMappingModel()
                {
                    Buyerid = (int)dr["Buyerid"],
                    Sellerid=(int)dr["Sellerid"]
                };
                mappingModels.Add(chatMappingModel);
            }
            connection.Close();
            return mappingModels;
        }


        public List<ChatsModel> GetChatModel()
        {
            List<ChatsModel> chatList = new List<ChatsModel>();

            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("select Chat,MapId,BuyOrSellId from Chats ", con);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                ChatsModel c = new ChatsModel()
                {
                    //Chatsec = (int)dr["Chatsec"],
                    Mapid=(int)dr["Mapid"],
                    BuyOrSellId=(bool)dr["BuyOrSellId"],
                    Chat=dr["Chat"].ToString()
                    //CreatedOn=(DateTime)dr["CreatedOn"]
                };
                //c.ChatId = Convert.ToInt32(dr.GetValue(0).ToString());
                //c.UserId = Convert.ToInt32(dr.GetValue(1).ToString());
                //c.ProductId = Convert.ToInt32(dr.GetValue(2).ToString());
                //c.BuyOrSellId = Convert.ToInt32(dr.GetValue(3).ToString());
                //c.Chat1 = dr.GetValue(4).ToString();


                chatList.Add(c);

            }
            con.Close();

            return chatList;

        }

        public List<ChatMddual> GetChatMdduals()
        {
            List<ChatMddual> chatList = new List<ChatMddual>();

            SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("ChatSelect", con);
           
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                ChatMddual c = new ChatMddual()
                {
                    UserId = Convert.ToInt32(dr["userid"]),
                    ChatId = Convert.ToInt32(dr["chatid"]),
                    ProductId = Convert.ToInt32(dr["ProductId"]),
                    BuyOrSellId = Convert.ToInt32(dr["buyorsellid"]),
                    Chat1 = dr["chat1"].ToString(),
                    firstName = dr["FirstName"].ToString()
                };
                //c.ChatId = Convert.ToInt32(dr.GetValue(0).ToString());
                //c.UserId = Convert.ToInt32(dr.GetValue(1).ToString());
                //c.ProductId = Convert.ToInt32(dr.GetValue(2).ToString());
                //c.BuyOrSellId = Convert.ToInt32(dr.GetValue(3).ToString());
                //c.Chat1 = dr.GetValue(4).ToString();


                chatList.Add(c);

            }
            con.Close();

            return chatList;

        }
        public bool insertChat(ChatMddual chat)
        {
            if (chat == null)
            {
                throw new ArgumentNullException("chat", "The chat object is null.");
            }

            string cs = ConfigurationManager.ConnectionStrings["OLXDB"]?.ConnectionString;

            if (string.IsNullOrEmpty(cs))
            {
                throw new ConfigurationErrorsException("Connection string 'OLXDB' not found in configuration.");
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO chat VALUES (@UserId, @ProductId, @BuyOrSellId, @chat)", con);
                    cmd.Parameters.AddWithValue("@UserId", chat.UserId);
                    cmd.Parameters.AddWithValue("@ProductId", chat.ProductId);
                    cmd.Parameters.AddWithValue("@BuyOrSellId", chat.BuyOrSellId);
                    cmd.Parameters.AddWithValue("@chat", chat.Chat1);

                    //  SqlCommand cmd = new SqlCommand("INSERT INTO chat (chat1) VALUES (@chat)", con);
                    //cmd.Parameters.AddWithValue("@chat", chat.Chat1);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();

                    if (i > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }
        public bool InsertChat(string chatMessage)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    //SqlCommand cmd = new SqlCommand("insert into chat (chat1) values (@chat1)", con);
                    SqlCommand cmd = new SqlCommand("Chatinsert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // cmd.Parameters.AddWithValue("@chat", chatMessage);
                    con.Open();
                    int i = cmd.ExecuteNonQuery();

                    return i > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
