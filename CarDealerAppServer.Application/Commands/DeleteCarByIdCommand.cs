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
    public record DeleteCarByIdCommand(Guid CarId) : IRequest
    {
    }

    public class DeleteCarByIdCommandHandler : IRequestHandler<DeleteCarByIdCommand>
    {
        private readonly ILogger<DeleteCarByIdCommandHandler> _logger;
        private readonly ICarRepository _carRepository;
        public DeleteCarByIdCommandHandler(ILogger<DeleteCarByIdCommandHandler> logger, ICarRepository carRepository)
        {
            _logger = logger;
            _carRepository = carRepository;
        }
        public async Task Handle(DeleteCarByIdCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var car = await _carRepository.GetCarByIdAsync(request.CarId);
                if (car == null)
                {
                    throw new FileNotFoundException($"Car with Id: {request.CarId} does not exist.");
                }
                await _carRepository.DeleteCarByIdAsync(request.CarId);
            }
            catch (FileNotFoundException ex)
            {
                _logger.LogWarning(400, ex, ex.Message);
                throw ex;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(500, ex, "DeleteCarByIdCommandHandler failed.");
                throw ex;
            }
        }
    }
}
