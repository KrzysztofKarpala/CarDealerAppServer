using CarDealerAppServer.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Core.Repository
{
    public interface ICacheService
    {
        Task AddCarAsync(CarResponse car);
        Task RemoveCarAsync(Guid carId);
        Task<CarResponse> GetCarByIdAsync(Guid carId);
    }
}
