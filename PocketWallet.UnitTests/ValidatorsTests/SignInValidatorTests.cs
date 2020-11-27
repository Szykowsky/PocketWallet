using FluentValidation.TestHelper;
using PocketWallet.UnitTests.Configuration;
using PocketWallet.Validators;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PocketWallet.UnitTests.ValidatorsTests
{
    public class SignInValidatorTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Login_ShouldHaveError(string testValue)
        {
            var userLogin = FakeModelsRepository.GetFakeUser();
            var validator = new SignInValidator(userLogin);

            validator.ShouldHaveValidationErrorFor(model => model.Login, testValue);
        }

        [Theory]
        [InlineData("TestLogin")]
        [InlineData("Login@1235;<>{}%")]
        public void Login_ShouldNotHaveError(string testValue)
        {
            var userLogin = FakeModelsRepository.GetFakeUser();
            var validator = new SignInValidator(userLogin);

            validator.ShouldNotHaveValidationErrorFor(model => model.Login, testValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Password_ShouldHaveError(string testValue)
        {
            var userLogin = FakeModelsRepository.GetFakeUser();
            var validator = new SignInValidator(userLogin);

            validator.ShouldHaveValidationErrorFor(model => model.Password, testValue);
        }

        [Theory]
        [InlineData("TestLogin")]
        [InlineData("Login@1235;<>{}%")]
        public void Password_ShouldNotHaveError(string testValue)
        {
            var userLogin = FakeModelsRepository.GetFakeUser();
            var validator = new SignInValidator(userLogin);

            validator.ShouldNotHaveValidationErrorFor(model => model.Password, testValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("TestLogin")]
        [InlineData("Login@1235;<>{}%")]
        public void IpAddress_ShouldHaveError(string testValue)
        {
            var userLogin = FakeModelsRepository.GetFakeUser();
            var validator = new SignInValidator(userLogin);

            validator.ShouldHaveValidationErrorFor(model => model.IpAddress, testValue);
        }

        [Theory]
        [InlineData("10.0.10.12")]
        [InlineData("192.168.4.5")]
        [InlineData("2001:0db8:85a3:0000:0000:8a2e:0370:7334")]
        public void IpAddress_ShouldNotHaveError(string testValue)
        {
            var userLogin = FakeModelsRepository.GetFakeUser();
            var validator = new SignInValidator(userLogin);

            validator.ShouldNotHaveValidationErrorFor(model => model.IpAddress, testValue);
        }

        [Theory]
        [InlineData(2)]
        public void Login_BlockFor5Second(int count)
        {
            var userLogin = FakeModelsRepository.GetFakeUser(count, DateTime.Now, DateTime.Now, DateTime.Now.AddSeconds(6));
            var ipAddressValue = "192.168.4.5";
            var validator = new SignInValidator(userLogin);

            validator.ShouldHaveValidationErrorFor(model => model.Login, ipAddressValue);
        }

        [Theory]
        [InlineData(3)]
        public void Login_BlockFor10Second(int count)
        {
            var userLogin = FakeModelsRepository.GetFakeUser(count, DateTime.Now, DateTime.Now, DateTime.Now.AddSeconds(10));
            var ipAddressValue = "192.168.4.5";
            var validator = new SignInValidator(userLogin);

            validator.ShouldHaveValidationErrorFor(model => model.Login, ipAddressValue);
        }

        [Theory]
        [InlineData(1)]
        public void Login_AfterOneFailure(int count)
        {
            var ipAddressValue = "192.168.4.5";
            var userLogin = FakeModelsRepository.GetFakeUser(count);
            var validator = new SignInValidator(userLogin);

            validator.ShouldNotHaveValidationErrorFor(model => model.Login, ipAddressValue);
        }
    }
}
