using System.Collections.Generic;
using System.Threading.Tasks;
using ProfileApi.Models;
using ProfileApi.Services;
using Xunit;
using ProfileApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileApi.Services.QueryService;
using Microsoft.Extensions.Logging;

namespace ProfileApi.Tests
{
    public class ProfileControllerTests
    {
        private readonly IQueryBuilder<Profile> _queryBuilder = new Mock<IQueryBuilder<Profile>>().Object;
        private readonly ILogger<ProfilesController> _logger = new Mock<ILogger<ProfilesController>>().Object;

        private Profile GetDummyProfile()
        {
            return new Profile
            {
                Id = "1",
                Email = "arjunshetty2020@gmail.com",
                Age = 16
            };
        }

        [Fact]
        public async Task GetProfiles_Returns404_IfNoProfileInRepo()//returing 200 ok makes sense since the request worked as expected and the DB did not have any profiles
        {
            // Arrange
            var mockProfileRepo = new Mock<IRepository<Profile>>();
            mockProfileRepo.Setup(i => i.Get()).ReturnsAsync(new List<Profile>());// repo returing empty list rather than null
            var controller = new ProfilesController(
                mockProfileRepo.Object, 
                _queryBuilder, 
                _logger
             );

            // Act
            var response = await controller.Get();
            var notFoundResult = Assert.IsType<NotFoundResult>(response);


            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetProfiles_Returns404_IfNullReturnedFromRepo()//since no resource found for this endpoint
        {
            // Arrange
            var mockProfileRepo = new Mock<IRepository<Profile>>();
            mockProfileRepo.Setup(i => i.Get()).ReturnsAsync((List<Profile>)null);
            var controller = new ProfilesController(
                mockProfileRepo.Object, 
                _queryBuilder, 
                _logger
             );

            // Act
            var response = await controller.Get();
            var notFoundResult = Assert.IsType<NotFoundResult>(response);

            // Assert
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetProfiles_Returns200_IfProfilesReturnedFromRepo()
        {
            // Arrange
            var mockProfileRepo = new Mock<IRepository<Profile>>();
            mockProfileRepo.Setup(i => i.Get()).ReturnsAsync(new List<Profile>{
                GetDummyProfile()
            });

            var controller = new ProfilesController(
                mockProfileRepo.Object, 
                _queryBuilder, 
                _logger
             );

            // Act
            var response = await controller.Get();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            var resultValue = Assert.IsType<List<Profile>>(okObjectResult.Value);
            Assert.NotNull(resultValue);
            Assert.Single(resultValue);
        }

        [Fact]
        public async Task GetProfiles_Returns404_IfProfileIdNotPresentInRepo()
        {
            // Arrange
            var idOfProfileToGet = "Some Id which is not present";
            var mockProfileRepo = new Mock<IRepository<Profile>>();
            mockProfileRepo.Setup(i => i.Get(idOfProfileToGet)).ReturnsAsync((Profile)null);

            var controller = new ProfilesController(
                mockProfileRepo.Object, 
                _queryBuilder, 
                _logger
             );

            // Act
            var response = await controller.Get(idOfProfileToGet);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(response); ;
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetProfiles_Returns200_IfProfileIdPresentInRepo()
        {
            // Arrange
            var dummyprofile = GetDummyProfile();
            var idOfProfileToGet = dummyprofile.Id;
            var mockProfileRepo = new Mock<IRepository<Profile>>();
            mockProfileRepo.Setup(i => i.Get(idOfProfileToGet)).ReturnsAsync(dummyprofile);

            var controller = new ProfilesController(
                mockProfileRepo.Object, 
                _queryBuilder, 
                _logger
             );

            // Act
            var response = await controller.Get(idOfProfileToGet);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(response);
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);

            var resultValue = Assert.IsType<Profile>(okObjectResult.Value);
            Assert.NotNull(resultValue);
            Assert.Equal(idOfProfileToGet, resultValue.Id);
        }

        [Fact]
        public async Task CreateProfile_Returns201_IfProfileCreatedInRepo()
        {
            // Arrange
            var dummmyProfile = GetDummyProfile();
            var mockProfileRepo = new Mock<IRepository<Profile>>();

            mockProfileRepo.Setup(i => i.Create(dummmyProfile)).ReturnsAsync(dummmyProfile);

            var controller = new ProfilesController(
                mockProfileRepo.Object, 
                _queryBuilder, 
                _logger
             );

            // Act
            var response = await controller.Create(dummmyProfile);

            // Assert
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(response); ;
            Assert.NotNull(createdAtRouteResult);
            Assert.Equal(201, createdAtRouteResult.StatusCode);

            var resultValue = Assert.IsType<Profile>(createdAtRouteResult.Value); ;
            Assert.NotNull(resultValue);
            Assert.Equal(dummmyProfile.Id, resultValue.Id);
        }

        [Fact]
        public async Task CreateProfile_Returns409_IfProfileCreationFailed()
        {
            // Arrange
            var dummmyProfile = GetDummyProfile();
            var mockProfileRepo = new Mock<IRepository<Profile>>();

            mockProfileRepo.Setup(i => i.Create(dummmyProfile)).ReturnsAsync((Profile)null);

            var controller = new ProfilesController(
                mockProfileRepo.Object, 
                _queryBuilder, 
                _logger
             );

            // Act
            var response = await controller.Create(dummmyProfile);

            // Assert
            var conflictResult = Assert.IsType<ConflictResult>(response);
            Assert.NotNull(conflictResult);
            Assert.Equal(409, conflictResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProfile_Returns200_IfProfileUpdateSuccessfully()
        {
            // Arrange
            var dummmyProfile = GetDummyProfile();
            var mockProfileRepo = new Mock<IRepository<Profile>>();

            mockProfileRepo.Setup(i => i.Update(dummmyProfile.Id, dummmyProfile)).Returns(Task.CompletedTask);

            var controller = new ProfilesController(
                mockProfileRepo.Object, 
                _queryBuilder, 
                _logger
             );

            // Act
            var response = await controller.Update(dummmyProfile.Id, dummmyProfile);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(response);
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProfile_Returns200_IfProfileDeletedSuccessfully()
        {
            // Arrange
            var dummmyProfile = GetDummyProfile();
            var mockProfileRepo = new Mock<IRepository<Profile>>();

            mockProfileRepo.Setup(i => i.Get(dummmyProfile.Id)).ReturnsAsync(dummmyProfile);
            mockProfileRepo.Setup(i => i.Remove(dummmyProfile.Id)).Returns(Task.CompletedTask);

            var controller = new ProfilesController(
                mockProfileRepo.Object, 
                _queryBuilder, 
                _logger
             );

            // Act
            var response = await controller.Delete(dummmyProfile.Id);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(response);
            Assert.NotNull(noContentResult);
            Assert.Equal(204, noContentResult.StatusCode);
        }

        [Fact]
        public async Task DeleteProfile_Returns404_IfProfileToBeDeletedIsNotThereInRepo()
        {
            // Arrange
            var dummmyProfile = GetDummyProfile();
            var mockProfileRepo = new Mock<IRepository<Profile>>();

            mockProfileRepo.Setup(i => i.Get("2")).ReturnsAsync(dummmyProfile);
            mockProfileRepo.Setup(i => i.Remove(dummmyProfile.Id)).Returns(Task.CompletedTask);

            var controller = new ProfilesController(
                mockProfileRepo.Object, 
                _queryBuilder, 
                _logger
             );

            // Act
            var response = await controller.Delete(dummmyProfile.Id);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(response);
            Assert.NotNull(notFoundResult);
            Assert.Equal(404, notFoundResult.StatusCode);
        }
    }
}