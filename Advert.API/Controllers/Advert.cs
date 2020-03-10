using Advert.API.Models;
using Advert.API.Models.Messages;
using Advert.API.Services;
using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Advert.API.Controllers
{
    [ApiController]
    [Route("api/v1/adverts")]
    public class Advert : ControllerBase
    {
        private readonly IAdvertStorageService _storageService;
        private readonly IConfiguration _configuration;
        public Advert(IAdvertStorageService storageService, IConfiguration configuration)
        {
            _storageService = storageService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(CreateAdvertResponse), 201)]
        public async Task<IActionResult> Create(AdvertModel model)
        {
            string recordId;
            try
            {
                recordId = await _storageService.Add(model);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return StatusCode(201, new CreateAdvertResponse() { Id = recordId });
        }

        [HttpPut]
        [Route("confirm")]
        [ProducesResponseType(404)]
        [ProducesResponseType(typeof(CreateAdvertResponse), 200)]
        public async Task<IActionResult> Confirm(ConfirmModel model)
        {
            try
            {
                await _storageService.Confirm(model);
                _ = RaiseAdvertConfirmedMessage(model);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return StatusCode(201, new CreateAdvertResponse() { Id = model.Id });
        }

        private async Task RaiseAdvertConfirmedMessage(ConfirmModel model)
        {
            var arn = _configuration.GetValue<string>("TopicARN");

            using (var client = new AmazonSimpleNotificationServiceClient())
            {
                var message = new AdvertConfirmedMessage()
                {
                    Id = model.Id,
                    Title = "Advert Model Confirmed"
                };
                _ = await client.PublishAsync(arn, System.Text.Json.JsonSerializer.Serialize(message));
            }
        }
    }
}
