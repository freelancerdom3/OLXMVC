using OLX.Models;
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
    public class SellDA
    {
        private SqlConnection con;
        private SqlTransaction transaction;

        public object imageData { get; private set; }

        public SellDA()
        {

            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);
        }

        public List<byte[]> GetAdvertiseImages(int advertiseId)
        {
            List<byte[]> imageDataList = new List<byte[]>();

            // Fetch images from the database based on the advertiseId
            using (SqlCommand cmd = new SqlCommand("SELECT imageData FROM tbl_AdvertiseImages WHERE advertiseId = @advertiseId", con))
            {
                cmd.Parameters.AddWithValue("@advertiseId", advertiseId);
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["imageData"] != DBNull.Value)
                        {
                            byte[] imageData = (byte[])reader["imageData"];
                            imageDataList.Add(imageData);
                        }
                    }
                }
                con.Close();
            }

            return imageDataList;
        }
        public MyAdvertiseModel getAdvertiseDetailsbyId(int? advertiseId)
        {
          
            MyAdvertiseModel myAdvertiseModel = new MyAdvertiseModel();


            if (advertiseId.HasValue)
            {
                string sqlQuery = "SELECT myad.advertiseId, myad.advertiseTitle, myad.advertiseDescription, myad.advertisePrice, myimg.imageData " +
                          "FROM tbl_MyAdvertise AS myad " +
                          "LEFT JOIN tbl_AdvertiseImages AS myimg ON myimg.advertiseId = myad.advertiseId " +
                          "WHERE myad.advertiseId = @advertiseId";
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@advertiseId", advertiseId.Value);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    myAdvertiseModel.advertiseId = Convert.ToInt32(dr["advertiseId"]);
                   // myAdvertiseModel.productSubCategoryId = Convert.ToInt32(dr["productSubCategoryId"]);
                    myAdvertiseModel.advertiseTitle = Convert.ToString(dr["advertiseTitle"]);
                    myAdvertiseModel.advertiseDescription = Convert.ToString(dr["advertiseDescription"]);
                    myAdvertiseModel.advertisePrice = Convert.ToDecimal(dr["advertisePrice"]);
                    //myAdvertiseModel.areaId = Convert.ToInt32(dr["areaId"]);
                   // myAdvertiseModel.userId = Convert.ToInt32(dr["userId"]);
                    if (dr["imageData"] != DBNull.Value)
                    {
                        myAdvertiseModel.imageData = (byte[])dr["imageData"];
                    }
                }

                con.Close();
            }

            return myAdvertiseModel;
        }

        public void EditAdvertiseDetails(MyAdvertiseModel productDetailsModel)
        {
           
            SqlCommand com = new SqlCommand("EditAdvertiseDetails", con);
            com.CommandType = CommandType.StoredProcedure;

            com.Parameters.AddWithValue("@advertiseId", productDetailsModel.advertiseId);
            com.Parameters.AddWithValue("@productSubCategoryId", productDetailsModel.productSubCategoryId);
            com.Parameters.AddWithValue("@advertiseTitle", productDetailsModel.advertiseTitle);
            com.Parameters.AddWithValue("@advertiseDescription", productDetailsModel.advertiseDescription);
            com.Parameters.AddWithValue("@advertisePrice", productDetailsModel.advertisePrice);
            com.Parameters.AddWithValue("@areaId", productDetailsModel.areaId);
            com.Parameters.AddWithValue("@userId", productDetailsModel.userId);


            if (productDetailsModel.imageData != null)
            {
                com.Parameters.Add("@imageData", SqlDbType.VarBinary, -1).Value = productDetailsModel.imageData;
            }
            else
            {
                com.Parameters.Add("@imageData", SqlDbType.VarBinary, -1).Value = DBNull.Value;
            }

           // com.Parameters.Add("@imageData", SqlDbType.VarBinary, -1).Value = productDetailsModel.imageData;
            //SqlParameter imageDataParam = new SqlParameter("@imageData", SqlDbType.VarBinary, -1);
            //imageDataParam.Value = productDetailsModel.imageData;
            //com.Parameters.Add(imageDataParam);

            con.Open();
            com.ExecuteNonQuery();
            con.Close();
        }

        public int InsertAdvertise(MyAdvertiseModel advertise)
        {
            int advertiseId = 0;

            try
            {
                con.Open();
                transaction = con.BeginTransaction();


                SqlCommand cmd = new SqlCommand("INSERT INTO tbl_MyAdvertise (productSubCategoryId, advertiseTitle, advertiseDescription, advertisePrice, areaId, userId, advertiseStatus,advertiseApproved, createdOn, updatedOn) OUTPUT INSERTED.advertiseId VALUES (@productSubCategoryId, @advertiseTitle, @advertiseDescription, @advertisePrice, @areaId, @userId, DEFAULT,DEFAULT, GETDATE(), GETDATE())", con, transaction);

                cmd.Parameters.AddWithValue("@productSubCategoryId", advertise.productSubCategoryId);
                cmd.Parameters.AddWithValue("@advertiseTitle", advertise.advertiseTitle);
                cmd.Parameters.AddWithValue("@advertiseDescription", advertise.advertiseDescription);
                cmd.Parameters.AddWithValue("@advertisePrice", advertise.advertisePrice);
                cmd.Parameters.AddWithValue("@areaId", advertise.areaId);
                cmd.Parameters.AddWithValue("@userId", advertise.userId);

                advertiseId = (int)cmd.ExecuteScalar();


                transaction.Commit();
            }
            catch (Exception ex)
            {

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
                transaction = con.BeginTransaction();

                List<byte[]> imageDataList = image.ImageDataList;
               // SqlCommand cmd = new SqlCommand("INSERT INTO tbl_AdvertiseImages(advertiseId, imageData,createdOn, updatedOn) VALUES (@advertiseId, @imageData,GETDATE(), GETDATE())", con, transaction);


                foreach (byte[] imageData in image.ImageDataList)
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO tbl_AdvertiseImages(advertiseId, imageData, createdOn, updatedOn) VALUES (@advertiseId, @imageData, GETDATE(), GETDATE())", con, transaction);
                    cmd.Parameters.AddWithValue("@advertiseId", image.advertiseId);
                    cmd.Parameters.Add("@imageData", SqlDbType.VarBinary, -1).Value = imageData;

                    cmd.ExecuteNonQuery();
                }


                transaction.Commit();
            }
            catch (Exception ex)
            {

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
                //product.productSubCategoryId = Convert.ToInt32(reader["productSubCategoryId"]);
                product.advertiseTitle = Convert.ToString(reader["advertiseTitle"]);
                product.advertiseDescription = Convert.ToString(reader["advertiseDescription"]);
                // product.imageData = (byte[])reader["imageData"];
                product.imageData = reader["imageData"] != DBNull.Value ? (byte[])reader["imageData"] : null;
                //product.areaName = Convert.ToString(reader["areaName"]);
                //product.areaId=Convert.ToInt32(reader["areaId"]);
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


        
    }
}