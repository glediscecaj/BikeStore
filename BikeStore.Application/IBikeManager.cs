using BikeStore.Models;
using BikeStore.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace BikeStore.Application
{
    public interface IBikeManager
    {
        Task CreateNewBike(UpsertBikeDTO bike);
        Task<bool> DeleteBike(Guid bikeId);
        Task<bool> UpdateBike(Guid bikeId, UpsertBikeDTO newBike);
        Task<Bike> GetBikeById(Guid bikeId);
        Task<List<Bike>> GetBikes();
    }
}
