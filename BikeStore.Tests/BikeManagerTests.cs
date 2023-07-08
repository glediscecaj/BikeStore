using BikeStore.Application;
using BikeStore.Application.PersistanceInterfaces;
using BikeStore.Models;
using BikeStore.Models.Dtos;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;

namespace BikeStore.Tests
{
    public class BikeManagerTests
    {
        private readonly Mock<IJsonPersistanceService> _jsonPersistanceService = new();

        [Fact]
        public async Task GetAllBikes_WhenBikesExists_ReturnsBikeList()
        {
            // Arrange
            string filePath = "bikes.json";
            List<Bike> expectedBikes = new()
            {
            new Bike { Id = Guid.NewGuid(), Model= "Test1", Color = "Blue", Make = "Test1Make", Size = 22.2, Type = BikeType.City },
            new Bike { Id = Guid.NewGuid(), Model= "Test2", Color = "Blue2", Make = "Test12Make", Size = 23.2, Type = BikeType.Mountain },
        };

            _jsonPersistanceService.Setup(fs => fs.GetAllBikes(filePath)).ReturnsAsync(expectedBikes);

            var manager = new BikeManager(_jsonPersistanceService.Object);
            // Act
            List<Bike> actualBikes = await manager.GetBikes();

            // Assert
            actualBikes.Should().BeEquivalentTo(expectedBikes);
        }

        [Fact]
        public async Task GetAllBikes_WhenBikesIsNull_ReturnsException()
        {
            // Arrange
            string filePath = "bikes.json";
            List<Bike> expectedBikes = null;

            string fileContent = JsonConvert.SerializeObject(expectedBikes);

            _jsonPersistanceService.Setup(fs => fs.GetAllBikes(filePath)).ReturnsAsync(expectedBikes);

            var manager = new BikeManager(_jsonPersistanceService.Object);
            // Act
            Func<Task> act = manager.GetBikes;

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("List is empty");
        }

        [Fact]
        public async Task CreateNewBike_ShouldSaveBike()
        {
            // Arrange
            var filePath = "bikes.json";
            var bike = new UpsertBikeDTO
            {
                Color = "Blue",
                Make = "make1",
                Model = "model1",
                Size = 30.1,
                Type = BikeType.City
            };
            var bikeToSave = Bike.CreateBike(bike);

            var manager = new BikeManager(_jsonPersistanceService.Object);

            // Act
            await manager.CreateNewBike(bike);

            // Assert

            _jsonPersistanceService.Verify(
            x => x.CreateBike(It.IsAny<Bike>(), It.IsAny<string>()),
            Times.Once);

        }

        [Fact]
        public async Task DeleteBike_ShouldRemoveBike()
        {
            // Arrange
            var filePath = "bikes.json";
            var bikeId = Guid.NewGuid();
            List<Bike> bikes = new()
            {
            new Bike { Id = bikeId, Model= "Test1", Color = "Blue", Make = "Test1Make", Size = 22.2, Type = BikeType.City },
            new Bike { Id = Guid.NewGuid(), Model= "Test2", Color = "Blue2", Make = "Test12Make", Size = 23.2, Type = BikeType.Mountain },
        };

            _jsonPersistanceService.Setup(x => x.GetAllBikes(filePath)).ReturnsAsync(bikes);

            var manager = new BikeManager(_jsonPersistanceService.Object);

            // Act
            await manager.DeleteBike(bikeId);

            // Assert
            _jsonPersistanceService.Verify(
                x => x.SaveFile(It.Is<List<Bike>>(b => b.Count == 1 && b.First().Id != bikeId), It.Is<string>(f => f == filePath)),
                Times.Once);
        }

        [Fact]
        public async Task UpdateBike_ShouldUpdateBike()
        {
            // Arrange
            var filePath = "bikes.json";
            var bikeId = Guid.NewGuid();
            List<Bike> bikes = new()
            {
            new Bike { Id = bikeId, Model= "Test1", Color = "Blue", Make = "Test1Make", Size = 22.2, Type = BikeType.City },
            new Bike { Id = Guid.NewGuid(), Model= "Test2", Color = "Blue2", Make = "Test12Make", Size = 23.2, Type = BikeType.Mountain },
        };
            var updatedBike = new UpsertBikeDTO
            {
                Model = "Test1UU",
                Color = "BlueUU",
                Make = "Test1MakeUU",
                Size = 30,
                Type = BikeType.Mountain
            };

            _jsonPersistanceService.Setup(x => x.GetAllBikes(filePath)).ReturnsAsync(bikes);

            var manager = new BikeManager(_jsonPersistanceService.Object);

            // Act
            var result = await manager.UpdateBike(bikeId, updatedBike);

            // Assert
            result.Should().BeTrue();
            _jsonPersistanceService.Verify(
                x => x.SaveFile(It.Is<List<Bike>>(b => b.Count == bikes.Count && b.Any(bike => bike.Id == bikeId && CompareBikeProperties(bike, updatedBike))), It.Is<string>(f => f == filePath)),
                Times.Once);
        }

        [Fact]
        public async Task GetBike_WhenBikeExists_ReturnBike()
        {
            // Arrange
            var filePath = "bikes.json";
            var bikeId = Guid.NewGuid();
            List<Bike> bikes = new()
            {
            new Bike { Id = bikeId, Model= "Test1", Color = "Blue", Make = "Test1Make", Size = 22.2, Type = BikeType.City },
            new Bike { Id = Guid.NewGuid(), Model = "Test2", Color = "Blue2", Make = "Test12Make", Size = 23.2, Type = BikeType.Mountain },
        };

            _jsonPersistanceService.Setup(x => x.GetAllBikes(filePath)).ReturnsAsync(bikes);

            var manager = new BikeManager(_jsonPersistanceService.Object);

            // Act
            var bike = await manager.GetBikeById(bikeId);

            // Assert
            bike.Should().BeEquivalentTo(bikes.FirstOrDefault(x => x.Id == bikeId));
        }

        private bool CompareBikeProperties(Bike bike, UpsertBikeDTO updateBike)
        {
            return bike.Model == updateBike.Model &&
           bike.Make == updateBike.Make &&
           bike.Color == updateBike.Color &&
           bike.Type == updateBike.Type &&
           bike.Size == updateBike.Size;
        }
    }


}
