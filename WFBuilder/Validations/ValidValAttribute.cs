using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WFBuilder.Validations
{
    public class ValidValAttribute : ValidationAttribute
    {
        private readonly string dataType;
        public ValidValAttribute(string dataType)
        {
            this.dataType = dataType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            if (value == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            var val = value.ToString();
            if (val.StartsWith("@") || val.StartsWith("::"))
            {
                if (MainWindow.Instance.VariablesGeneric(dataType).Where(x => x.Name == val).Count() > 0)
                    return ValidationResult.Success;
            }

            ValidationContext valContext;
            var results = new List<ValidationResult>();
            List<ValidationAttribute> atributes;
            switch (dataType)
            {
                case "Integer":
                    {
                        atributes = new List<ValidationAttribute> { new RangeAttribute(int.MinValue, int.MaxValue) };
                        valContext = new ValidationContext(value, null, null) { };

                        if (Validator.TryValidateValue(value, valContext, results, atributes))
                        {
                            return ValidationResult.Success;
                        }
                        else
                        {
                            ErrorMessage = results.FirstOrDefault().ToString();
                            return new ValidationResult(ErrorMessage);
                        }

                    }
                case "Double":
                    atributes = new List<ValidationAttribute> { new RangeAttribute(Double.MinValue, Double.MaxValue) };
                    valContext = new ValidationContext(value, null, null) { };
                    if (Validator.TryValidateValue(value, valContext, results, atributes))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        ErrorMessage = results.FirstOrDefault().ToString();
                        return new ValidationResult(ErrorMessage);
                    }
                case "Date":
                    atributes = new List<ValidationAttribute> { new RangeAttribute(typeof(DateTime), DateTime.MinValue.ToString(), DateTime.MaxValue.ToString()) };
                    valContext = new ValidationContext(value, null, null) { };
                    if (Validator.TryValidateValue(value, valContext, results, atributes))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        ErrorMessage = results.FirstOrDefault().ToString();
                        return new ValidationResult(ErrorMessage);
                    }
                case "Currency":
                    atributes = new List<ValidationAttribute> { new RegularExpressionAttribute(@"^\d+.?\d{0,2}$") };
                    valContext = new ValidationContext(value, null, null) { };
                    if (Validator.TryValidateValue(value, valContext, results, atributes))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        ErrorMessage = results.FirstOrDefault().ToString();
                        return new ValidationResult(ErrorMessage);
                    }
                case "Boolean":
                    atributes = new List<ValidationAttribute> { new RangeAttribute(typeof(bool), "false", "true") };
                    valContext = new ValidationContext(value, null, null) { };
                    if (Validator.TryValidateValue(value, valContext, results, atributes))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        ErrorMessage = results.FirstOrDefault().ToString();
                        return new ValidationResult(ErrorMessage);
                    }

                case "String":
                case "Object":
                    return ValidationResult.Success;
                default:
                    return new ValidationResult(ErrorMessage);
            }


            return ValidationResult.Success;
        }
    }
}
