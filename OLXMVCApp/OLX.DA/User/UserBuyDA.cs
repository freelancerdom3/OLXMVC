
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
    public class UserBuyDA
    {
        private SqlConnection _connection;
        public UserBuyDA()
        {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

        }

        #region FILTER
        public List<UserBuyModel> newFilter(int? productCategoryId, int? productSubCategoryId, int? stateId, int? cityId,
            int? areaId, decimal? minprice, decimal? maxprice, int? advertiseId)
        {
            List<UserBuyModel> models = new List<UserBuyModel>();
            SqlCommand cmd = new SqlCommand("newfilter2", _connection);
            _connection.Open();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productCategoryId", productCategoryId);
            cmd.Parameters.AddWithValue("@productsubCategoryId", productSubCategoryId);
            cmd.Parameters.AddWithValue("@stateId", stateId);
            cmd.Parameters.AddWithValue("@cityId", cityId);
            cmd.Parameters.AddWithValue("@areaId", areaId);
            cmd.Parameters.AddWithValue("@minprice", minprice);
            cmd.Parameters.AddWithValue("@maxprice", maxprice);
            cmd.Parameters.AddWithValue("@advertiseId", advertiseId);

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {

                bool isApproved = Convert.ToBoolean(reader["advertiseapproved"]);

                if (isApproved)
                {
                    UserBuyModel model = new UserBuyModel()
                    {
                        advertiseTitle = reader["advertiseTitle"].ToString(),
                        imageData = (byte[])reader["imageData"],
                        advertiseDescription = reader["advertiseDescription"].ToString(),
                        advertisePrice = Convert.ToInt32(reader["advertisePrice"]),
                        cityName = reader["cityName"].ToString(),
                        stateName = reader["statename"].ToString(),
                        areaName = reader["areaName"].ToString(),
                        advertiseId = Convert.ToInt32(reader["advertiseId"]),
                        productCategoryName = reader["productCategoryName"].ToString(),
                        productSubCategoryName = reader["productSubCategoryName"].ToString(),
                        productCategoryId = Convert.ToInt32(reader["productCategoryId"]),
                        productSubCategoryId = Convert.ToInt32(reader["productSubCategoryId"]),

                    };
                    models.Add(model);
                }
            }
            _connection.Close();
            return models;
        }
        #endregion

        public List<UserBuyModel> GetCategoryWithSubcategories()
        {
            List<UserBuyModel> models = new List<UserBuyModel>();

            SqlCommand cmd = new SqlCommand("CategoryWithSubcategory", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            _connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                UserBuyModel subCategory = new UserBuyModel()
                {
                    productCategoryId = Convert.ToInt32(reader["productCategoryId"]),
                    productCategoryName = reader["productCategoryName"].ToString(),
                    productSubCategoryId = Convert.ToInt32(reader["productSubCategoryId"]),
                    productSubCategoryName = reader["productSubCategoryName"].ToString(),
                };
                models.Add(subCategory);
            }
            _connection.Close(); return models;
        }

        public IEnumerable<UserBuyModel> GetAdvertiseById(int advertiseId)
        {
            List<UserBuyModel> advertise = new List<UserBuyModel>();
            _connection.Open();
            SqlCommand cmd = new SqlCommand("DisplayAdDetail ", _connection);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@advertiseId", advertiseId);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                UserBuyModel add = new UserBuyModel()
                {
                    advertiseTitle = dr["advertiseTitle"].ToString(),
                    advertiseDescription = dr["advertiseDescription"].ToString(),
                    advertisePrice = Convert.ToInt32(dr["advertisePrice"]),
                    cityName = dr["cityName"].ToString(),
                    stateName = dr["statename"].ToString(),
                    areaName = dr["areaName"].ToString(),
                    imageData = (byte[])dr["imageData"],
                    advertiseId = Convert.ToInt32(dr["advertiseId"]),
                    userId = Convert.ToInt32(dr["userId"]),
                    firstName = dr["firstName"].ToString(),
                };
                advertise.Add(add);
            }
            _connection.Close();
            return advertise;
        }

        public List<UserBuyModel> GetLocation()
        {
            List<UserBuyModel> location = new List<UserBuyModel>();

            SqlCommand cmd = new SqlCommand("DisplayLocation", _connection);
            cmd.CommandType = CommandType.StoredProcedure;
            _connection.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                UserBuyModel modelLocation = new UserBuyModel()
                {
                    stateId = Convert.ToInt32(reader["stateId"]),
                    stateName = reader["stateName"].ToString(),
                    cityId = Convert.ToInt32(reader["cityId"]),
                    cityName = reader["cityName"].ToString(),
                    areaId = Convert.ToInt32(reader["areaId"]),
                    areaName = reader["areaName"].ToString(),
                };
                location.Add(modelLocation);
            }
            _connection.Close();
            return location;
        }
    }
}
