using CarDealerAppServer.Core.Entity;
using CarDealerAppServer.Core.Repository;
using CarDealerAppServer.Infrastructure.Mongo.DbSettings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CarDealerAppServer.Infrastructure.Mongo.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly IMongoCollection<Car> _carCollection;
        public CarRepository(IOptions<CarDatabaseSettings> carDatabaseSettings)
        {
            MongoClientSettings settings = MongoClientSettings.FromConnectionString(carDatabaseSettings.Value.DBConnectionString);
            var mongoClient = new MongoClient(settings);
            var mongoDatabase = mongoClient.GetDatabase(carDatabaseSettings.Value.DatabaseName);
            _carCollection = mongoDatabase.GetCollection<Car>(carDatabaseSettings.Value.CarCollectionName);
        }
        public async Task AddOrReplaceCarAsync(Car car)
        {
            await _carCollection.ReplaceOneAsync(x => x.CarId == car.CarId, car, new ReplaceOptions()
            {
                IsUpsert = true
            });
        }

        public async Task DeleteCarByIdAsync(Guid carId)
        {
            await _carCollection.DeleteOneAsync(x => x.CarId == carId);
        }

        public async Task DeleteCarByNameAsync(string name)
        {
            await _carCollection.DeleteOneAsync(x => x.CarName == name);
        }

        public async Task<Car> GetCarByIdAsync(Guid carId)
        {
            return await _carCollection.Find(x => x.CarId == carId).FirstOrDefaultAsync();
        }

        public async Task<Car> GetCarByNameAsync(string name)
        {
            return await _carCollection.Find(x => x.CarName == name).FirstOrDefaultAsync();
        }

        public async Task<List<Car>> GetCarsAsync()
        {
            return await _carCollection.Find(x => true).ToListAsync();
        }
    }
}
