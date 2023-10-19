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
   public class productDA
    {
        private SqlConnection con;

        //To Handle connection related activities    
        private void connection()
        {
            string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString();
            con = new SqlConnection(constr);

        }


        public IEnumerable<product> productDisplay()
        {
            connection();
            List<product> lstProject = new List<product>();
             string query = "select * from tbl_ProductCategory ";
            SqlDataAdapter da = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            con.Open();
            da.Fill(dt);
            con.Close();
            foreach (DataRow rdr in dt.Rows)
            {

                lstProject.Add(

                    new product
                    {
                        productCategoryId = Convert.ToInt32(rdr["productCategoryId"]),
                        productCategoryName = Convert.ToString(rdr["productCategoryName"]),                       
                        createdOn = Convert.ToDateTime(rdr["createdOn"]),
                        updatedOn = Convert.ToDateTime(rdr["updatedOn"]),
                    }
                );
            }
            return lstProject;
        }

        public void productDisplayAdd(product product)
        {
            connection();

            SqlCommand cmd = new SqlCommand("AddNewproductCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@productCategoryId", product.productCategoryId);
            cmd.Parameters.AddWithValue("@productCategoryName", product.productCategoryName);
            cmd.Parameters.AddWithValue("@createdOn",DateTime.Now);
            cmd.Parameters.AddWithValue("@updatedOn", DateTime.Now);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();

        }


        public product productDisplayDetail(int? productCategoryId)
        {
            connection();
            product product = new product();
            string sqlQuery = "SELECT * FROM tbl_ProductCategory WHERE productCategoryId= " + productCategoryId;
            SqlCommand cmd = new SqlCommand(sqlQuery, con);
            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                product.productCategoryId = Convert.ToInt32(rdr["productCategoryId"]);
                product.productCategoryName = rdr["productCategoryName"].ToString();
                product.createdOn = Convert.ToDateTime(rdr["createdOn"]);
                product.updatedOn = Convert.ToDateTime(rdr["updatedOn"]);
            }
            return product;
        }

        public void productDisplayDelete(int? productCategoryId)
        {
            connection();
            SqlCommand cmd = new SqlCommand("spDeletetbl_productCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@productCategoryId", productCategoryId);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
