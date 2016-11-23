using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common
{
    public class AutoValidator<TContract>
    {
        public static OperationResult Validate(TContract contract)
        {
            var validationErrors = new List<ValidationResult>();
            Validator.TryValidateObject(contract, new ValidationContext(contract), validationErrors, true);
            return new OperationResult(validationErrors);
        }
        public static OperationResult Validate(TContract contract, Action<TContract, List<ValidationResult>> action)
        {
            var validationErrors = new List<ValidationResult>();
            Validator.TryValidateObject(contract, new ValidationContext(contract), validationErrors, true);
            if(action != null) action(contract, validationErrors);
            return new OperationResult(validationErrors);
        }
    }
}
