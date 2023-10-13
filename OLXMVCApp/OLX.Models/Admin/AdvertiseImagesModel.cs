using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OLX.Models.Admin
{
    public class AdvertiseImagesModel
    {
        public int advertiseImageId { get; set; }
        public int advertiseId { get; set; }
        public byte[] imageData { get; set; }
        public byte[] ImageBytes { get; set; }
    }
}
