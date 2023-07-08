using BikeStore.Application.PersistanceInterfaces;
using BikeStore.Models;
using BikeStore.Models.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BikeStore.Application
{
    public class BikeManager : IBikeManager
    {
        private readonly IJsonPersistanceService _jsonPersistanceService;
        private const string filePath = "bikes.json";

        public BikeManager(IJsonPersistanceService persistanceService)
        {
            _jsonPersistanceService = persistanceService;
        }
        /// <summary>
        /// This method calls the JsonPresitenceService method for creating bike and pass dto 
        /// and filePath to it
        /// </summary>
        /// <param name="bike"></param>
        /// <returns></returns>
        public async Task CreateNewBike(UpsertBikeDTO bike)
        {
            var bikeToSave = Bike.CreateBike(bike);
            await _jsonPersistanceService.CreateBike(bikeToSave, filePath);
        }
        /// <summary>
        /// This method is implemented for deleting bike. Firstly,gets bikeList 
        /// and checks if Bike with given Id exists in List, than removes it and saves file
        /// </summary>
        /// <param name="bikeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        /// 
        
        public async Task<bool> DeleteBike(Guid bikeId)
        {
            var bikes = await _jsonPersistanceService.GetAllBikes(filePath);

            var bikeToDelete = bikes.Where(x => x.Id == bikeId).FirstOrDefault();

            if (bikeToDelete is null)
            {
                throw new Exception($"Bike with id:{bikeId} does not exist");
            }

            bikes.Remove(bikeToDelete);
            try
            {
                await _jsonPersistanceService.SaveFile(bikes, filePath);
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }
        /// <summary>
        /// This method calls the JsonPresitenceService method for getting List of Bikes, 
        /// by passing filePath as parameter, than we get List od Bikes ordered by Model
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Bike>> GetBikes()
        {
            var bikes = await _jsonPersistanceService.GetAllBikes(filePath);
            if (bikes != null)
            {
                var bikesList = bikes.OrderBy(x => x.Model).ToList();
                return bikesList;
            }
            else
            {
                throw new Exception($"List is empty");
            }
        }
        /// <summary>
        /// This method calls the JsonPresitenceService method for updating bike and pass Id and dto to it. 
        /// Gets the list of bikes, and checks if bike with given Id exists in the list, 
        /// than updates the new data and saves file
        /// </summary>
        /// <param name="bikeId"></param>
        /// <param name="updateBike"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> UpdateBike(Guid bikeId, UpsertBikeDTO updateBike)
        {
            var bikes = await _jsonPersistanceService.GetAllBikes(filePath);

            if (bikes is null || bikes.Count == 0)
            {
                throw new Exception($"List is empty");
            }

            var bike = bikes.FirstOrDefault(x => x.Id == bikeId);

            if (bike is null)
            {
                throw new Exception($"Bike with id:{bikeId} does not exist");
            }
            Update(updateBike, bike);

            try
            {
                await _jsonPersistanceService.SaveFile(bikes, filePath);
                return true;

            }
            catch (Exception)
            {
                return false;

            }
        }
        /// <summary>
        /// This method calls the JsonPresitenceService method to get list of bikes 
        /// by passing filePath as parameter,then checks if bike with given id exists to it, and returns bike 
        /// </summary>
        /// <param name="bikeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Bike> GetBikeById(Guid bikeId)
        {
            var bikes = await _jsonPersistanceService.GetAllBikes(filePath);

            if (bikes is null || bikes.Count == 0)
            {
                throw new Exception($"List is empty");
            }

            var bike = bikes.FirstOrDefault(x => x.Id == bikeId);

            if (bike is null)
            {
                throw new Exception($"Bike with id:{bikeId} does not exist");
            }

            return bike;

        }

        #region PrivateMethods
        private void Update(UpsertBikeDTO createBikeDTO, Bike bike)
        {
            bike.Model = createBikeDTO.Model;
            bike.Make = createBikeDTO.Make;
            bike.Size = createBikeDTO.Size;
            bike.Color = createBikeDTO.Color;
            bike.Type = createBikeDTO.Type;
        }
        #endregion
    }
}
