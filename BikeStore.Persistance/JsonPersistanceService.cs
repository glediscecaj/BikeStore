using BikeStore.Application.PersistanceInterfaces;
using BikeStore.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BikeStore.Persistance
{
    public class JsonPersistanceService : IJsonPersistanceService
    {
        /// <summary>
        /// This method checks if the file exists. If it doesn't exist, it creates it and then adds a empty array
        /// to it. If the file exists we read its content, deserialize the array and then we add the data to it.
        /// Then we serialize and write the new array to the file.
        /// </summary>
        /// <param name="bikeToSave"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Task CreateBike(Bike bikeToSave, string filePath)
        {
            List<Bike> bikeList = new();

            if (!File.Exists(filePath))
            {
                JsonArray jsonArray = new();
                File.AppendAllText(filePath, JsonConvert.SerializeObject(jsonArray));
            }

            string fileContent = File.ReadAllText(filePath);

            bikeList = JsonConvert.DeserializeObject<List<Bike>>(fileContent);

            bikeList.Add(bikeToSave);
            var bikeListJson = JsonConvert.SerializeObject(bikeList);

            File.WriteAllText(filePath, bikeListJson);

            return Task.CompletedTask;
        }

        /// <summary>
        /// This method checks if file exists and if it does, reads the file and deserializes 
        /// the file it to a list of bikes, than returns that list
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Bike>> GetAllBikes(string filePath)
        {
            List<Bike> list = new();
            if (!File.Exists(filePath))
            {
                throw new Exception("File does not exist!");
            }

            string fileContent = File.ReadAllText(filePath);

            list = JsonConvert.DeserializeObject<List<Bike>>(fileContent);

            return list;
        }
        /// <summary>
        /// This method checks if file exists and if it does, serializes the list of bikes 
        /// into a json and than overwrites
        /// </summary>
        /// <param name="bikes"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task SaveFile(List<Bike> bikes, string filePath)
        {

            if (!File.Exists(filePath))
            {
                throw new Exception("File does not exist!");
            }

            var bikeListJson = JsonConvert.SerializeObject(bikes);

            File.WriteAllText(filePath, bikeListJson);

            return Task.CompletedTask;
        }
    }
}
