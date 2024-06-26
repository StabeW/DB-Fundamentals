﻿using System.ComponentModel.DataAnnotations;

namespace BillsPaymentSystem.Models.Attribute
{
    [AttributeUsage(AttributeTargets.Property)]
    public class XorAttribute : ValidationAttribute
    {
        private string xorTargetAttribute;

        public XorAttribute(string xorTargetAttribute)
        {
            this.xorTargetAttribute = xorTargetAttribute;
        }

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var targetAttribute = validationContext.ObjectType
                .GetProperty(xorTargetAttribute)
                .GetValue(validationContext.ObjectInstance);

            if (targetAttribute == null && value != null ||
                (targetAttribute != null && value == null))
            {
                return ValidationResult.Success;
            }

            var errorMsg = "The two properties must have opposite values!";

            return new ValidationResult(errorMsg);
        }
    }
}
