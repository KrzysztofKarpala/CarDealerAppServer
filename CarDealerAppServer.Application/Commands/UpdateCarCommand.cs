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
    public record UpdateCarCommand(Guid CarId, CarDto CarDto) : IRequest
    {
    }

    public class UpdateCarCommandHandler : IRequestHandler<UpdateCarCommand>
    {
        private readonly ILogger<UpdateCarCommandHandler> _logger;
        private readonly ICarRepository _carRepository;
        public UpdateCarCommandHandler(ILogger<UpdateCarCommandHandler> logger, ICarRepository carRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
        }
        public async Task Handle(UpdateCarCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var car = await _carRepository.GetCarByIdAsync(request.CarId);
                if (car == null)
                {
                    throw new FileNotFoundException($"Car with Id: {request.CarId} does not exist.");
                }
                car.UpdateCar(request.CarDto.CarName, request.CarDto.CarDescription, request.CarDto.CarImage);
                await _carRepository.AddOrReplaceCarAsync(car);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(400, ex, ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(500, ex, "UpdateCarCommandHandler failed.");
                throw ex;
            }
        }
    }
}
