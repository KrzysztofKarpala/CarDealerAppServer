using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Core.Entity
{
    public class Car
    {
        public Guid CarId { get; private set; }
        public string CarName { get; private set; }
        public string CarDescription { get; private set; }
        public string CarImage { get; private set; }
        private Car(Guid carId, string carName, string carDescritpion, string carImage)
        {
            CarId = carId;
            CarName = carName;
            CarDescription = carDescritpion;
            CarImage = carImage;
        }

        public Car CreateCar(string carName, string carDescritpion, string carImage)
        {
            var car = new Car(Guid.NewGuid(), carName, carDescritpion, carImage);
            return car;
        }
    }
}
