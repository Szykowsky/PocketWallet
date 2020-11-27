using FluentValidation;
using PocketWallet.Data;
using PocketWallet.Data.Models;
using PocketWallet.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PocketWallet.Validators
{
    public class SignInValidator : AbstractValidator<LoginModel>
    {
        private readonly User _user;

        public SignInValidator(User user)
        {
            _user = user;

            RuleFor(x => x.Login)
                .Must(UserNotNull).WithMessage("Wrong login or password");

            When(x => user != null, () =>
            {
                RuleFor(x => x.Login)
                    .NotEmpty().WithMessage("Login cannot be empty!")
                    .NotNull().WithMessage("Login cannot be null")
                    .Must(BlockUserFor5Seconds).WithMessage("Your account is block for 5 seconds")
                    .Must(BlockFor10Seconds).WithMessage("Your account is block for 10 seconds");

                RuleFor(x => x.Password)
                    .NotEmpty().WithMessage("Password cannot be empty!")
                    .NotNull().WithMessage("Password cannot be null");

                RuleFor(x => x.IpAddress)
                    .NotEmpty().WithMessage("IpAddress cannot be empty!")
                    .NotNull().WithMessage("IpAddress cannot be null")
                    .Must(IsCorrectIp).WithMessage("Not correct ip address");
            });        
        }

        private bool IsCorrectIp(string ipaddress) => IPAddress.TryParse(ipaddress, out IPAddress ip);

        private bool UserNotNull(string login)
        {
            if(_user == null)
            {
                return false;
            }
            return true;
        }

        private bool BlockUserFor5Seconds(string login)
        {
            if (_user.InCorrectLoginCount > 1 && _user.InCorrectLoginCount <= 2 && _user.BlockLoginTo > DateTime.Now)
            {
                return false;
            }
            return true;
        }

        private bool BlockFor10Seconds(string login)
        {
            if (_user.InCorrectLoginCount > 2 && _user.InCorrectLoginCount >= 3 && _user.BlockLoginTo > DateTime.Now)
            {
                return false;
            }
            return true;
        }
    }
}
