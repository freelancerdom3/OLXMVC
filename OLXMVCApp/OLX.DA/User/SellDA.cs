using OLX.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApplication1.Models
{
    public class SellDA : IDisposable
    {
        private SqlConnection con;
        private SqlTransaction transaction;

        public SellDA()
        {
            // Initialize the connection in the constructor
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);
        }

        public int InsertAdvertise(MyAdvertiseModel advertise)
        {
            int advertiseId = 0;

            try
            {
                con.Open();
                transaction = con.BeginTransaction(); // Start a new transaction

                // Create a SQL command to insert data into the Advertise table
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_MyAdvertise (productSubCategoryId, advertiseTitle, advertiseDescription, advertisePrice, areaId, userId, advertiseStatus,advertiseApproved, createdOn, updatedOn) OUTPUT INSERTED.advertiseId VALUES (@productSubCategoryId, @advertiseTitle, @advertiseDescription, @advertisePrice, @areaId, @userId, DEFAULT,DEFAULT, GETDATE(), GETDATE())", con, transaction);

                cmd.Parameters.AddWithValue("@productSubCategoryId", advertise.productSubCategoryId);
                cmd.Parameters.AddWithValue("@advertiseTitle", advertise.advertiseTitle);
                cmd.Parameters.AddWithValue("@advertiseDescription", advertise.advertiseDescription);
                cmd.Parameters.AddWithValue("@advertisePrice", advertise.advertisePrice);
                cmd.Parameters.AddWithValue("@areaId", advertise.areaId);
                cmd.Parameters.AddWithValue("@userId", advertise.userId);




                advertiseId = (int)cmd.ExecuteScalar();

                // Commit the transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Handle exceptions and roll back the transaction
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return advertiseId;
        }

        public void InsertAdvertiseImage(AdvertiseImagesModel image)
        {
            try
            {
                con.Open();
                transaction = con.BeginTransaction(); // Start a new transaction

                // Create a SQL command to insert data into the AdvertiseImages table
                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_AdvertiseImages(advertiseId, imageData,createdOn, updatedOn) VALUES (@advertiseId, @imageData,GETDATE(), GETDATE())", con, transaction);
                cmd.Parameters.AddWithValue("@advertiseId", image.advertiseId);

                foreach (byte[] imageData in image.ImageDataList)
                {
                    cmd.Parameters.Add("@imageData", SqlDbType.VarBinary, -1).Value = imageData;

                    cmd.ExecuteNonQuery();
                }

                // Commit the transaction
                transaction.Commit();
            }
            catch (Exception ex)
            {
                // Handle exceptions and roll back the transaction
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public List<MyAdvertiseModel> GetAdvertiseDetails()
        {
            connection();
            SqlCommand com = new SqlCommand("GetAdvertiseDetails", con);
            com.CommandType = CommandType.StoredProcedure;

            List<MyAdvertiseModel> productList = new List<MyAdvertiseModel>();

            con.Open();
            SqlDataReader reader = com.ExecuteReader();
            while (reader.Read())
            {
                MyAdvertiseModel product = new MyAdvertiseModel();
                product.advertiseId = Convert.ToInt32(reader["advertiseId"]);
                //product.productSubCategoryName = Convert.ToString(reader["productSubCategoryName"]);
                product.productSubCategoryId = Convert.ToInt32(reader["productSubCategoryId"]);
                product.advertiseTitle = Convert.ToString(reader["advertiseTitle"]);
                product.advertiseDescription = Convert.ToString(reader["advertiseDescription"]);
                // product.imageData = (byte[])reader["imageData"];
                product.imageData = reader["imageData"] != DBNull.Value ? (byte[])reader["imageData"] : null;
                //product.areaName = Convert.ToString(reader["areaName"]);
                product.areaId = Convert.ToInt32(reader["areaId"]);
                product.advertisePrice = reader.GetDecimal(reader.GetOrdinal("advertisePrice"));

                //product.advertiseStatus= Convert.ToBoolean(reader["advertiseStatus"]);
                // product.firstName= Convert.ToString(reader["firstName"]);
                //product.userId=Convert.ToInt32(reader["userId"]);
                // product.advertiseapproved= Convert.ToBoolean(reader["advertiseapproved"]);
                product.createdOn = Convert.ToDateTime(reader["createdOn"]);
                product.updatedOn = Convert.ToDateTime(reader["updatedOn"]);
                productList.Add(product);
            }
            con.Close();
            return productList;
        }

        private void connection()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            con.Dispose();
        }
    }
}