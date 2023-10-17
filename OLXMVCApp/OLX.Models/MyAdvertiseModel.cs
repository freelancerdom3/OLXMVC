using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.Models
{
    public class MyAdvertiseModel
    {
        public int advertiseId { get; set; }
        public int productSubCategoryId { get; set; }
        public string advertiseTitle { get; set; }
        public string advertiseDescription { get; set; }
        public decimal advertisePrice { get; set; }
        public int areaId { get; set; }
        public bool advertiseStatus { get; set; }
        public int userId { get; set; }
        public bool advertiseapproved { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }

    }
    public class AdvertiseImagesModel
    {
        public int advertiseImageId { get; set; }
        public int advertiseId { get; set; }
        public byte[] imageData { get; set; }
        public List<byte[]> ImageDataList { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }

    }

}
