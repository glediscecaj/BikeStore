using BikeStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Application.PersistanceInterfaces
{
    public interface IJsonPersistanceService
    {
        Task CreateBike(Bike bikeToSave, string filePath);
        Task<List<Bike>> GetAllBikes(string filePath);
        Task SaveFile(List<Bike> bikes, string filePath);
    }
}
