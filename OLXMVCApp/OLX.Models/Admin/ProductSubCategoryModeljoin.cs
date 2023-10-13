using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.Models.Admin
{
    public class ProductSubCategoryModeljoin
    {
        [Required]
        public int productSubCategoryId { get; set; }
        [Required]
        public string productCategoryName { get; set; }
        [Required]
        public int productCategoryId { get; set; }
        [Required]
        public string productSubCategoryName { get; set; }
        //[Required]
        public DateTime createdOn { get; set; }
        // [Required]
        public DateTime updatedOn { get; set; }
    }
}
