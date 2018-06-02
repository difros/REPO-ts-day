using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GQ.Data.validators
{
    public class FunctionValidatorAttribute : ValidationAttribute
    {
        public delegate bool FunctionValidatorDelegate(object value, object ObjectInstance);

        private MethodInfo Function { get; set; }

        public FunctionValidatorAttribute(Type delegateType, string delegateName)
        {
            Function = delegateType.GetMethod(delegateName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = (bool)Function.Invoke(validationContext.ObjectInstance, new object[] { value, validationContext.ObjectInstance });

            if (!result)
            {
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName), new string[] { validationContext.DisplayName });
            }
            return null;
        }
    }
}
