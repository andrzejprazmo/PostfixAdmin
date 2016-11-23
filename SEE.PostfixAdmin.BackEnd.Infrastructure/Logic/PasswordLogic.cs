using SEE.PostfixAdmin.BackEnd.Common;
using SEE.PostfixAdmin.BackEnd.Common.Configuration;
using SEE.PostfixAdmin.BackEnd.Common.Contracts;
using SEE.PostfixAdmin.BackEnd.Common.Enums;
using SEE.PostfixAdmin.BackEnd.Infrastructure.Repository.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Infrastructure.Logic
{
    public class PasswordLogic
    {
        private readonly PasswordConfiguration _passwordConfiguration;

        protected PasswordLogic() { }

        private PasswordLogic(PasswordConfiguration passwordConfiguration)
        {
            _passwordConfiguration = passwordConfiguration;
        }

        public static PasswordLogic Create(PasswordConfiguration configuration) => new PasswordLogic(configuration);

        public virtual OperationResult Validate(PasswordContract password)
        {
            return AutoValidator<PasswordContract>.Validate(password, (c, errors) =>
            {
                if (string.Compare(c.Password, c.Confirm) != 0)
                {
                    errors.Add(new ValidationResult("Passwords are not the same.", new string[] { "Confirm" }));
                }
                if (!IsStrong(c.Password))
                {
                    errors.Add(new ValidationResult("Password is too weak.", new string[] { "Password" }));
                }
            });
        }

        public virtual string Hash(string rawPassword)
        {
            byte[] encrypted;
            PasswordEncoders encoder = (PasswordEncoders)Enum.Parse(typeof(PasswordEncoders), _passwordConfiguration.Encoder);
            switch (encoder)
            {
                default:
                case Common.Enums.PasswordEncoders.SHA1:
                    using (var sha = SHA1.Create())
                    {
                        encrypted = sha.ComputeHash(Encoding.ASCII.GetBytes(rawPassword));
                    }
                    break;
                case Common.Enums.PasswordEncoders.SHA256:
                    using (var sha = SHA256.Create())
                    {
                        encrypted = sha.ComputeHash(Encoding.ASCII.GetBytes(rawPassword));
                    }
                    break;
                case Common.Enums.PasswordEncoders.SHA512:
                    using (var sha = SHA512.Create())
                    {
                        encrypted = sha.ComputeHash(Encoding.ASCII.GetBytes(rawPassword));
                    }
                    break;
            }
            return Convert.ToBase64String(encrypted);
        }

        protected virtual bool IsStrong(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            int score = 1;

            if (password.Length >= _passwordConfiguration.MinLength) score++;
            if (_passwordConfiguration.Digit)
            {
                if (Regex.IsMatch(password, @"\d+"))
                {
                    score++;
                }
            }
            else
            {
                score++;
            }
            if (_passwordConfiguration.BigLetter)
            {
                if (Regex.IsMatch(password, @"[A-Z]"))
                {
                    score++;
                }
            }
            else
            {
                score++;
            }
            if (_passwordConfiguration.SpecialChar)
            {
                if (Regex.IsMatch(password, @"[!@#\$%\^&\*\?_~\-\(\);\.\+:]+"))
                {
                    score++;
                }
            }
            else
            {
                score++;
            }

            return score >= 5;
        }
    }
}
