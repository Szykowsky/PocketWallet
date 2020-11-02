using PocketWallet.Helpers;
using Xunit;

namespace PocketWallet.UnitTests.HashTests
{
    public class SymmectricEncryptorTests
    {
        [Theory]
        [InlineData("123124235345", "SuperSecretKey@11")]
        [InlineData("*&&^gshd%%$sad", "SuperSecretKey@11")]
        [InlineData("mnbmvgf%%%vbvbdsd+=", "SuperSecretKey@11")]
        public void GeneratedEncryptValueIsNotNull(string password, string secretkey)
        {
            var encryptedForm = SymmetricEncryptor.EncryptString(password, secretkey);
            Assert.NotNull(encryptedForm);
        }


        [Theory]
        [InlineData("123124235345", "SuperSecretKey@11")]
        [InlineData("*&&^gshd%%$sad", "SuperSecretKey@11")]
        [InlineData("mnbmvgf%%%vbvbdsd+=", "SuperSecretKey@11")]
        public void IsDecryptedFormSameAfterDecrypt(string password, string secretkey)
        {
            var encryptedValue = SymmetricEncryptor.EncryptString(password, secretkey);
            var decryptedValue = SymmetricEncryptor.DecryptToString(encryptedValue, secretkey);

            Assert.Equal(password, decryptedValue);
        }
    }
}
