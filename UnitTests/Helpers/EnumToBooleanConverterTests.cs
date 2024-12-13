using System;
using System.Globalization;
using Xunit;
using BPR2_Desktop.Model.Helpers;
using BPR2_Desktop.Model.Helpers;
using Wpf.Ui.Appearance;

namespace BPR2_Desktop.Tests.Helpers
{
    public class EnumToBooleanConverterTests
    {
        private readonly EnumToBooleanConverter _converter;

        public EnumToBooleanConverterTests()
        {
            _converter = new EnumToBooleanConverter();
        }

        [Fact]
        public void Convert_ShouldReturnTrue_WhenEnumValuesMatch()
        {
            // Arrange
            var value = ApplicationTheme.Dark; // The current enum value
            var parameter = "Dark"; // The parameter passed in (should match the enum name)

            // Act
            var result = _converter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);

            // Assert
            Assert.True((bool)result);
        }

        [Fact]
        public void Convert_ShouldReturnFalse_WhenEnumValuesDoNotMatch()
        {
            // Arrange
            var value = ApplicationTheme.Light; // The current enum value
            var parameter = "Dark"; // The parameter passed in (should not match the enum value)

            // Act
            var result = _converter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture);

            // Assert
            Assert.False((bool)result);
        }

        [Fact]
        public void Convert_ShouldThrowArgumentException_WhenInvalidEnumValueIsPassed()
        {
            // Arrange
            var value = "InvalidEnum"; // Invalid enum value
            var parameter = "Dark"; // The parameter passed in (should match the enum name)

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _converter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture));
            Assert.Equal("ExceptionEnumToBooleanConverterValueMustBeAnEnum", exception.Message);
        }

        [Fact]
        public void Convert_ShouldThrowArgumentException_WhenParameterIsNotStringEnum()
        {
            // Arrange
            var value = ApplicationTheme.Light; // A valid enum value
            var parameter = 123; // Invalid parameter type (should be a string)

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _converter.Convert(value, typeof(bool), parameter, CultureInfo.InvariantCulture));
            Assert.Equal("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName", exception.Message);
        }

        [Fact]
        public void ConvertBack_ShouldReturnCorrectEnumValue()
        {
            // Arrange
            var parameter = "Dark"; // The parameter passed in (should match the enum name)

            // Act
            var result = _converter.ConvertBack(true, typeof(ApplicationTheme), parameter, CultureInfo.InvariantCulture);

            // Assert
            Assert.Equal(ApplicationTheme.Dark, result);
        }

        [Fact]
        public void ConvertBack_ShouldThrowArgumentException_WhenParameterIsNotStringEnum()
        {
            // Arrange
            var parameter = 123; // Invalid parameter type (should be a string)

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _converter.ConvertBack(true, typeof(ApplicationTheme), parameter, CultureInfo.InvariantCulture));
            Assert.Equal("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName", exception.Message);
        }
    }
}
