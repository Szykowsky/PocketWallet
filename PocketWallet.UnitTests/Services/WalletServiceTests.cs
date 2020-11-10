using Microsoft.Extensions.Caching.Memory;
using Moq;
using PocketWallet.Helpers;
using PocketWallet.Services;
using PocketWallet.UnitTests.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PocketWallet.UnitTests.Services
{
    public class WalletServiceTests
    {
        [Fact]
        public async Task AddPassword_Successfull()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);

            object expectedValue = HashHelper.SHA512("zdRpf^%f65V(0" + "testSALT" + "Password");
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache(expectedValue);

            var service = new WalletService(dbContextMock.Object, memoryCacheMock.Object);
            var addPasswordModel = FakeModelsRepository.GetFakeAddNewPasswordModel();
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.AddNewPassowrd(addPasswordModel, FakeModelsRepository.UserExistLogin, cancellationToken);

            //Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task AddPassword_NotFoundCache()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache(null);

            var service = new WalletService(dbContextMock.Object, memoryCacheMock.Object);
            var addPasswordModel = FakeModelsRepository.GetFakeAddNewPasswordModel();
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.AddNewPassowrd(addPasswordModel, FakeModelsRepository.UserExistLogin, cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task AddPassword_NotFoundUser()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache(null);

            var service = new WalletService(dbContextMock.Object, memoryCacheMock.Object);
            var addPasswordModel = FakeModelsRepository.GetFakeAddNewPasswordModel();
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.AddNewPassowrd(addPasswordModel, FakeModelsRepository.UserNotExistLogin, cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetPassword_PasswordNotFound()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var passwords = FakeModelsRepository.GetFakePasswords();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, passwords);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache(null);

            var service = new WalletService(dbContextMock.Object, memoryCacheMock.Object);
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.GetPassword(new Guid("6a2c050a-29f8-4bcb-8a70-9bccc2d57aad"), 
                FakeModelsRepository.UserExistLogin, 
                cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task GetPassword_Success()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var passwords = FakeModelsRepository.GetFakePasswords();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, passwords);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache(null);

            var service = new WalletService(dbContextMock.Object, memoryCacheMock.Object);
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.GetPassword(new Guid(), FakeModelsRepository.UserExistLogin, cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeletePassword_PasswordNotFound()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var passwords = FakeModelsRepository.GetFakePasswords();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, passwords);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache(null);

            var service = new WalletService(dbContextMock.Object, memoryCacheMock.Object);
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.DeletePassword(new Guid("6a2c050a-29f8-4bcb-8a70-9bccc2d57aad"), cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        [Fact]
        public async Task DeletePassword_Success()
        {
            //Arrange
            var users = FakeModelsRepository.GetFakeUsers();
            var passwords = FakeModelsRepository.GetFakePasswords();
            var dbContextMock = MockInjectedServices.GetMockDbContext(users, passwords);
            var memoryCacheMock = MockInjectedServices.GetMockmemoryCache(null);

            var service = new WalletService(dbContextMock.Object, memoryCacheMock.Object);
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.DeletePassword(new Guid(), cancellationToken);

            //Assert
            Assert.False(result.Success);
        }
    }
}
