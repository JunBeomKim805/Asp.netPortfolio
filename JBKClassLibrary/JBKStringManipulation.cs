using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace JBKClassLibrary
{
    public static class JBKStringManipulation
    {
        public static string JBKExtractDigits(string value)
        {
            string digit = "";

            foreach (var items in value)
            {
                if (char.IsDigit(items))
                    digit += items;            
            }
            value = digit;
            return value + "";
        }

        public static Boolean JBKPostalCodeIsValid(string postalCode,string regexPatteren)
        {
            if (string.IsNullOrEmpty(postalCode)) return true;
            Regex pattern = new Regex(@"^[a-z]\d[a-z] ?\d[a-z]\d$", RegexOptions.IgnoreCase);
            Regex pattern1 = new Regex(@"^\d{5}(?:-\d{4})?$");

            if (pattern.IsMatch(postalCode))
                return true;
            else if (pattern1.IsMatch(postalCode))
                return true;
            else
                return false;
        }
        public static string JBKCapitalize(string value)
        {
            if (string.IsNullOrEmpty(value))
                return (value + "");
            else
            {
                value = (value + "").Trim().ToLower();
                TextInfo capitalize = new CultureInfo("en-US", false).TextInfo;
                value = capitalize.ToTitleCase(value);
                return value;
                
            }
        }
    }
}
