using CarDealerAppServer.Application.Contracts;
using CarDealerAppServer.Application.Dto;
using CarDealerAppServer.Core.Repository;
using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarDealerAppServer.Application.Queries
{
    public record GetCarByIdQuery(Guid CarId) : IRequest<CarResponseDto>
    {
    }

    public class GetCarByIdQueryHandler : IRequestHandler<GetCarByIdQuery, CarResponseDto>
    {
        private readonly ILogger<GetCarByIdQueryHandler> _logger;
        private readonly ICarRepository _carRepository;
        private readonly ICacheService _cacheService;
        private readonly IPublishEndpoint _publishEndpoint;
        public GetCarByIdQueryHandler(ILogger<GetCarByIdQueryHandler> logger, ICarRepository carRepository, ICacheService cacheService, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _carRepository = carRepository;
            _cacheService = cacheService;
            _publishEndpoint = publishEndpoint;
        }
        public async Task<CarResponseDto> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var carResponse = await _cacheService.GetCarByIdAsync(request.CarId);
                var carResponseDto = new CarResponseDto();
                if(carResponse != null)
                {
                    carResponseDto = carResponse.Adapt<CarResponseDto>();
                }
                else
                {
                    var car = await _carRepository.GetCarByIdAsync(request.CarId);
                    if (car == null)
                    {
                        throw new FileNotFoundException($"Car with Id: {request.CarId} does not exist.");
                    }
                    carResponseDto = car.Adapt<CarResponseDto>();
                    var addCarToCacheMessage = new AddCarToCacheMessage() { CarId = car.CarId , CarName = car.CarName, CarDescription = car.CarDescription, CarImage = car.CarImage };
                    await _publishEndpoint.Publish(addCarToCacheMessage);

                }
                return carResponseDto;
            }
            catch(FileNotFoundException ex)
            {
                _logger.LogWarning(400, ex, ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(500, ex, "GetCarByIdQueryHandler failed.");
                throw ex;
            }
        }
    }
}
