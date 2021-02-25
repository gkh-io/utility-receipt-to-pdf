using System.Globalization;
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    public sealed class BarcodeBlockPrinter : IBarcodeBlockPrinter
    {
        private readonly IBarcodeRenderer _barcodeRenderer;
        private readonly CommonPresentationSettings _commonPresentationSettings;

        public BarcodeBlockPrinter(IBarcodeRenderer barcodeRenderer, CommonPresentationSettings commonPresentationSettings)
        {
            _barcodeRenderer = barcodeRenderer;
            _commonPresentationSettings = commonPresentationSettings;
        }

        public PdfPTable Print(string barcodeMonth, string barcodeTotal, PaymentsInfo payments, PdfWriter writer,
            PaymentPeriod period)
        {
            var imageTable = new PdfPTable(1);

            var label = CreatePeriodBarcodeLabel(period);

            imageTable.AddCell(new PdfPCell(CreateBarcodeHeader(payments.ForPeriod, label))
            {
                BorderWidth = _commonPresentationSettings.WithoutBorder
            });

            var monthBarcode = _barcodeRenderer.Render(barcodeMonth, writer);
            imageTable.AddCell(monthBarcode);

            imageTable.AddCell(new PdfPCell(CreateBarcodeHeader(payments.TotalPayment, 
                "Итого к оплате"))
            {
                BorderWidth = _commonPresentationSettings.WithoutBorder
            });

            var yearBarcode = _barcodeRenderer.Render(barcodeTotal, writer);
            imageTable.AddCell(yearBarcode);

            return imageTable;
        }

        private PdfPTable CreateBarcodeHeader(decimal sum, string label)
        {
            var recordTable = new PdfPTable(new[] {1f, 1f});


            var labelCell = new PdfPCell(new Phrase(label, _commonPresentationSettings.SmallFont))
            {
                BorderWidth = _commonPresentationSettings.WithoutBorder
            };
            recordTable.AddCell(labelCell);

            // ToDo: вынести это форматирование
            var nfi = (NumberFormatInfo) CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            var sumCell = new PdfPCell(new Phrase(sum.ToString("#,0.00", nfi),
                _commonPresentationSettings.SmallBoldFont))
            {
                BorderWidth = _commonPresentationSettings.WithoutBorder
            };

            recordTable.AddCell(sumCell);
            return recordTable;
        }

        private static string CreatePeriodBarcodeLabel(PaymentPeriod period)
        {
            var formattedPaymentPeriod = $"{period.GetMonthString()} {period.Year}";

            var label = "Начислено за " + formattedPaymentPeriod;
            return label;
        }
    }
}