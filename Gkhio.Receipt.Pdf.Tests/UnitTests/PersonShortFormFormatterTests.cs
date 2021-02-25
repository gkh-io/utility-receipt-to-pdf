using System;
using GkhIo.Receipt.Pdf.Models;
using GkhIo.Receipt.Pdf.Services;
using Xunit;

namespace Gkhio.Receipt.Pdf.Tests.UnitTests
{
    /// <summary>
    /// Тесты краткого представления ФИО
    /// </summary>
    public class PersonShortFormFormatterTests
    {
        [Fact]
        public void TestShortFormat()
        {
            // подготовка
            var source = new Person
            {
                LastName = "11",
                FirstName = "22",
                MiddleName = "33"
            };
            var formatter = new PersonShortFormFormatter();

            // действие
            var result = formatter.ToShortForm(source);

            // проверка
            Assert.NotNull(result);
            Assert.Equal("11 2.3.", result);
        }

        /// <summary>
        /// Проверяем пустой фио
        /// </summary>
        [Fact]
        public void TestEmpty()
        {
            // подготовка
            var source = new Person();
            var formatter = new PersonShortFormFormatter();

            // действие
            var result = formatter.ToShortForm(source);

            // проверка
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void TestFirstNameOnly()
        {
            // подготовка
            var source = new Person
            {
                FirstName = "22"
            };
            var formatter = new PersonShortFormFormatter();

            // действие
            var result = formatter.ToShortForm(source);

            // проверка
            Assert.NotNull(result);
            Assert.Equal("2.", result);
        }

        [Fact]
        public void TestNullParameter()
        {
            // подготовка
            var formatter = new PersonShortFormFormatter();

            // действие
            Assert.Throws<ArgumentNullException>(() => formatter.ToShortForm(null));
        }
    }
}