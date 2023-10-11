using CarDealerAppServer.Application.Commands;
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
        public async Task<ActionResult<List<CarResponseDto>>> GetAllCars()
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getcarbyid")]
        public async Task<ActionResult<CarResponseDto>> GetCarById(Guid carId)
        {
            try
            {
                var getCarByIdQuery = new GetCarByIdQuery(carId);
                var res = await _mediator.Send(getCarByIdQuery);
                return Ok(res);
            }
            catch(FileNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("getcarsbyname")]
        public async Task<ActionResult<List<CarResponseDto>>> GetCarsByName(string carName)
        {
            try
            {
                var getCarByIdQuery = new GetCarsByNameQuery(carName);
                var res = await _mediator.Send(getCarByIdQuery);
                return Ok(res);
            }
            catch (FileNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("addcar")]
        public async Task<ActionResult> AddCar(CarDto carDto)
        {
            try
            {
                var addCarCommand = new AddCarCommand(carDto);
                await _mediator.Send(addCarCommand);
                _logger.LogInformation(200, $"Added Car with name: {carDto.CarName}.");
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("updatecar")]
        public async Task<ActionResult> UpdateCar(Guid carId, CarDto carDto)
        {
            try
            {
                var updateCarCommand = new UpdateCarCommand(carId, carDto);
                await _mediator.Send(updateCarCommand);
                _logger.LogInformation(200, $"Updated Car with id: {carId}.");
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete("deletecar")]
        public async Task<ActionResult> DeleteCar(Guid carId)
        {
            try
            {
                var deleteCarByIdCommand = new DeleteCarByIdCommand(carId);
                await _mediator.Send(deleteCarByIdCommand);
                _logger.LogInformation(200, $"Deleted Car with Id: {carId}.");
                return Ok();
            }
            catch (FileNotFoundException ex)
            {
                return StatusCode(StatusCodes.Status404NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
