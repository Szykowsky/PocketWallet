using PocketWallet.Services;
using PocketWallet.UnitTests.Configuration;
using System;
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
            var ipAddress = FakeModelsRepository.GetFakeIpAddress();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, ipAddress);
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
            var ipAddress = FakeModelsRepository.GetFakeIpAddress();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, ipAddress);
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
            var ipAddress = FakeModelsRepository.GetFakeIpAddress();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, ipAddress);
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
        public async Task Login_BlockFor5Second()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers(2, DateTime.Now, DateTime.Now, DateTime.Now.AddSeconds(5));
            var ipAddress = FakeModelsRepository.GetFakeIpAddress();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, ipAddress);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var loginModel = FakeModelsRepository.GetFakeLoginModel(FakeModelsRepository.UserExistLogin, "BadPassword");
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.Login(loginModel, cancellationToken);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Your account is block for 5 seconds", result.Messege);
        }

        [Fact]
        public async Task Login_BlockFor10Second()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers(3, DateTime.Now, DateTime.Now, DateTime.Now.AddSeconds(10));
            var ipAddress = FakeModelsRepository.GetFakeIpAddress();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, ipAddress);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var loginModel = FakeModelsRepository.GetFakeLoginModel(FakeModelsRepository.UserExistLogin, "BadPassword");
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.Login(loginModel, cancellationToken);

            //Assert
            Assert.False(result.Success);
            Assert.Equal("Your account is block for 10 seconds", result.Messege);
        }

        [Fact]
        public async Task Login_ShouldSignInAfterBlockFor10Second()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers(3, DateTime.Now, DateTime.Now, DateTime.Now.AddSeconds(-10));
            var ipAddress = FakeModelsRepository.GetFakeIpAddress();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, ipAddress);
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
        public async Task Login_IPBannedPermanently()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers(3, DateTime.Now, DateTime.Now, DateTime.Now.AddSeconds(-10));
            var ipAddress = FakeModelsRepository.GetFakeIpAddress("10.10.10.10", 4, true);
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, ipAddress);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var loginModel = FakeModelsRepository.GetFakeLoginModel();
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
