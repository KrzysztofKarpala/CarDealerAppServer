using CarDealerAppServer.Application.Contracts;
using CarDealerAppServer.Core.Entity;
using CarDealerAppServer.Core.Repository;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAppServer.Application.Consumers
{
    public class AddCarToCacheConsumer : IConsumer<AddCarToCacheMessage>
    {
        private readonly ILogger<AddCarToCacheConsumer> _logger;
        private readonly ICacheService _cacheService;
        public AddCarToCacheConsumer(ILogger<AddCarToCacheConsumer> logger, ICacheService cacheService)
        {
            _logger = logger;
            _cacheService = cacheService;
        }
        public async Task Consume(ConsumeContext<AddCarToCacheMessage> context)
        {
            try
            {
                var carResponse = new CarResponse() { CarId = context.Message.CarId, CarName = context.Message.CarName, CarDescription = context.Message.CarDescription, CarImage = context.Message.CarImage};
                await _cacheService.AddCarAsync(carResponse);
                _logger.LogInformation($"Car: {carResponse.CarId} added to cache");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(500, ex, "AddCarToCacheConsumer Error");
                throw;
            }
        }
    }
}
