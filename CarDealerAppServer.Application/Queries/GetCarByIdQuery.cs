using CarDealerAppServer.Application.Dto;
using CarDealerAppServer.Core.Repository;
using Mapster;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public GetCarByIdQueryHandler(ILogger<GetCarByIdQueryHandler> logger, ICarRepository carRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
        }
        public async Task<CarResponseDto> Handle(GetCarByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _carRepository.GetCarByIdAsync(request.CarId);
                if(res == null) 
                {
                    throw new FileNotFoundException($"Car with Id: {request.CarId} does not exist.");
                }
                return res.Adapt<CarResponseDto>();
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
