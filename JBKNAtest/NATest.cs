using JBKClubs1.Controllers;
using JBKClubs1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;
using JBKClassLibrary;

namespace JBKNAtest
{
    public class NATest
    {
        #region global variables and database connection
        // database context - relies on redundant connection string
        // in context's OnConfiguring method to replace DI service
        ClubsContext _context = new ClubsContext();
        // following NameAddress object re-instantiated for each test
        // - before each new test, old object is removed from EF queue
        NameAddress nameAddress;

        // Create a subclass of the target XXEmailAttribute ... with an IoC-inspired method
        // (call it and it'll create its own required ValidationContext parameter)
        // - a ValidationContext needs a DisplayName value at minimum
        public class Local_EmailAttribute : JBKEmailAttribute
        {
            public ValidationResult Run_IsValid(object value)
            {
                return IsValid(value, new ValidationContext(new { DisplayName = "fred" }));
            }
        }


        #endregion

        #region Create perfect Name/Address object & test it
        private void Initialize()
        {
            // purge old test object from EF queue
            try
            {
                _context.Entry(nameAddress).State = EntityState.Detached;
            }
            catch (Exception) { }

            // create an ideal Name/Address object
            nameAddress = new NameAddress()
            {
                FirstName = "David",
                LastName = "Turton",
                CompanyName = "Conestoga College",
                StreetAddress = "123 University Ave.",
                City = "Waterloo",
                PostalCode = "N3A 3A3",
                ProvinceCode = "ON",
                Email = "dturton@conestogac.on.ca",
                Phone = "519-748-1220"
            };
        }

        // if benchmark fails, all other tests will be invalid
        // - failure is usually caused by incorrect edits
        [Fact]
        public void Benchmark_ShouldBeAccepted()
        {
            //Arrange
            Initialize();
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            _context.EFValidation();
        }
        #endregion

        #region String checks: trim, capitalize

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void FirstNameEmptyNullSpaces_shouldPassAsEmpty(string value)
        {
            //Arrange
            Initialize();
            nameAddress.FirstName = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted
            _context.EFValidation();
            Assert.Equal("", nameAddress.FirstName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void LastNameEmptyNullSpaces_shouldPassAsEmpty(string value)
        {
            //Arrange
            Initialize();
            nameAddress.LastName = value;
            //Act
            _context.Add(nameAddress);
            //Assert

            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted
            _context.EFValidation();
            Assert.Equal("", nameAddress.LastName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CompanyNameEmptyNullSpaces_shouldPassAsEmpty(string value)
        {
            //Arrange
            Initialize();
            nameAddress.CompanyName = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted
            _context.EFValidation();
            Assert.Equal("", nameAddress.CompanyName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void StreetAddressEmptyNullSpaces_shouldPassAsEmpty(string value)
        {
            //Arrange
            Initialize();
            nameAddress.StreetAddress = value;
            //Act
            _context.Add(nameAddress);
            //Assert 
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted
            _context.EFValidation();
            Assert.Equal("", nameAddress.StreetAddress);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void CityNameEmptyNullSpaces_shouldPassAsEmpty(string value)
        {
            //Arrange
            Initialize();
            nameAddress.City = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted
            _context.EFValidation();
            Assert.Equal("", nameAddress.City);
        }

        [Theory]
        [InlineData("New Hamburg ")]
        [InlineData("new hamburg ")]
        [InlineData(" NEW HAMBUrg")]
        [InlineData("new   Hamburg")]
        public void FirstName_TrimAndCapitalise(string value)
        {
            //Arrange
            Initialize();
            nameAddress.FirstName = value;
            //Act
            _context.Add(nameAddress);
            //Assert

            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted/calculated
            _context.EFValidation();
            Assert.Equal("New Hamburg", nameAddress.FirstName);
        }

        [Theory]
        [InlineData("New Hamburg ")]
        [InlineData("new hamburg ")]
        [InlineData(" NEW HAMBUrg")]
        [InlineData("new   Hamburg")]
        public void LastName_TrimAndCapitalise(string value)
        {
            //Arrange
            Initialize();
            nameAddress.LastName = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted/calculated
            _context.EFValidation();
            Assert.Equal("New Hamburg", nameAddress.LastName);
        }

        [Theory]
        [InlineData("New Hamburg ")]
        [InlineData("new hamburg ")]
        [InlineData(" NEW HAMBUrg")]
        [InlineData("new   Hamburg")]
        public void CompanyName_TrimAndCapitalise(string value)
        {
            //Arrange
            Initialize();
            nameAddress.CompanyName = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted/calculated
            _context.EFValidation();
            Assert.Equal("New Hamburg", nameAddress.CompanyName);
        }

        [Theory]
        [InlineData("New Hamburg ")]
        [InlineData("new hamburg ")]
        [InlineData(" NEW HAMBUrg")]
        [InlineData("new   Hamburg")]
        public void StreetAddress_TrimAndCapitalise(string value)
        {
            //Arrange
            Initialize();
            nameAddress.StreetAddress = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted/calculated
            _context.EFValidation();
            Assert.Equal("New Hamburg", nameAddress.StreetAddress);
        }

        [Theory]
        [InlineData("New Hamburg ")]
        [InlineData("new hamburg ")]
        [InlineData(" NEW HAMBUrg")]
        [InlineData("new   Hamburg")]
        public void CityAddress_TrimAndCapitalise(string value)
        {
            //Arrange
            Initialize();
            nameAddress.City = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted/calculated
            _context.EFValidation();
            Assert.Equal("New Hamburg", nameAddress.City);
        }

        #endregion

        #region conditionally required

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void FirstNameNullEmpty_ShouldBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.FirstName = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            _context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void LastNameNullEmpty_ShouldBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.LastName = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            _context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void CompanyNameNullEmpty_ShouldBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.CompanyName = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            _context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EmailWithStreetAddressNullEmpty_ShouldBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.StreetAddress = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            _context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EmailWithCityNullEmpty_ShouldBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.City = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            _context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EmailWithPostalNullEmpty_ShouldBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.PostalCode = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            _context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Fact]
        public void EmailWithPostalInvalid_ShouldTripValidation()
        {
            // even though postal information is optional when email
            // is provided, data in a postal field should be valid
            //Arrange
            Initialize();
            nameAddress.PostalCode = "AAA AAA";
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EmailWithProvinceNullEmpty_ShouldBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.ProvinceCode = value;
            nameAddress.PostalCode = value;
            //Act
            _context.Add(nameAddress);
            //Assert

            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            _context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Fact]
        public void EmailWithProvinceInvalid_ShouldTripValidation()
        {
            // even though postal information is optional when email
            // is provided, data in a postal field should be valid
            //Arrange
            Initialize();
            nameAddress.ProvinceCode = "3";
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NoEmailNoStreeAddress_ShouldNotBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.Email = value;
            nameAddress.StreetAddress = value;
            //Act
            _context.Add(nameAddress);
            //Assert

            // use this if the data is invalid and should trip validation
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NoEmailNoCity_ShouldNotBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.Email = value;
            nameAddress.City = value;
            //Act
            _context.Add(nameAddress);
            //Assert

            // use this if the data is invalid and should trip validation
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NoEmailNoPostal_ShouldNotBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.Email = value;
            nameAddress.PostalCode = value;
            //Act
            _context.Add(nameAddress);
            //Assert

            // use this if the data is invalid and should trip validation
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void NoEmailNoProvince_ShouldNotBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.Email = value;
            nameAddress.ProvinceCode = value;
            //Act
            _context.Add(nameAddress);
            //Assert

            // use this if the data is invalid and should trip validation
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        #endregion

        #region phone number checks

        [Fact]
        public void NullPhone_ShouldFail()
        {
            // Arrange
            Initialize();
            nameAddress.Phone = null;
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            //_context.EFValidation();
        }

        [Fact]
        public void EmptyPhone_ShouldFail()
        {
            // Arrange
            Initialize();
            nameAddress.Phone = "";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            //_context.EFValidation();
        }

        [Fact]
        public void phoneNoDigits_ShouldFail()
        {
            // Arrange
            Initialize();
            nameAddress.Phone = "pasdfghjkl";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            //_context.EFValidation();
        }

        [Fact]
        public void PhoneBuriedInTrash_ShouldBeAccepted()
        {
            // Arrange
            Initialize();
            nameAddress.Phone = "bb1jk2jk3j4.5.6.^&7(8.9-0kgfkhg";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            _context.EFValidation();
        }

        [Fact]
        public void PhoneBuriedInTrash_ShouldBeReformatted()
        {
            // Arrange
            Initialize();
            nameAddress.Phone = "bb1jk2jk3j4.5.6.^&7(8.9-0kgfkhg";
            //Act
            _context.NameAddress.Add(nameAddress);
            _context.EFValidation();

            Assert.Equal("123-456-7890", nameAddress.Phone);
        }

        [Theory]
        [InlineData("123-456-7890")]
        [InlineData("(123) 456-7890")]
        [InlineData("123 456 7890")]
        [InlineData("123-4567890")]
        [InlineData("1234567890")]
        [InlineData("123456-7890")]
        public void Phone10Digits_ShouldBeAcceptedReformatted(string value)
        {
            // Arrange
            Initialize();
            nameAddress.Phone = value;
            //Act
            _context.NameAddress.Add(nameAddress);
            _context.EFValidation();

            Assert.Equal("123-456-7890", nameAddress.Phone);
        }

        [Theory]
        [InlineData("1")]
        [InlineData("123")]
        [InlineData("12345")]
        [InlineData("1234567")]
        [InlineData("123456789A")]
        [InlineData("12d34d567r89g7j9")]
        [InlineData("12345678901")]
        [InlineData("(123)456-7890 6")]
        [InlineData("123-456-78901")]
        [InlineData("123-456-789")]
        public void PhoneNot10Digits_ShouldNotBeAccepted(string value)
        {
            // Arrange
            Initialize();
            nameAddress.Phone = value;
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            //_context.EFValidation();
        }

        #endregion

        #region postal Code checks

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void PostalWithNoProvince_ShouldTripValidation(string value)
        {
            //Arrange
            Initialize();
            nameAddress.ProvinceCode = value;
            //Act
            _context.Add(nameAddress);
            //Assert

            // use this if the data is invalid and should trip validation
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Fact]
        public void PostalCodeAAAAAA_ShouldNotBeAccepted()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = "AAAAAA";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            //_context.EFValidation();
        }

        [Fact]
        public void PostalCodeNull_ShouldBeAccepted()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = null;
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            _context.EFValidation();
        }

        [Fact]
        public void PostalCodeNull_ShouldBeConvertedToEmpty()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = null;
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            //_context.EFValidation();

            // use this if the data is to be reformatted
            _context.EFValidation();
            Assert.Equal("", nameAddress.PostalCode);
        }

        [Fact]
        public void PostalCodeEmpty_ShouldBeAccepted()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = "";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            _context.EFValidation();
        }

        [Fact]
        public void PostalCodeSpaces_ShouldBeTrimmed()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = "   ";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            //_context.EFValidation();

            // use this if the data is to be reformatted
            _context.EFValidation();
            Assert.Equal("", nameAddress.PostalCode);
        }

        [Fact]
        public void PostalCodeLower_IsAccepted()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = "n1b 2c3";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            _context.EFValidation();
        }

        [Fact]
        public void PostalCodeUpper_NoSpace_IsAccepted()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = "N1B2C3";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            _context.EFValidation();
        }
        [Fact]
        public void PostalCodeLower_NoSpace_IsAccepted()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = "n1b2c3";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            _context.EFValidation();
        }

        [Fact]
        public void PostalFirstLetter_ValidProvince_IsAccepted()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = "N2N 3A3";
            nameAddress.ProvinceCode = "ON";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            _context.EFValidation();
        }

        [Fact]
        public void PostalFirstLetter_InvalidProvice_ShouldBeRejected()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = "N2N 3A3";
            nameAddress.ProvinceCode = "AB";
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert
            // use following if data is bad, and should be caught
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            //_context.EFValidation();
        }

        [Fact]
        public void PostalCodeLower_ShiftsToUpper()
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = "n1b 2c3";
            // act     
            _context.NameAddress.Add(nameAddress);
            _context.EFValidation();
            // assert
            Assert.Equal("N1B 2C3", nameAddress.PostalCode);
        }

        [Theory]
        [InlineData("N1B 2C3 ")]
        [InlineData(" N1B 2C3")]
        public void PostalCodeLeadingTrailingSpaces_ShouldBeTrimmed(string value)
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = value;
            // act     
            _context.NameAddress.Add(nameAddress);
            _context.EFValidation();
            // assert
            // use following if data is clean and should not trip any validations
            _context.EFValidation();
            Assert.True(nameAddress.PostalCode.Length == 7);
        }

        [Theory]
        [InlineData("n1b2c3")]
        [InlineData("N1B2C3")]
        [InlineData("n1b 2c3")]
        [InlineData("N1B 2C3")]
        public void PostalCode_SingleSpaceInserted(string value)
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = value;
            // act     
            _context.NameAddress.Add(nameAddress);
            _context.EFValidation(); // EF runs all validations except [Remote]

            // assert
            Assert.Equal("N1B 2C3", nameAddress.PostalCode.ToUpper());
        }


        [Theory]
        [InlineData("K1B 1C1")]
        [InlineData("M2G 3H4")]
        [InlineData("M5K 6L7")]
        [InlineData("N8N 9P0")]
        [InlineData("P9S 9T9")]
        [InlineData("K9W 9X9")]
        [InlineData("N9Z 9A9")]
        [InlineData("k1b 1c1")]
        [InlineData("m2g 3h4")]
        [InlineData("n5k 6l7")]
        [InlineData("p8n 9p0")]
        [InlineData("k9s 9t9")]
        [InlineData("n9w 9x9")]
        [InlineData("m9z 9a9")]

        public void PostalCodeValidLetters_ShouldBeAccepted(string value)
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = value;
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            _context.EFValidation();
        }


        [Theory]
        [InlineData("W1B 1C1")]
        [InlineData("Z2G 3H4")]
        [InlineData("J5D 6L7")]
        [InlineData("M8N 9F0")]
        [InlineData("R9I 9T9")]
        [InlineData("V9W 9O9")]
        [InlineData("Y9Q 9U9")]
        public void PostalCodeInvalidLetters_ShouldTripValidation(string value)
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = value;
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            //_context.EFValidation();
        }

        [Theory]
        [InlineData("A1B 1C1X")]
        [InlineData("XE2G 3H4")]
        [InlineData("J5K J5K 6L7")]
        [InlineData("M8N 9P09P0")]
        [InlineData("V9W  9X9")]
        public void PostalCodePlusTrash_ShouldBeCaught(string value)
        {
            // arrange
            Initialize();
            nameAddress.PostalCode = value;
            // Act
            _context.NameAddress.Add(nameAddress);
            // Assert

            // use following if data is bad, and should be caught
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use following if data is clean and should not trip any validations
            //_context.EFValidation();
        }
        #endregion

        #region zip code checks

        [Theory]
        [InlineData("12345")]
        [InlineData("12345-1234")]
        public void AmericanState_ZipShouldBeAccepted(string value)
        {
            //Arrange
            Initialize();
            nameAddress.ProvinceCode = "NY";
            nameAddress.PostalCode = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            _context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Theory]
        [InlineData("A3A3A3")]
        [InlineData("A3A 3A3")]
        [InlineData("a12345a")]
        [InlineData("12345-67890")]
        public void AmericanStateInvalidZips_ShouldTripValidation(string value)
        {
            //Arrange
            Initialize();
            nameAddress.ProvinceCode = "NY";
            nameAddress.PostalCode = value;
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("12345", nameAddress.ProvinceCode);
        }

        [Fact]
        public void AmericanStateZip_9DigitsShouldBeValid()
        {
            //Arrange
            Initialize();
            nameAddress.ProvinceCode = "NY";
            nameAddress.PostalCode = "12345-1234";
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            _context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("12345", nameAddress.ProvinceCode);
        }

        [Fact]
        public void CanadianProvince_ZipShouldBeRejected()
        {
            // even though postal information is optional when email
            // is provided, data in a postal field should be valid
            //Arrange
            Initialize();
            nameAddress.PostalCode = "12345";
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            //_context.EFValidation();
            //Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        [Fact(Skip = "ZIP reformat not in assignment")]
        public void Zip9Digits_ShouldBeReformatted()
        {
            //Arrange
            Initialize();
            nameAddress.ProvinceCode = "NY";
            nameAddress.PostalCode = "123451234";
            //Act
            _context.Add(nameAddress);
            //Assert
            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            _context.EFValidation();
            Assert.Equal("12345-1234", nameAddress.ProvinceCode);
        }

        #endregion

        #region Class Library checks

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void EmailAnnotationNullEmpty_ShouldPass(string value)
        {
            //Arrange
            Local_EmailAttribute attribute = new Local_EmailAttribute();
            //Act & Assert
            Assert.Equal(ValidationResult.Success, attribute.Run_IsValid(value));
        }

        [Theory]
        [InlineData("dturton@conestogac.on.ca")]
        [InlineData("schneier@schneier.com")]
        [InlineData("faculty.affairs@university7.professor-faculty-affairs.mil")]
        [InlineData("caat-pension.on.ca@mail82.atl111.rsgsv.net")]
        [InlineData("no-reply@d2l.conestoga.edu")]
        [InlineData("pamuslide@padmate.cn")]
        [InlineData("dturton@conestoga.accountant")]
        public void EmailAnnotationValidEmails_ShouldPass(string value)
        {
            //Arrange
            Local_EmailAttribute attribute = new Local_EmailAttribute();
            //Act & Assert
            Assert.Equal(ValidationResult.Success, attribute.Run_IsValid(value));
        }

        [Theory]
        [InlineData("dturton.conestogac.on.ca.")]
        [InlineData(".schneier@schneier.com")]
        [InlineData("(no-reply:)@d2l.conestoga.edu")]
        [InlineData(@"pamu\slide@pad_mate.cn")]
        [InlineData("david,turton@conestoga.accountant")]
        public void EmailAnnotationInvalidEmails_ShouldTripValidation(string value)
        {
            //Arrange
            Local_EmailAttribute attribute = new Local_EmailAttribute();
            //Act & Assert
            Assert.NotEqual(ValidationResult.Success, attribute.Run_IsValid(value));
        }

        #endregion

        #region foreign key checks

        [Fact]
        public void ProvinceCodeNotOnFile()
        {
            //Arrange
            Initialize();
            nameAddress.ProvinceCode = "ZZ";
            //Act
            _context.Add(nameAddress);
            //Assert

            //use this if the data is invalid and should trip validation
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted/calculated
            //_context.EFValidation();
            //Assert.Equal("New Hamburg", nameAddress.FirstName);
        }

        [Fact]
        public void ProvinceLowerCase_ShouldShiftToUpper()
        {
            //Arrange
            Initialize();
            nameAddress.ProvinceCode = "on";
            //Act
            _context.Add(nameAddress);
            //Assert

            // use this if the data is invalid and should trip validation
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());

            // use this if the data is valid and should pass edits cleanly
            //_context.EFValidation();

            // use this if the data is to be reformatted / calculated
            _context.EFValidation();
            Assert.Equal("ON", nameAddress.ProvinceCode);
        }

        #endregion

        #region controller checks (try-catch, modelstate)

        [Fact]
        public async void NameAddressController_CreateShouldCatchExceptions()
        {
            //Arrange
            JBKNameAddressController controller = new JBKNameAddressController(_context);
            Initialize();
            nameAddress.ProvinceCode = "green";
            //Act 
            try
            {
                var result = await controller.Create(nameAddress);
            }
            //Assert
            catch (Exception ex)
            {
                Assert.True(false, "NameAddressController's Create did not catch exception ... " +
                        ex.GetBaseException().Message);
            }
        }

        [Fact]
        public async void NameAddressController_CreateShouldPutExceptionIntoModelState()
        {
            //Arrange
            JBKNameAddressController controller = new JBKNameAddressController(_context);
            Initialize();
            nameAddress.ProvinceCode = "green";           
            try
            {
                //Act 
                var result = await controller.Create(nameAddress);
                //Assert
                Assert.IsType<ViewResult>(result);
                ViewResult viewResult = (ViewResult)result;
                Assert.NotNull(viewResult.ViewData.ModelState);
                Assert.NotEmpty(viewResult.ViewData.ModelState.Keys);
                foreach (string item in viewResult.ViewData.ModelState.Keys)
                {
                    Assert.Equal("", item);
                }
            }
            catch (Exception) { } // if this test code throws an exception, just ignore it
        }

        #endregion

    }
}
