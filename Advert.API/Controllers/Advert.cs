using Advert.API.Models;
using Advert.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Advert.API.Controllers
{
    [ApiController]
    [Route("api/v1/adverts")]
    public class Advert : ControllerBase
    {
        private readonly IAdvertStorageService _storageService;
        public Advert(IAdvertStorageService storageService)
        {
            _storageService = storageService;
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
    }
}
