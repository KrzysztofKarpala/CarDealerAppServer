using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Core.Entity
{
    public class CarResponse
    {
        public Guid CarId { get; set; }
        public string CarName { get; set; }
        public string CarDescription { get; set; }
        public string CarImage { get; set; }
    }
}
