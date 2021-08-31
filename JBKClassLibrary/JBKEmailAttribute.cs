using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;

namespace JBKClassLibrary
{
    public class JBKEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;
            if (value.ToString() == "") return ValidationResult.Success;
            try
            {
                MailAddress fred = new MailAddress(value.ToString());
                return ValidationResult.Success;
            }
            catch (Exception)
            {
                return new ValidationResult("is not  a correct email pattern");
            }
        }
    }
}
