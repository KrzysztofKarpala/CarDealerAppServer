using CarDealerAppServer.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Core.Entity
{
    public class Car : AggregateRoot
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

        public static Car CreateCar(string carName, string carDescritpion, string carImage)
        {
            if(string.IsNullOrWhiteSpace(carName))
            {
                throw new ArgumentException("Car Name can not be empty.");
            }      
            if(string.IsNullOrWhiteSpace(carDescritpion))
            {
                throw new ArgumentException("Car Description can not be empty.");
            }     
            if(string.IsNullOrWhiteSpace(carImage))
            {
                throw new ArgumentException("Car Image can not be empty.");
            }
            var car = new Car(Guid.NewGuid(), carName, carDescritpion, carImage);
            car.SetCreationDate();
            car.SetModificationDate();
            car.IncrementVersion();
            return car;
        }

        public void UpdateCar(string carName, string carDescritpion, string carImage)
        {
            if (string.IsNullOrWhiteSpace(carName))
            {
                throw new ArgumentException("Car Name can not be empty.");
            }
            if (string.IsNullOrWhiteSpace(carDescritpion))
            {
                throw new ArgumentException("Car Description can not be empty.");
            }
            if (string.IsNullOrWhiteSpace(carImage))
            {
                throw new ArgumentException("Car Image can not be empty.");
            }
            CarName = carName;
            CarDescription = carDescritpion;
            CarImage = carImage;
            SetModificationDate();
            IncrementVersion();
        }
    }
}
