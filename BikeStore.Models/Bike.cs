using BikeStore.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Models
{
    public class Bike
    {
        public Guid Id { get; set; }
        public string Model { get; set; }
        public string Make { get; set; }
        public string Color { get; set; }
        public BikeType Type { get; set; }
        public double Size { get; set; }

        public static Bike CreateBike(UpsertBikeDTO dto)
        {
            return new Bike()
            {
                Id = Guid.NewGuid(),
                Model = dto.Model,
                Make = dto.Make,
                Color = dto.Color,
                Type = dto.Type,
                Size = dto.Size,
            };
        }
    }

    public enum BikeType
    {
        City,
        Mountain
    }
}
