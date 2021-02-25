using System;
using System.IO;
using System.Threading.Tasks;
using GkhIo.Receipt.Pdf.Models;
using GkhIo.Receipt.Pdf.Services;
using NodaTime;
using Xunit;
using Xunit.Abstractions;

namespace Gkhio.Receipt.Pdf.Tests.ModuleTests
{
    /// <summary>
    ///     Тест полной функции печати документа
    /// </summary>
    public sealed class ReceiptPdfPritnerTests
    {
        private const string EthalonPath = "samples\\sample.pdf";
        private readonly ITestOutputHelper _testOutputHelper;

        public ReceiptPdfPritnerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task CentrstroiReceiptTest()
        {
            // подготовка
            var model = new GkhIo.Receipt.Pdf.Models.Receipt
            {
                Address = new Address
                {
                    CityFull = "г. Реутов",
                    StreetFull = "Земляничный пр-кт",
                    HouseFull = "дом 14",
                    FlatFull = "кв. 262"
                },
                PersonalAccount = "1234567890",
                Meters = new[]
                {
                    new Meter
                    {
                        Type = "ГВС",
                        Number = "ГВ-14-019224702",
                        PreviousValue = 123,
                        Date = new LocalDate(2022, 08, 24)
                    },
                    new Meter
                    {
                        Date = new LocalDate(2021, 10, 3),
                        Number = "Э-12345678",
                        Type = "Электроэн.",
                        PreviousValue = 1223.32m
                    },
                    new Meter
                    {
                        Type = "ХВС",
                        Number = "ХВ-14-123123123",
                        PreviousValue = 218,
                        Date = new LocalDate(2021, 01, 01)
                    },
                    new Meter
                    {
                        Type = "Отопление",
                        Number = "Т-2725241",
                        PreviousValue = 3.1718m,
                        Date = new LocalDate(2025, 10, 31)
                    }
                },
                Payer = new Person
                {
                    FirstName = "Михаил",
                    MiddleName = "Васгенович",
                    LastName = "Бубрунков"
                },
                QR =
                    "ST00012|Name=ООО Управляющая компания \"Центрстрой\"|PersonalAcc=40702810800000085338|BankName=ПАО \"ПРОМСВЯЗЬБАНК\"|BIC=044525555|CorrespAcc=30101810400000000555|PayeeINN=5012054070|lastName=МУРАВЬЕВА|firstName=ВАЛЕНТИНА|middleName=ВАСИЛЬЕВНА|persAcc=2530000109|PayerAddress=ЮБИЛЕЙНЫЙ пр-кт, дом 16, кв. 262|Sum=364157",
                PaymentReceiver = new PaymentReceiver
                {
                    BankName = "ПАО \"ПРОМСВЯЗЬБАНК\"",
                    Bic = "044525555",
                    CompanyName = "ООО Управляющая компания \"Центрстрой\"",
                    CorrespondenceAccount = "30101810400000000555",
                    INN = "5012054070",
                    Kpp = "504101001",
                    PaymentAccount = "40702810800000085338"
                },
                Number = "12345678",
                PaymentPeriod = new PaymentPeriod
                {
                    Year = 2021,
                    Month = 12
                },
                Sum = 5678.89m,
                QRAdditionalInfo =
                    "Данный QR-код предназначен для удобства оплаты квитанции через приложение Сбербанк Онлайн(комиссия 1%) или через терминалы/автоматы Сбербанка(комиссия 1.75%)",
                FlatInfo = new FlatInfo
                {
                    AreaOfCommonAreas = 5940.90m,
                    CommonArea = 56.80m,
                    HeatedArea = 55.70m,
                    HouseArea = 36659.00m,
                    LivingArea = 23.70m,
                    PersonsRegistered = 1,
                    PremisesArea = 30718.10m,
                    Type = "частная"
                },
                PaidDay = 10,
                Payments = new PaymentsInfo
                {
                    ForPeriod = 3641.57m,
                    IncomingInPeriod = 3935.68m,
                    LastPayment = new LocalDate(2020, 9, 6),
                    RestForBeggingOfPeriod = 3935.68m,

                    TotalPayment = 3641.57m
                },
                BottomNotes = new[]
                {
                    "Адрес электронной почты: centrstroy.service@yandex.ru" + Environment.NewLine +
                    "Официальный сайт: uk-centrstroy.ru",

                    "Бухгалтерия ул. Октября, д.18 тел. 8 (495) 528 10 42, ул.Реутовских ополченцев д.14 тел. 8 (495) 528 10 24,\r\nДиспетчерская служба УК \"Центрстрой\": южная часть тел 8 (495) 791 35 95, 8(495) 528 10 02, 8(495) 528 10 06, северная часть тел 8(495) 528 10 71,8(495) 528 10 81 Паспортный стол г.Реутов, ул .Победы, д.7 тел. 8(495) 526-41-30,\r\nСоц.защита - ул.Кирова, д. 5,тел. 8(495)528-11-30, 8(495)528-33-79,000 \"Панател\" тел. 8(495)791-95-00. Обслуживание домофонной системы: южная часть ООО \"Сейф-Видео” тел. 8-916-884-43-22, 8-495-226-85-11, северная часть ООО \"ПСГ\" тел. 8-499-929-99-99",

                    "Внимание!!! Управляющая компания напоминает! В соответствие с Федеральным законом от 23.11.2009 № 201 -ФЗ «Об энергосбережении и энергетической эффективности...)» владельцы помещений в многоквартирных домах обязаны за счет установить и ввести в эксплуатацию приборы учета потребляемых коммунальных ресурсов.",

                    "По вопросам начислений за услуги «Обращение с ТКО» и «Взнос на капитальный ремонт» обращаться в ООО МОСОБЛЕИРЦ, тел, +7 (496) 374-51-91 доб. 1192. 1193, 1194, 1195, 1196. сайт, адрес- г. Реутов, ул, Новая, д, 2,"
                },
                RightNotes = new[]
                {
                    "Уважаемые собственники!\r\nВы можете воспользоваться личным кабинетом для оплаты, внесения показаний по счетчикам:",
                    "https://centstroy.dom4me.ru",
                    "с 20 по 23 число текущего месяца"
                },
                BarcodeForMonth = "7901207559510364157027",
                BarcodeForTotal = "7901207559510364157036",

                PayUpTo = new LocalDate(2020, 10, 11),

                UtilityGroups = new[]
                {
                    new UtilityGroup
                    {
                        Name = "Начисления за содержание жилого помещения",
                        Utilities = new []
                        {
                            new UtilityData
                            {
                                Name = "СОДЕРЖ.И РЕМ.ЖИЛ.ПОМ.",
                                Volume = 55.7m,
                                Unit = "кв. м.",
                                Tariff = 39.90m,
                                ChargesByTariff = 2222.43m,
                                Total = 2222.43m
                            },
                            new UtilityData
                            {
                                Name = "ХВС НА ОДН",
                                Volume = 0.0646m,
                                Unit = "куб.м.",
                                Tariff = 47.72m,
                                ChargesByTariff = 3.08m,
                                IncreaseCoefficient = 1.5m,
                                IncreasePayment = 2.33m,
                                Recalculation = 2.33m,
                                Total = 3.08m
                            }
                        }
                    },
                    new UtilityGroup
                    {
                        Name = "Начисления за коммунальные услуги",
                        Utilities = new []
                        {
                            new UtilityData
                            {
                                Name = "СОДЕРЖ.И РЕМ.ЖИЛ.ПОМ.",
                                Volume = 55.7m,
                                Unit = "кв. м.",
                                Tariff = 39.90m,
                                ChargesByTariff = 2222.43m,
                                Total = 2222.43m
                            },
                            new UtilityData
                            {
                                Name = "ХВС НА ОДН",
                                Volume = 0.0646m,
                                Unit = "куб.м.",
                                Tariff = 47.72m,
                                ChargesByTariff = 3.08m,
                                IncreaseCoefficient = 1.5m,
                                IncreasePayment = 2.33m,
                                Recalculation = 2.33m,
                                Total = 3.08m
                            }
                        }
                    },
                    new UtilityGroup
                    {
                        Name = "Начисления за иные услуги",
                        Utilities = new []
                        {
                            new UtilityData
                            {
                                Name = "СОДЕРЖ.И РЕМ.ЖИЛ.ПОМ.",
                                Volume = 55.7m,
                                Unit = "кв. м.",
                                Tariff = 39.90m,
                                ChargesByTariff = 2222.43m,
                                Total = 2222.43m
                            },
                            new UtilityData
                            {
                                Name = "ХВС НА ОДН",
                                Volume = 0.0646m,
                                Unit = "куб.м.",
                                Tariff = 47.72m,
                                ChargesByTariff = 3.08m,
                                IncreaseCoefficient = 1.5m,
                                IncreasePayment = 2.33m,
                                Recalculation = 2.33m,
                                Total = 3.08m
                            }
                        }
                    }
                }
            };

            var addressFormatter = new AddressFormatter();
            var commonPresentationSettings = new CommonPresentationSettings();
            var personFullFormFormatter = new PersonFullFormFormatter();
            var flatInfoPrinter = new FlatInfoPrinter(commonPresentationSettings);
            var paymentAdvicePrinter = new QRAdditionalInfoPrinter(commonPresentationSettings);
            var derailsPaymentInfoPrinter = new DetailsPaymentInfoPrinter(commonPresentationSettings,
                personFullFormFormatter, addressFormatter, flatInfoPrinter);
            var receiptSumAndSignPrinter = new ReceiptSumAndSignPrinter(commonPresentationSettings);
            var tabledWordRenderer = new TabledWordRenderer(commonPresentationSettings);
            var personalAccountTablePrinter =
                new PersonalAccountTablePrinter(commonPresentationSettings, tabledWordRenderer);
            var receiptPersonPaymentsPrinter =
                new ReceiptPersonPaymentsPrinter(commonPresentationSettings, tabledWordRenderer);
            var metersTablesPrinter =
                new MeterTablesPrinter(commonPresentationSettings, new PersonShortFormFormatter());
            var qrBlockPrinter = new QrBlockPrinter(commonPresentationSettings);
            var paymentInfoPrinter = new PaymentInfoPrinter(commonPresentationSettings,
                tabledWordRenderer, personFullFormFormatter, addressFormatter);
            var barcodeRenderer = new BarcodeRenderer();
            var barcodeBlockPrinter = new BarcodeBlockPrinter(barcodeRenderer, commonPresentationSettings);
            var utilitiesPrinter = new UtilitiesPrinter(commonPresentationSettings);
            var receiptPdfPrinter = new ReceiptPdfPrinter(addressFormatter,
                personalAccountTablePrinter, metersTablesPrinter, commonPresentationSettings,
                qrBlockPrinter, paymentInfoPrinter, receiptSumAndSignPrinter, paymentAdvicePrinter,
                derailsPaymentInfoPrinter, receiptPersonPaymentsPrinter, barcodeBlockPrinter, utilitiesPrinter);

            // действие
            var document = await receiptPdfPrinter.Print(model);

            // проверка
            Assert.NotNull(document);
            Assert.NotNull(document.FileName);
            Assert.NotNull(document.Stream);
            var loadEthalon = await File.ReadAllBytesAsync(EthalonPath);
            var createdFileStream = new MemoryStream();
            await document.Stream.CopyToAsync(createdFileStream);
            var createdFileArray = createdFileStream.ToArray();

            await File.WriteAllBytesAsync(document.FileName, createdFileArray);
            _testOutputHelper.WriteLine(Path.Combine(document.FileName));
            Assert.Equal(loadEthalon.Length, createdFileArray.Length);
        }
    }
}