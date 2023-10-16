using OLX.Models.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace OLX.DA.User
{
    public class Chat
    {
        string cs = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
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
