using PocketWallet.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace PocketWallet.UnitTests.HashTests
{
    public class HashHelperTests
    {
        [Theory]
        [InlineData("123124235345", "SuperSecretKey@11")]
        [InlineData("*&&^gshd%%$sad", "SuperSecretKey@11")]
        [InlineData("mnbmvgf%%%vbvbdsd+=", "SuperSecretKey@11")]
        public void GenerateHMACSHA512HashIsNotNull(string password, string secretKey)
        {
            var result = HashHelper.HMACSHA512(password, secretKey);
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("123124235345")]
        [InlineData("*&&^gshd%%$sad")]
        [InlineData("mnbmvgf%%%vbvbdsd+=")]
        public void GenerateSHA512HashIsNotNull(string password)
        {
            var result = HashHelper.SHA512(password);
            Assert.NotNull(result);
        }
    }
}
