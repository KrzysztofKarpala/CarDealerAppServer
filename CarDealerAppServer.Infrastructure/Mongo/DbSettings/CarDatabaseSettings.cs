using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Infrastructure.Mongo.DbSettings
{
    public class CarDatabaseSettings
    {
        public string DBConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CarCollectionName { get; set; } = null!;
    }
}
