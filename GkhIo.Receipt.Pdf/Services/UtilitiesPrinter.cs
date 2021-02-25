using System.Globalization;
using GkhIo.Receipt.Pdf.Abstract;
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Org.BouncyCastle.Asn1;

namespace GkhIo.Receipt.Pdf.Services
{
    public sealed class UtilitiesPrinter : IUtilitiesPrinter
    {
        private readonly CommonPresentationSettings _commonPresentationSettings;
        private PaymentPeriod _paymentPeriod;
        private string[] Headers => new[]
        {
            "Виды услуг",
            "Объем услуг",
            "Ед.изм.",
            "Тариф руб.",
            "Начислено по тарифу руб.",
            "Размер повышающего коэффициента",
            "Размер превышения платы, рассчитанной с применением повышающего коэффициента над размером платы",
            "Перерасчеты, руб.",
            $"Итого к оплате за {_paymentPeriod.GetMonthString()} {_paymentPeriod.Year} г., руб."
        };

        public UtilitiesPrinter(CommonPresentationSettings commonPresentationSettings)
        {
            _commonPresentationSettings = commonPresentationSettings;
        }

        public PdfPTable Print(UtilityGroup[] utilityGroups, PaymentPeriod paymentPeriod)
        {
            _paymentPeriod = paymentPeriod;

            var utilityTable = new PdfPTable(new[] {15f, 4f, 3.3f, 2.5f, 3.7f, 3.8f, 5f, 4f, 4.7f})
            {
                WidthPercentage = 100,
                SpacingBefore = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks
            };

            utilityTable.AddCell(
                new PdfPCell(
                        new Phrase($"РАСЧЕТ РАЗМЕРА ПЛАТЫ ЗА ЖИЛОЕ ПОМЕЩЕНИЕ, КОММУНАЛЬНЫЕ и ИНЫЕ УСЛУГИ за {paymentPeriod.GetMonthString()} {paymentPeriod.Year} г.",
                            _commonPresentationSettings.SmallFont))
                {
                    Colspan = utilityTable.NumberOfColumns,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            
            PrintHeaders(utilityTable);

            foreach (var group in utilityGroups)
            {
                PrintUtilityGroup(utilityTable, @group);
            }

            return utilityTable;
        }

        private void PrintUtilityGroup(PdfPTable utilityTable, UtilityGroup @group)
        {
            utilityTable.AddCell(
                new PdfPCell(
                    new Phrase(@group.Name, _commonPresentationSettings.SmallBoldItalicFont))
                {
                    Colspan = utilityTable.NumberOfColumns,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

            foreach (var utility in @group.Utilities)
            {
                PrintUtility(utilityTable, utility);
            }
        }

        private void PrintUtility(PdfPTable utilityTable, UtilityData utility)
        {
            utilityTable.AddCell(new PdfPCell(new Phrase(utility.Name, _commonPresentationSettings.SmallFont))
            {
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });

            AddNumberCell(utilityTable, utility.Volume);

            utilityTable.AddCell(new PdfPCell(new Phrase(utility.Unit, _commonPresentationSettings.SmallFont))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });

            AddNumberCell(utilityTable, utility.Tariff);
            AddNumberCell(utilityTable, utility.ChargesByTariff);
            AddNumberCell(utilityTable, utility.IncreaseCoefficient);
            AddNumberCell(utilityTable, utility.IncreasePayment);
            AddNumberCell(utilityTable, utility.Recalculation);
            AddNumberCell(utilityTable, utility.Total);
        }

        private void AddNumberCell(PdfPTable utilityTable, decimal? value)
        {
            if (value == null)
            {
                utilityTable.AddCell("");
                return;
            }

            utilityTable.AddCell(new PdfPCell(new Phrase(value.Value.ToString(CultureInfo.InvariantCulture),
                _commonPresentationSettings.SmallFont))
            {
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_MIDDLE
            });
        }

        private void PrintHeaders(PdfPTable utilityTable)
        {
            foreach (var header in Headers)
            {
                var usedFont = _commonPresentationSettings.SmallFont;
                if (header.Length > 15)
                {
                    usedFont = _commonPresentationSettings.ExtraSmallFont;
                }
                else if (header.Length > 30)
                {
                    usedFont = _commonPresentationSettings.SuperExtraSmallFont;
                }
                utilityTable.AddCell(new PdfPCell(new Phrase(header, usedFont))
                {
                    HorizontalAlignment = Element.ALIGN_CENTER,
                    VerticalAlignment = Element.ALIGN_MIDDLE
                });
            }
        }
    }
}