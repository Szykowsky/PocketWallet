using AutoFixture;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using PocketWallet.Data;
using PocketWallet.Data.Models;
using PocketWallet.Helpers;
using PocketWallet.Services;
using PocketWallet.UnitTests.Configuration;
using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace PocketWallet.UnitTests.Services
{
    public class AuthServicesTests
    {
        private const string UserExistLogin = "Login";
        private const string UserNotExistLogin = "Login1";

        [Fact]
        public async Task Register_UserLoginExist()
        {
            //Arrange
            var users = GetFakeUsers();

            var usersMock = DbContextMock.CreateDbSetMock(users);
            var dbContextMock = new Mock<PasswordWalletContext>(DbContextMock.DummyOptions);
            dbContextMock.Setup(x => x.Users).Returns(usersMock.Object);

            var memoryCacheMock = new Mock<IMemoryCache>();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);

            var registerModel = GetFakeRegisterModel(UserExistLogin);
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
            var users = GetFakeUsers();

            var usersMock = DbContextMock.CreateDbSetMock(users);
            var dbContextMock = new Mock<PasswordWalletContext>(DbContextMock.DummyOptions);
            dbContextMock.Setup(x => x.Users).Returns(usersMock.Object);

            var memoryCacheMock = new Mock<IMemoryCache>();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var registerModel = GetFakeRegisterModel(UserNotExistLogin);
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
            var users = GetFakeUsers();

            var usersMock = DbContextMock.CreateDbSetMock(users);
            var dbContextMock = new Mock<PasswordWalletContext>(DbContextMock.DummyOptions);
            dbContextMock.Setup(x => x.Users).Returns(usersMock.Object);

            var memoryCacheMock = new Mock<IMemoryCache>();

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);

            var loginModel = GetFakeLoginModel(UserNotExistLogin);
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
            var users = GetFakeUsers();

            var usersMock = DbContextMock.CreateDbSetMock(users);
            var dbContextMock = new Mock<PasswordWalletContext>(DbContextMock.DummyOptions);
            dbContextMock.Setup(x => x.Users).Returns(usersMock.Object);

            var memoryCacheMock = new Mock<IMemoryCache>();
            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var loginModel = GetFakeLoginModel();
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
            var users = GetFakeUsers();

            var usersMock = DbContextMock.CreateDbSetMock(users);
            var dbContextMock = new Mock<PasswordWalletContext>(DbContextMock.DummyOptions);
            dbContextMock.Setup(x => x.Users).Returns(usersMock.Object);

            var memoryCacheMock = new Mock<IMemoryCache>();
            memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            var service = new AuthService(dbContextMock.Object, memoryCacheMock.Object);
            var loginModel = GetFakeLoginModel(UserExistLogin, "BadPassword");
            var cancellationToken = new CancellationToken();

            //Act
            var result = await service.Login(loginModel, cancellationToken);

            //Assert
            Assert.False(result.Success);
        }

        private IQueryable<User> GetFakeUsers()
        {
            var fixture = new Fixture();
            return new List<User>
            {
                fixture.Build<User>()
                    .With(u => u.Login, UserExistLogin)
                    .With(u => u.PasswordHash, HashHelper.SHA512("zdRpf^%f65V(0" + "testSALT" + "Password" ))
                    .With(u => u.Salt, "testSALT")
                    .Create(),
            }.AsQueryable();
        }

        private RegisterModel GetFakeRegisterModel(string login = UserExistLogin)
        {
            return new RegisterModel
            {
                Login = login,
                IsPasswordKeptAsHash = true,
                Password = "Password"
            };
        }

        private LoginModel GetFakeLoginModel(string login = UserExistLogin, string password = "Password")
        {
            return new LoginModel
            {
                Login = login,
                Password = password
            };
        }
    }
}
