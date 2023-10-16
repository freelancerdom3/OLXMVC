using OLX.Models.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.Models.User
{
   public class UserBuyModel
    {
        public int advertiseImageId { get; set; }
        public byte[] imageData { get; set; }
        public int advertiseId { get; set; }
        public string advertiseTitle { get; set; }
        public string advertiseDescription { get; set; }
        public int advertisePrice { get; set; }
        public bool advertiseStatus { get; set; }
        public int userId { get; set; }
        public string firstName { get; set; }
        public bool advertiseapproved { get; set; }
        public int productCategoryId { get; set; }
        public string productCategoryName { get; set; }
        public int productSubCategoryId { get; set; }
        public string productSubCategoryName { get; set; }
        public int areaId { get; set; }
        public string areaName { get; set; }
        public int cityId { get; set; }
        public string cityName { get; set; }
        public int stateId { get; set; }
        public string stateName { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
    }
}
