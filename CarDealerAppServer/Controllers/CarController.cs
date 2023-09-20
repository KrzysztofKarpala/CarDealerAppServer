using CarDealerAppServer.Application.Dto;
using CarDealerAppServer.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CarDealerAppServer.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/v{version:apiVersion}/cars")]
    public class CarController : Controller
    {
        private readonly ILogger<CarController> _logger;
        private readonly IMediator _mediator;
        public CarController(ILogger<CarController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpGet("getallcars")]
        public async Task<ActionResult<List<CarDto>>> GetAllCars()
        {
            var getCarsQuery = new GetCarsQuery();
            var res = await _mediator.Send(getCarsQuery);
            /*            var cars = new List<CarDto>();
                        var car1 = new CarDto() { CarId = Guid.NewGuid(), CarName = "Audi", CarDescription = "Good car", CarImage = "https://mediaservice.audi.com/media/live/50900/fly1400x601n1/f83rj7/2022.png?wid=850" };
                        var car2 = new CarDto() { CarId = Guid.NewGuid(), CarName = "Mercedes", CarDescription = "Fine car", CarImage = "https://www.mercedes-benz.pl/passengercars/mercedes-benz-cars/coupe/range-overview/_jcr_content/swipeableteaserbox/par/swipeableteaser_1902035350/asset.MQ6.12.20201216143714.jpeg" };
                        cars.Add(car1);
                        cars.Add(car2);*/
            _logger.LogInformation(200, "Fetched Cars.");
            return Ok(res);
        }

        [HttpGet("getcarbyid")]
        public async Task<ActionResult<CarDto>> GetCarById(Guid CarId)
        {
            var car = new CarDto() { CarId = CarId, CarName = "Audi", CarDescription = "Good car", CarImage = "https://mediaservice.audi.com/media/live/50900/fly1400x601n1/f83rj7/2022.png?wid=850" };
            return Ok(car);
        }

        [HttpPost("addcar")]
        public async Task<ActionResult<string>> AddCar()
        {
            return Ok("Added");
        }
    }
}
