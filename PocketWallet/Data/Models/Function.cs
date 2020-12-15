using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PocketWallet.Data.Models
{

    public static class FunctionName
    {
        public static class Auth
        {
            public static readonly string SignIn = "SignIn";
            public static readonly string SignUp = "SignUp";
            public static readonly string ChangeMasterPassword = "ChangeMasterPassword";
            public static readonly string GetLoginInfo = "GetLoginInfo";
        }

        public static class Wallet
        {
            public static readonly string AddPassword = "AddPassword";
            public static readonly string SharePassword = "SharePassword";
            public static readonly string GetWallet = "GetWallet";
            public static readonly string GetPassword = "GetPassword";
            public static readonly string GetFullSecurityPassword = "GetFullSecurityPassword";
            public static readonly string DeletePassword = "DeletePassword";
            public static readonly string EditPassword = "EditPassword";
        }
    }

    public class Function
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public static IEnumerable<Function> GetSeedData()
        {
            return new List<Function>
            {
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Auth.SignIn,
                    Description = "User sign in to application"
                },
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Auth.SignUp,
                    Description = "User create new account in application"
                },
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Auth.ChangeMasterPassword,
                    Description = "User change his master password"
                },
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Auth.GetLoginInfo,
                    Description = "User gets his sign in information (i.e successful login time)"
                },
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Wallet.AddPassword,
                    Description = "User add new password to his wallet"
                },
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Wallet.DeletePassword,
                    Description = "User delete password from his wallet"
                },
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Wallet.EditPassword,
                    Description = "User edit password in his wallet"
                },
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Wallet.GetFullSecurityPassword,
                    Description = "Get password record without password value"
                },
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Wallet.GetPassword,
                    Description = "Get password decrypted value"
                },
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Wallet.GetWallet,
                    Description = "Get all wallet"
                },
                new Function
                {
                    Id = Guid.NewGuid(),
                    Name = FunctionName.Wallet.SharePassword,
                    Description = "Share password record to other user"
                }
            };
        }
    }
}
