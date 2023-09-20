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
    public record GetCarsByNameQuery(string CarName) : IRequest<List<CarResponseDto>>
    {
    }

    public class GetCarsByNameQueryHandler : IRequestHandler<GetCarsByNameQuery, List<CarResponseDto>>
    {
        private readonly ILogger<GetCarsByNameQueryHandler> _logger;
        private readonly ICarRepository _carRepository;
        public GetCarsByNameQueryHandler(ILogger<GetCarsByNameQueryHandler> logger, ICarRepository carRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
        }
        public async Task<List<CarResponseDto>> Handle(GetCarsByNameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _carRepository.GetCarsByNameAsync(request.CarName);
                return res.Adapt<List<CarResponseDto>>();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(500, ex, "GetCarsByNameQueryHandler failed.");
                throw ex;
            }
        }
    }
}
