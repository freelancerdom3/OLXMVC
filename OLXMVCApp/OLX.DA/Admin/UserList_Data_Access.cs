using OLX.Models.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.DA.Admin
{
    public class UserList_Data_Access
    {
        private SqlConnection con;
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);

        }


        public List<UserList> GetAllUser()
        {
            connection();
            List<UserList> lstUser = new List<UserList>();
            SqlCommand cmd = new SqlCommand("GetUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                UserList ul = new UserList();
                ul.UserId = Convert.ToInt32(dr["userId"]);
                ul.firstName = Convert.ToString(dr["firstName"]);
                ul.lastName = Convert.ToString(dr["lastName"]);
                ul.userEmail = Convert.ToString(dr["userEmail"]);
                ul.Password = Convert.ToString(dr["Password"]);
                ul.MobileNo = Convert.ToString(dr["MobileNo"]);
                ul.Gender = Convert.ToString(dr["Gender"]);
                ul.Address = Convert.ToString(dr["Address"]);
                ul.City = Convert.ToString(dr["City"]);
                //createupdate

                lstUser.Add(ul);
            }
            con.Close();

            return lstUser;
        }

        public int Updateuser(UserList ul)
        {
            int i;
            connection();

            SqlCommand cmd = new SqlCommand("updateRecord", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userId", ul.UserId);
            cmd.Parameters.AddWithValue("@firstName", ul.firstName);
            cmd.Parameters.AddWithValue("@lastName", ul.lastName);
            cmd.Parameters.AddWithValue("@userEmail", ul.userEmail);
            cmd.Parameters.AddWithValue("@Password", ul.Password);
            cmd.Parameters.AddWithValue("@MobileNo", ul.MobileNo);
            cmd.Parameters.AddWithValue("@Gender", ul.Gender);
            cmd.Parameters.AddWithValue("@Address", ul.Address);
            cmd.Parameters.AddWithValue("@City", ul.City);
            con.Open();
            i = cmd.ExecuteNonQuery();
            con.Close();

            return i;
        }
        public int DeleteUser(int? userId)
        {
            int i;
            connection();

            SqlCommand cmd = new SqlCommand("deleteRecord", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId);
            con.Open();
            i = cmd.ExecuteNonQuery();
            con.Close();
            return i;
        }
        public UserList GetUserData(int? userId)
        {
            connection();
            UserList ul = new UserList();
            string sqlQuery = "SELECT * FROM users WHERE userId= " + userId;
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                ul.UserId = Convert.ToInt32(dr["userId"]);
                ul.firstName = Convert.ToString(dr["firstName"]);
                ul.lastName = Convert.ToString(dr["lastName"]);
                ul.userEmail = Convert.ToString(dr["userEmail"]);
                ul.Password = Convert.ToString(dr["Password"]);
                ul.MobileNo = Convert.ToString(dr["MobileNo"]);
                ul.Gender = Convert.ToString(dr["Gender"]);
                ul.Address = Convert.ToString(dr["Address"]);
                ul.City = Convert.ToString(dr["City"]);
            }
            return ul;
        }

    }
}
