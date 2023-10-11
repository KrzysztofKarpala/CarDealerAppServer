using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Application.Contracts
{
    public interface IAddCarToCacheMessage
    {
        Guid CarId { get; set; }
        string CarName { get; set; }
        string CarDescription { get; set; }
        string CarImage { get; set; }
    }
    public class AddCarToCacheMessage : IAddCarToCacheMessage
    {
        public Guid CarId { get; set; }
        public string CarName { get; set; }
        public string CarDescription { get; set; }
        public string CarImage { get; set; }
    }
}
