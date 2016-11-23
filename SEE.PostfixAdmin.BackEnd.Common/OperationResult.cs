using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SEE.PostfixAdmin.BackEnd.Common
{
    public class ErrorItem
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class OperationResult
    {
        public List<ValidationResult> Errors { get; set; } = new List<ValidationResult>();

        public bool Succeeded
        {
            get
            {
                return Errors.Count() == 0;
            }
        }

        #region ctors
        public OperationResult()
        {
        }

        public OperationResult(List<ValidationResult> errors)
        {
            Errors = errors;
        }
        public OperationResult(ValidationResult error)
        {
            Errors.Add(error);
        }
        public OperationResult(string errorMessage)
            : base()
        {
            Errors.Add(new ValidationResult(errorMessage));
        }

        public OperationResult(Exception exception)
            : base()
        {
            Errors.Add(new ValidationResult(exception.Message, new string[] { "EXCEPTION" }));
        }

        #endregion

        public List<ErrorItem> GetAllErrors()
        {
            List<ErrorItem> result = new List<ErrorItem>();
            foreach (var err in Errors)
            {
                foreach (var mn in err.MemberNames)
                {
                    result.Add(new ErrorItem
                    {
                        ErrorMessage = err.ErrorMessage,
                        PropertyName = mn
                    });
                }
            }
            return result;
        }
    }

    public class OperationResult<T> : OperationResult
    {
        public T Value { get; set; }

        public OperationResult()
            :base()
        {

        }

        public OperationResult(T value)
            :base()
        {
            Value = value;
        }
        public OperationResult(List<ValidationResult> errors)
            :base(errors)
        {
        }
        public OperationResult(string errorMessage)
            : base(errorMessage)
        {
        }
        public OperationResult(Exception exception)
            : base(exception)
        {
        }
        public OperationResult(ValidationResult error)
            : base(error)
        {
        }
    }
}
