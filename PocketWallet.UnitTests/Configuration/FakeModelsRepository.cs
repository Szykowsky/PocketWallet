using AutoFixture;
using PocketWallet.Data.Models;
using PocketWallet.Helpers;
using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace PocketWallet.UnitTests.Configuration
{
    public static class FakeModelsRepository
    {
        public const string UserExistLogin = "Login";
        public const string UserNotExistLogin = "Login1";

        public static IQueryable<User> GetFakeUsers()
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

        public static IQueryable<Password> GetFakePasswords()
        {
            var fixture = new Fixture();
            return new List<Password>
            {
                fixture.Build<Password>()
                    .With(u => u.Login, UserExistLogin)
                    .With(u => u.PasswordValue, HashHelper.SHA512("zdRpf^%f65V(0" + "testSALT" + "Password" ))
                    .With(u => u.UserId, new Guid())
                    .Create(),
            }.AsQueryable();
        }

        public static RegisterModel GetFakeRegisterModel(string login = UserExistLogin)
        {
            return new RegisterModel
            {
                Login = login,
                IsPasswordKeptAsHash = true,
                Password = "Password"
            };
        }

        public static LoginModel GetFakeLoginModel(string login = UserExistLogin, string password = "Password")
        {
            return new LoginModel
            {
                Login = login,
                Password = password
            };
        }

        public static ChangePasswordModel GetFakeChangePasswordModel(string login = UserExistLogin, string oldPassword = "Password", string newPassword = "PasswordNew")
        {
            return new ChangePasswordModel
            {
                Login = login,
                OldPassword = oldPassword,
                NewPassword = newPassword,
                IsPasswordKeptAsHash = true
            };
        }

        public static AddPasswordModel GetFakeAddNewPasswordModel(
            string login = UserExistLogin, 
            string pasword = "Password", 
            string description = "Description", 
            string webPage = "webpage.pl", 
            Guid userId = new Guid())
        {
            return new AddPasswordModel
            {
                Login = login,
                Password = pasword,
                Description = description,
                WebPage = webPage,
                UserId = userId,
            };
        }
    }
}
