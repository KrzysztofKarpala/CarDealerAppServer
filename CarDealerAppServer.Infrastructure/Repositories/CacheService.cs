using CarDealerAppServer.Core.Entity;
using CarDealerAppServer.Core.Repository;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Infrastructure.Repositories
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        public CacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
        }
        public async Task AddCarAsync(CarResponse car)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(car.CarId.ToString(), JsonConvert.SerializeObject(car), new TimeSpan(0,30,0));
        }

        public async Task<CarResponse> GetCarByIdAsync(Guid carId)
        {
            var db = _connectionMultiplexer.GetDatabase();
            var ret = await db.StringGetAsync(carId.ToString());
            if(ret.IsNull)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<CarResponse>(ret);
        }

        public Task RemoveCarAsync(Guid carId)
        {
            throw new NotImplementedException();
        }
    }
}
