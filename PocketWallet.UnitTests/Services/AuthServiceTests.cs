using PocketWallet.Services;
using PocketWallet.UnitTests.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PocketWallet.UnitTests.Services
{
    public class AuthServiceTests
    {
        [Fact]
        public async Task Register_UserLoginExist()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);

            var registerModel = FakeModelsRepository.GetFakeRegisterModel(FakeModelsRepository.UserExistLogin);
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.Register(registerModel, cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Register_NewLogin()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var registerModel = FakeModelsRepository.GetFakeRegisterModel(FakeModelsRepository.UserNotExistLogin);
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.Register(registerModel, cancellationToken);

            //Assert
            Assert.True(result.Success);
            Assert.Equal("Succesfully sign up", result.Messege);
        }

        [Fact]
        public async Task Login_UserLoginNotExist()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);

            var loginModel = FakeModelsRepository.GetFakeLoginModel(FakeModelsRepository.UserNotExistLogin);
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.Login(loginModel, cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task Login_Successful()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var loginModel = FakeModelsRepository.GetFakeLoginModel();
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.Login(loginModel, cancellationToken);

            //Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Login_BadPassword()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var loginModel = FakeModelsRepository.GetFakeLoginModel(FakeModelsRepository.UserExistLogin, "BadPassword");
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.Login(loginModel, cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ChangePassword_UserNotExist()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var changePasswordModel = FakeModelsRepository.GetFakeChangePasswordModel(FakeModelsRepository.UserNotExistLogin);
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.ChangePassword(changePasswordModel, cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ChangePassword_BadOldPassword()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var changePasswordModel = FakeModelsRepository.GetFakeChangePasswordModel(FakeModelsRepository.UserExistLogin, "BadOldPassword");
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.ChangePassword(changePasswordModel, cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task ChangePassword_Succesfull()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var changePasswordModel = FakeModelsRepository.GetFakeChangePasswordModel();
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.ChangePassword(changePasswordModel, cancellationToken);

            //Assert
            Assert.True(result.Success);
        }
    }
}
