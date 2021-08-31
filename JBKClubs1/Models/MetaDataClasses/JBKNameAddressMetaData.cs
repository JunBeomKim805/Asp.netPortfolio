using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace JBKClubs1.Models
{
    [ModelMetadataType(typeof(JBKNameAddressMetaData))]
    public partial class NameAddress : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            ClubsContext _context = new ClubsContext();
            FirstName = (FirstName + "").Trim();
            LastName = (LastName+ "").Trim();
            CompanyName = (CompanyName+ "").Trim();
            StreetAddress = (StreetAddress+ "").Trim();
            City = (City+ "").Trim();
            PostalCode = (PostalCode+ "").Trim();
            ProvinceCode = (ProvinceCode+ "").Trim();
            Email = (Email+ "").Trim();
            Phone = (Phone+ "").Trim();
            FirstName = JBKClassLibrary.JBKStringManipulation.JBKCapitalize(FirstName);
            LastName = JBKClassLibrary.JBKStringManipulation.JBKCapitalize(LastName);
            CompanyName = JBKClassLibrary.JBKStringManipulation.JBKCapitalize(CompanyName);
            StreetAddress = JBKClassLibrary.JBKStringManipulation.JBKCapitalize(StreetAddress);
            City = JBKClassLibrary.JBKStringManipulation.JBKCapitalize(City);
            Phone = JBKClassLibrary.JBKStringManipulation.JBKExtractDigits(Phone);
            if(String.IsNullOrEmpty(FirstName)&&string.IsNullOrEmpty(LastName)&&string.IsNullOrEmpty(CompanyName))
                yield return new ValidationResult
                    ("at least one of first name,last name or company name must be provided", new[] { nameof(FirstName), nameof(LastName), nameof(CompanyName) });
            if (ProvinceCode != "")
            {
                ProvinceCode = (ProvinceCode + "").Trim().ToUpper();
                Province province = _context.Province.Find(ProvinceCode);
                if (province == null)
                    yield return new ValidationResult
                        ("Province code is not on file", new[] { nameof(ProvinceCode) });
            }
            if (PostalCode != "")
            {
                ProvinceCode = (ProvinceCode + "").Trim().ToUpper();
                PostalCode = (PostalCode + "").Trim().ToUpper();
                Province province = _context.Province.Find(ProvinceCode);
                if (province != null) 
                { 
                    Country country = _context.Country.Find(province.CountryCode);
                    if (JBKClassLibrary.JBKStringManipulation.JBKPostalCodeIsValid(PostalCode, country.PostalPattern))
                    {

                        //add space, if not there already
                        // see if the first letter of postal is in province's firstletterofpostal
                        if (province.CountryCode == "CA")
                        {
                            if (Convert.ToInt32(PostalCode.Count()) == 6)
                            {
                                string first = PostalCode.Substring(0, 3);
                                string last = PostalCode.Substring(3, 3);
                                PostalCode = first + " " + last;
                            }
                            string firstLetter = PostalCode.Substring(0, 1);
                            if (!province.FirstPostalLetter.Contains(firstLetter))
                                yield return new ValidationResult("Postal code is not a valid pattern for the province", new[] { nameof(PostalCode) });
                        }
                    }
                    else
                    {
                        yield return new ValidationResult
                            ("Postal code is not a valid pattern for the country", new[] { nameof(PostalCode) });
                    }
                }
                if (province==null||string.IsNullOrEmpty(ProvinceCode))
                    yield return new ValidationResult
                        ("postal code is wrong for the given province", new[] { nameof(PostalCode) });


            }
            if (string.IsNullOrEmpty(Email))
            {
                if (string.IsNullOrEmpty(City) || string.IsNullOrEmpty(StreetAddress) || string.IsNullOrEmpty(PostalCode) || string.IsNullOrEmpty(ProvinceCode))
                    yield return new ValidationResult("all postal information is required if email is  not provided", new[] { nameof(Email) });
            }
            if (Phone.Length != 10)
            {
                yield return new ValidationResult("phone must  have exactly 10 digits", new[] { nameof(Phone) });
            }
            else
            {
                string first = Phone.Substring(0, 3);
                string middle = Phone.Substring(3, 3);
                string last = Phone.Substring(6, 4);
                Phone = first + "-" +middle+"-" +last;
            }



            yield return ValidationResult.Success;
        }
    }
    public class JBKNameAddressMetaData
    {
        public int NameAddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string ProvinceCode { get; set; }
        [JBKClassLibrary.JBKEmail]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
    }
}
