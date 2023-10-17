using OLX.Models.Admin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.DA.Admin
{
    public class AdvertiseDA
    {
        public List<MyAdvertiseModel> GetProductsFromDatabase()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            List<MyAdvertiseModel> products = new List<MyAdvertiseModel>();
            string query = "SELECT * FROM tbl_MyAdvertise";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();

            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                MyAdvertiseModel product = new MyAdvertiseModel()
                {
                    advertiseId = Convert.ToInt32(reader["advertiseId"]),
                    productSubCategoryId = Convert.ToInt32(reader["productSubCategoryId"]),
                    advertiseTitle = reader["advertiseTitle"].ToString(),
                    advertiseDescription = reader["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToDecimal(reader["advertisePrice"]),
                    areaId = Convert.ToInt32(reader["areaId"]),
                    advertiseStatus = reader.GetBoolean(reader.GetOrdinal("advertiseStatus")),
                    userId = Convert.ToInt32(reader["userId"]),
                    advertiseapproved = reader.GetBoolean(reader.GetOrdinal("advertiseapproved"))
                };
                products.Add(product);
            }
            sqlConnection.Close();
            return products;
        }

        public List<AdvertiseImagesModel> GetProductsFromDatabase1()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

            List<AdvertiseImagesModel> products = new List<AdvertiseImagesModel>();
            string query = "SELECT * FROM tbl_AdvertiseImages";
            SqlCommand command = new SqlCommand(query, sqlConnection);
            sqlConnection.Open();
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                AdvertiseImagesModel product = new AdvertiseImagesModel()
                {
                    advertiseImageId = Convert.ToInt32(reader["advertiseImageId"]),
                    advertiseId = Convert.ToInt32(reader["advertiseId"]),
                    imageData = (byte[])reader["imageData"]

                };
                products.Add(product);
            }
            sqlConnection.Close();
            return products;
        }
            public void Deleteproduct(int advertiseId, int advertiseImageId)
            {

                // Perform the delete operation using ADO.NET
                SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

                string query = "DELETE FROM tbl_MyAdvertise WHERE advertiseId = @advertiseId";
                SqlCommand command = new SqlCommand(query, sqlConnection);
                command.Parameters.AddWithValue("@advertiseId", advertiseId);

                string query1 = "DELETE FROM tbl_AdvertiseImages WHERE advertiseImageId = @advertiseImageId";
                SqlCommand command1 = new SqlCommand(query1, sqlConnection);
                command1.Parameters.AddWithValue("@advertiseImageId", advertiseImageId);
                sqlConnection.Open();
                int rowsAffected1 = command.ExecuteNonQuery();
                int rowsAffected2 = command1.ExecuteNonQuery();
                sqlConnection.Close();

            }


        }
    }

