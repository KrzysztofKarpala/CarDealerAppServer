using CarDealerAppServer.Application.Dto;
using CarDealerAppServer.Core.Entity;
using CarDealerAppServer.Core.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Application.Commands
{
    public record AddCarCommand(CarDto CarDto) : IRequest
    {
    }

    public class AddCarCommandHandler : IRequestHandler<AddCarCommand>
    {
        private readonly ILogger<AddCarCommandHandler> _logger;
        private readonly ICarRepository _carRepository;
        private readonly ICacheService _cacheService;
        public AddCarCommandHandler(ILogger<AddCarCommandHandler> logger, ICarRepository carRepository, ICacheService cacheService)
        {
            _logger = logger;
            _carRepository = carRepository;
            _cacheService = cacheService;
        }
        public async Task Handle(AddCarCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var car = Car.CreateCar(request.CarDto.CarName, request.CarDto.CarDescription, request.CarDto.CarImage);
                await _carRepository.AddOrReplaceCarAsync(car);
            }
            catch(ArgumentException ex)
            {
                _logger.LogWarning(400, ex, ex.Message);
                throw ex;
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(404, ex, ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(500, ex, "AddCarCommandHandler failed.");
                throw ex;
            }
        }
    }
}
