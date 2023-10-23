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
using OLX.Models;
namespace OLX.DA.User
{
    public class Chat
    {
        private SqlConnection sqlConnection;


        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            sqlConnection = new SqlConnection(constr);

        }
        //string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public List<showChatModel> showchat(int mapid)
        {
            connection();

            List<showChatModel> list = new List<showChatModel>();
            SqlCommand cmd = new SqlCommand("showChat", sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Mapid",mapid);
            sqlConnection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                showChatModel showChatModel = new showChatModel();

                showChatModel.AdvertiseId = Convert.ToInt32(dr["AdvertiseId"]);
                showChatModel.userId = Convert.ToInt32(dr["userId"]);
                showChatModel.BuyOrSellId = Convert.ToBoolean(dr["BuyOrSellId"]);
                showChatModel.firstName = dr["firstName"].ToString();
                showChatModel.Chat = dr["Chat"].ToString();

                list.Add(showChatModel);
            }
            sqlConnection.Close();
            return list;
        }
        public void InsertInMap( ChatMappingModel mappingModel/*int Buyerid, int Sellerid, int advertiseid*/)
        {
            connection();
            //int Buyerid = Convert.ToInt32(HttpContext.Current.Session["userid"]);
            //SqlConnection sqlConnection = new SqlConnection(cs);
            string checkQuery = "SELECT COUNT(*) FROM ChatMaping WHERE BuyerId = @buyerid AND SellerId = @sellerid AND advertiseid = @advertiseid";

            SqlCommand checkCmd = new SqlCommand(checkQuery, sqlConnection);
            checkCmd.Parameters.AddWithValue("@buyerid", mappingModel.Buyerid);
            checkCmd.Parameters.AddWithValue("@sellerid", mappingModel.Sellerid);
            checkCmd.Parameters.AddWithValue("@advertiseid", mappingModel.advertiseid);

            sqlConnection.Open();
            int existingRecordCount = (int)checkCmd.ExecuteScalar();
            sqlConnection.Close();


            if (existingRecordCount == 0) {
                string query = "insert into ChatMaping (BuyerId,SellerId,advertiseid)" +
                       " values (@buyerid,@sellerid,@advertiseid)";

                SqlCommand cmd = new SqlCommand(query, sqlConnection);
                cmd.Parameters.AddWithValue("@buyerid", mappingModel.Buyerid);
                cmd.Parameters.AddWithValue("@sellerid", mappingModel.Sellerid);
                cmd.Parameters.AddWithValue("@advertiseid", mappingModel.advertiseid);
                sqlConnection.Open();
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
            }

        }
        public int isseller(int advertiseid)
        {
            connection();
            string query = "select userId from tbl_MyAdvertise where AdvertiseId=@advertiseid";
            SqlCommand cmd = new SqlCommand(query,sqlConnection);
            cmd.Parameters.AddWithValue("@advertiseid", advertiseid);
            sqlConnection.Open();
            int count =(int) cmd.ExecuteScalar();
            sqlConnection.Close();
            if (count > 0)
            {
                return count;
            }
            else
                return 0;
        }
        public int getMapidfromOR(int advertiseid, int buyerid, int sellerid)
        {
            connection();
            //SqlConnection connection = new SqlConnection(cs);
            string q = "SELECT MapId FROM ChatMaping WHERE(BuyerId=@buyerid OR BuyerId=@sellerid) AND(SellerId=@sellerid OR SellerId=@buyerid) and AdvertiseId=@advertiseid";
            SqlCommand cmd = new SqlCommand(q, sqlConnection);
            cmd.Parameters.AddWithValue("@AdvertiseId", advertiseid);
            cmd.Parameters.AddWithValue("@buyerid", buyerid);
            cmd.Parameters.AddWithValue("@sellerid", sellerid);
            sqlConnection.Open();
            object count = cmd.ExecuteScalar();
            sqlConnection.Close();
            if (count != null)
            {
                return (int)count;
            }
            else

                return 0;
        }
        public int getMapid( int advertiseid,int buyerid,int sellerid)
        {
            connection();
            ChatMappingModel mappingModel = new ChatMappingModel();
            //SqlConnection connection = new SqlConnection(cs);
            string q = "select MapId from ChatMaping where AdvertiseId=@AdvertiseId and BuyerId=@buyerid and SellerId=@sellerid ";
            SqlCommand cmd = new SqlCommand(q, sqlConnection);
            cmd.Parameters.AddWithValue("@AdvertiseId", advertiseid);
            cmd.Parameters.AddWithValue("@buyerid", buyerid);
            cmd.Parameters.AddWithValue("@sellerid", sellerid);
            sqlConnection.Open();
          object count=cmd.ExecuteScalar();
            sqlConnection.Close();
            if (count != null)
            {
                return (int)count;
            }
            else
              
            return 0;
        }

        public bool EnterMessage(int mapid,string message,bool buyorsell )
        {
            connection();
            //int mappingid = getMapid(mapid);
            //SqlConnection connection = new SqlConnection(cs);
            string q = "insert into Chats(MapId, BuyOrSellId, Chat)values(@MapId, @buyorsell, @Chat)";
            SqlCommand cmd = new SqlCommand(q, sqlConnection);
            cmd.Parameters.AddWithValue("@MapId", mapid);
            cmd.Parameters.AddWithValue("@Chat", message);
            cmd.Parameters.AddWithValue("@buyorsell", buyorsell);

            sqlConnection.Open();
           int res= cmd.ExecuteNonQuery();
            sqlConnection.Close();
            if (res != 0)
            {
                return true;
            }
            else
                return false;

        }

       
        public int getadvertiseid(int userid)
        {

            connection();
            //int userid= Convert.ToInt32(HttpContext.Current.Session["userid"]);
            string q = "select advertiseId from tbl_MyAdvertise where userId=@userid";

            SqlCommand cmd = new SqlCommand(q, sqlConnection);
            cmd.Parameters.AddWithValue("@userid", userid);
            sqlConnection.Open();
           int adid= (int)cmd.ExecuteScalar();
            sqlConnection.Close();
            if (adid > 0)
            {
                return adid;
            }
            else
                return 0;
        }

        public List<int> getAdvertiseIds(int userid)
        {
            connection();
            List<int> adIds = new List<int>();


            sqlConnection.Open();

                string sqlQuery = "SELECT advertiseId FROM tbl_MyAdvertise WHERE userId=@userid";

                using (SqlCommand command = new SqlCommand(sqlQuery, sqlConnection))
                {
                    command.Parameters.AddWithValue("@userId", userid);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int adId = (int)reader["AdvertiseId"];
                            adIds.Add(adId);
                        }
                    }
                }
            

            return adIds;
        }
        public List<GetnameModel> getadid(int userid)
        {
            connection();
            List<GetnameModel> models = new List<GetnameModel>();
            string q = "select advertiseId from tbl_MyAdvertise where userId=@userid";
            SqlCommand command = new SqlCommand(q,sqlConnection);
            command.Parameters.AddWithValue("@userid",userid);
            sqlConnection.Open();
            SqlDataReader dr = command.ExecuteReader();
            while (dr.Read())
            {
                GetnameModel getnameModel = new GetnameModel()
                {
                    AdvertiseId = (int)dr["AdvertiseId"]
                };
                models.Add(getnameModel);

            }
            sqlConnection.Close();
            return models;
        } 

        public List<GetnameModel> getName(int? adid)
        {
            connection();
            List<GetnameModel> getnames = new List<GetnameModel>();
            SqlCommand cmd = new SqlCommand("getBuyerName",sqlConnection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AdvertiseId",adid);
            sqlConnection.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                GetnameModel getname = new GetnameModel()
                {
                    firstName=dr["firstName"].ToString(),
                    AdvertiseId=Convert.ToInt32(dr["AdvertiseId"]),
                    BuyOrSellId=Convert.ToBoolean(dr["BuyOrSellId"]),
                    MapId=Convert.ToInt32(dr["MapId"]),
                    userId=Convert.ToInt32(dr["userId"])
                };
                getnames.Add(getname);
            }
            sqlConnection.Close();
            return getnames;

        }

        public List<ChatMappingModel> show()
        {
            connection();
            List<ChatMappingModel> mappingModels = new List<ChatMappingModel>();
            //SqlConnection connection = new SqlConnection(cs);
            string query = "select BuyerId,SellerId from ChatMaping";
            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
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
            sqlConnection.Close();
            return mappingModels;
        }


        public List<ChatsModel> GetChatModel()
        {
            connection();
            List<ChatsModel> chatList = new List<ChatsModel>();

            //SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("select Chat,MapId,BuyOrSellId from Chats", sqlConnection);

            sqlConnection.Open();
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
            sqlConnection.Close();

            return chatList;

        }

        public List<ChatMddual> GetChatMdduals()
        {
            connection();
            List<ChatMddual> chatList = new List<ChatMddual>();

            //SqlConnection con = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand("ChatSelect", sqlConnection);
           
            cmd.CommandType = CommandType.StoredProcedure;
            sqlConnection.Open();
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
            sqlConnection.Close();

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
            
            using (SqlConnection con = new SqlConnection())
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
