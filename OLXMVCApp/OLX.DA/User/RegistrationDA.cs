﻿using OLX.Models.User;
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
    public class RegistrationDA
    {
        private SqlConnection con;
        public void connection()
        {
            con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        }
        public bool IsEmailAlreadyExists(string userEmail)
        {
            connection();
            string query = "select count(*) from Users where userEmail = @userEmail";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@userEmail", userEmail);
            if (con.State == ConnectionState.Closed)
                con.Open();
            int userCount = (int)cmd.ExecuteScalar();
            return userCount > 0;
        }

        public bool InsertUser(RegistrationModel obj)
        {
            SqlCommand cmd = new SqlCommand("insertuser", con);
           
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@firstName", obj.firstName);
            cmd.Parameters.AddWithValue("@lastName", obj.lastName);
            cmd.Parameters.AddWithValue("@userEmail", obj.userEmail);
            cmd.Parameters.AddWithValue("@Password", obj.Password);
            cmd.Parameters.AddWithValue("@MobileNo", obj.MobileNo);
            cmd.Parameters.AddWithValue("@Gender", obj.Gender);
            cmd.Parameters.AddWithValue("@Address", obj.Address);
            cmd.Parameters.AddWithValue("@City", obj.City);
            //cmd.Parameters.AddWithValue("@Role", Role);
            //con.Open();
            int rowsAffected = cmd.ExecuteNonQuery();
            con.Close();
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}