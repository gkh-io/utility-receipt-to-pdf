using System;
using GkhIo.Receipt.Pdf.Models;
using GkhIo.Receipt.Pdf.Services;
using Xunit;

namespace Gkhio.Receipt.Pdf.Tests
{
    /// <summary>
    /// Тестирование форматирования адреса для квитанции
    /// </summary>
    public class AddressFormatterTests
    {
        /// <summary>
        /// Проверка форматирования полного адреса
        /// </summary>
        [Fact]
        public void TestFullBuild()
        {
            // подготовка
            var address = new Address
            {
                CityFull = "г. Реутов",
                StreetFull = "Юбилейный пр-кт",
                HouseFull = "дом 33",
                FlatFull = "кв. 23"
            };
            var builder = new AddressFormatter();

            // действие
            var result = builder.FormatAddressForReceipt(address);

            // проверка
            Assert.NotNull(result);
            Assert.Equal("г. Реутов, Юбилейный пр-кт, дом 33, кв. 23", result);
        }

        /// <summary>
        /// Проверка случая пустого адреса
        /// факт того, что не будет никаких артефактов
        /// </summary>
        [Fact]
        public void TestEmptyAddress()
        {
            // подготовка
            var address = new Address();
            var builder = new AddressFormatter();

            // действие
            var result = builder.FormatAddressForReceipt(address);

            // проверка
            Assert.NotNull(result);
            Assert.Equal(string.Empty, result);
        }

        /// <summary>
        /// Проверка адреса только с домом
        /// факт того, что не будет никаких артефактов
        /// </summary>
        [Fact]
        public void TestSingleHouse()
        {
            // подготовка
            var address = new Address
            {
                HouseFull = "1"
            };
            var builder = new AddressFormatter();

            // действие
            var result = builder.FormatAddressForReceipt(address);

            // проверка
            Assert.NotNull(result);
            Assert.Equal("1", result);
        }

        /// <summary>
        /// Проверка передачи null аргумента
        /// </summary>
        [Fact]
        public void TestArgumentNullException()
        {
            // подготовка
            var builder = new AddressFormatter();

            // действие
            Assert.Throws<ArgumentNullException>(() => builder.FormatAddressForReceipt(null));
        }
    }
}