using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Keepi.Shared
{
    public class ImageUploadModel
    {
        public Guid UserId { get; set; }
        public string ImageBase64 { get; set; }
    }
}
