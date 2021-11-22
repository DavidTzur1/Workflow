using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using WFBuilder.Models;

namespace WFBuilder.Validations
{
    public class VariableValTypeAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public VariableValTypeAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; } = null;

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ErrorMessage = ErrorMessageString;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
                throw new ArgumentException("Property with this name not found");
            var comparisonValue = (ValidationDataTypeEx)property.GetValue(validationContext.ObjectInstance);
            if(value == null)
            {
                return new ValidationResult(ErrorMessage);
            }

            ValidationContext valContext;
            var results = new List<ValidationResult>();
            List<ValidationAttribute> atributes;
            switch (comparisonValue)
            {
                
                case ValidationDataTypeEx.Integer:
                    atributes = new List<ValidationAttribute> { new RangeAttribute(int.MinValue,int.MaxValue) };
                    valContext = new ValidationContext(value, null, null) { };
                    
                    if(Validator.TryValidateValue(value, valContext, results, atributes))
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        ErrorMessage = results.FirstOrDefault().ToString();
                        return new ValidationResult(ErrorMessage);
                    }
                case ValidationDataTypeEx.Double:
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
                case ValidationDataTypeEx.Date:
                    atributes = new List<ValidationAttribute> { new RangeAttribute(typeof(DateTime),DateTime.MinValue.ToString(),DateTime.MaxValue.ToString()) };
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
                case ValidationDataTypeEx.Currency:
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
                case ValidationDataTypeEx.Boolean:
                    atributes = new List<ValidationAttribute> { new RangeAttribute(typeof(bool),"false","true") };
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

                case ValidationDataTypeEx.String:
                case ValidationDataTypeEx.Object:
                    return ValidationResult.Success;
                default:
                    return new ValidationResult(ErrorMessage);
            }

        }
    }
}
