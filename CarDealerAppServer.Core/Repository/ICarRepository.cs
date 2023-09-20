using CarDealerAppServer.Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Core.Repository
{
    public interface ICarRepository
    {
        public Task<List<Car>> GetCarsAsync();
        public Task<Car> GetCarByIdAsync(Guid carId);
        public Task<List<Car>> GetCarsByNameAsync(string name);
        public Task AddOrReplaceCarAsync(Car car);
        public Task DeleteCarByIdAsync(Guid carId);
        public Task DeleteCarByNameAsync(string name);
    }
}
