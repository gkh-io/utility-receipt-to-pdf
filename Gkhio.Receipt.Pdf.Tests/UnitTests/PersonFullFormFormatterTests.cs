using System;
using GkhIo.Receipt.Pdf.Models;
using GkhIo.Receipt.Pdf.Services;
using Xunit;

namespace Gkhio.Receipt.Pdf.Tests.UnitTests
{
    /// <summary>
    /// Тесты полного представления ФИО
    /// </summary>
    public class PersonFullFormFormatterTests
    {
        [Fact]
        public void TestFullFormat()
        {
            // подготовка
            var source = new Person
            {
                LastName = "11",
                FirstName = "22",
                MiddleName = "33"
            };
            var formatter = new PersonFullFormFormatter();

            // действие
            var result = formatter.ToFullForm(source);

            // проверка
            Assert.NotNull(result);
            Assert.Equal("11 22 33", result);
        }

        /// <summary>
        /// Проверяем пустой фио
        /// </summary>
        [Fact]
        public void TestEmpty()
        {
            // подготовка
            var source = new Person();
            var formatter = new PersonFullFormFormatter();

            // действие
            var result = formatter.ToFullForm(source);

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
            var formatter = new PersonFullFormFormatter();

            // действие
            var result = formatter.ToFullForm(source);

            // проверка
            Assert.NotNull(result);
            Assert.Equal("22", result);
        }

        [Fact]
        public void TestNullParameter()
        {
            // подготовка
            var formatter = new PersonFullFormFormatter();

            // действие
            Assert.Throws<ArgumentNullException>(() => formatter.ToFullForm(null));
        }
    }
}