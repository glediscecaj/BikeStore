using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikeStore.Models.Dtos
{
    public class UpsertBikeDTO
    {
        public string Model { get; set; }
        public string Make { get; set; }
        public string Color { get; set; }
        public BikeType Type { get; set; }
        public double Size { get; set; }
    }
}
