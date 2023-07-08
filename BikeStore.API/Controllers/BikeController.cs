using BikeStore.Application;
using BikeStore.Models;
using BikeStore.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace BikeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BikeController : ControllerBase
    {
        private readonly IBikeManager _bikeManager;

        public BikeController(IBikeManager bikeManager)
        {
            _bikeManager = bikeManager;
        }
        /// <summary>
        /// Takes bike dto and calls BikeManager method to create new Bike
        /// </summary>
        /// <param name="bike"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateBike(UpsertBikeDTO bike)
        {
            if (bike.Type != BikeType.City && bike.Type != BikeType.Mountain)
            {
                return BadRequest();
            }
            await _bikeManager.CreateNewBike(bike);

            return NoContent();
        }
        /// <summary>
        /// Calls BikeManager method and gets all the bikes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBikesList()
        {
            var result = await _bikeManager.GetBikes();

            return Ok(result);
        }
        /// <summary>
        /// Gets bikeId and calls BikeManager method and deletes bike with given Id
        /// </summary>
        /// <param name="bikeId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteBike(Guid bikeId)
        {
            var result = await _bikeManager.DeleteBike(bikeId);

            return Ok(result);
        }
        /// <summary>
        /// Gets bikeId and dto and calls BikeManager method to update bike
        /// </summary>
        /// <param name="bikeId"></param>
        /// <param name="bikeDto"></param>
        /// <returns></returns>
        [HttpPut("{bikeId}")]
        public async Task<IActionResult> UpdateBike(Guid bikeId, [FromBody] UpsertBikeDTO bikeDto)
        {
            var result = await _bikeManager.UpdateBike(bikeId, bikeDto);

            return Ok(result);
        }
        /// <summary>
        /// Gets bikeId and calls BikeManager method to gets Bike by given Id
        /// </summary>
        /// <param name="bikeId"></param>
        /// <returns></returns>
        [HttpGet("{bikeId}")]
        public async Task<IActionResult> GetBikeById(Guid bikeId)
        {
            var result = await _bikeManager.GetBikeById(bikeId);

            return Ok(result);
        }
    }
}
