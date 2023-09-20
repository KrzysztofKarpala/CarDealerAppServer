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
    public record GetCarsQuery() : IRequest<List<CarResponseDto>>
    {
    }

    public class GetCarsQueryHandler : IRequestHandler<GetCarsQuery, List<CarResponseDto>>
    {
        private readonly ILogger<GetCarsQueryHandler> _logger;
        private readonly ICarRepository _carRepository;
        public GetCarsQueryHandler(ILogger<GetCarsQueryHandler> logger, ICarRepository carRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
        }
        public async Task<List<CarResponseDto>> Handle(GetCarsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _carRepository.GetCarsAsync();
                return res.Adapt<List<CarResponseDto>>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(500, ex, "GetCarsQueryHandler failed.");
                throw ex;
            }
        }
    }
}
