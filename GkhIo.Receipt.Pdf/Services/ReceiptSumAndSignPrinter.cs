using System.Globalization;
using GkhIo.Receipt.Pdf.Abstract;
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    public sealed class ReceiptSumAndSignPrinter : IReceiptSumAndSignPrinter
    {
        private readonly CommonPresentationSettings _commonPresentationSettings;
        
        private PdfPTable _layoutTable;

        public ReceiptSumAndSignPrinter(CommonPresentationSettings commonPresentationSettings)
        {
            _commonPresentationSettings = commonPresentationSettings;
        }

        public PdfPTable Print(string receiptNumber, PaymentPeriod paymentPeriod, decimal sum)
        {
            var result = new PdfPTable(1);

            var font = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.NORMAL);
            var boldFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.BOLD);
            var invisibleFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.NORMAL, BaseColor.WHITE);

            _layoutTable = new PdfPTable(new[]{2f, 5f});
            AddTextCell("№ счёта-извещения:", font);
            AddTextCell(receiptNumber, boldFont);
            result.AddCell(new PdfPCell(_layoutTable)
            {
                BorderWidth = 0
            });

            _layoutTable = new PdfPTable(new[] { 2f, 5f });
            AddBorderedtCell("Расчётный период", font, Element.ALIGN_CENTER);

            var formattedPaymentPeriod = $"{paymentPeriod.GetMonthString().ToUpperInvariant()} {paymentPeriod.Year} г.";
            AddBorderedtCell(formattedPaymentPeriod, boldFont, Element.ALIGN_CENTER);
            result.AddCell(new PdfPCell(_layoutTable)
            {
                BorderWidth = 0
            });

            _layoutTable = new PdfPTable(new[] { 2f, 5f });

            AddBorderedtCell("Сумма к оплате", font, Element.ALIGN_CENTER);
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            AddBorderedtCell(sum.ToString("#,0.00", nfi), boldFont, Element.ALIGN_CENTER);
            result.AddCell(new PdfPCell(_layoutTable)
            {
                BorderWidth = 0,
                PaddingTop = 2
            });

            _layoutTable = new PdfPTable(new[] { 2f, 5f });

            AddTextCell("", font);
            AddBorderedtCell("Сумма оплаты", font, Element.ALIGN_LEFT);
            AddTextCell("_", invisibleFont);
            var labelCell = new PdfPCell(new Phrase("_", invisibleFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BorderWidth = _commonPresentationSettings.ThinBorder,
                MinimumHeight = 32f
            };
            _layoutTable.AddCell(labelCell);

            result.AddCell(new PdfPCell(_layoutTable)
            {
                BorderWidth = 0,
                PaddingTop = 5
            });

            _layoutTable = new PdfPTable(new[] { 5f, 3f });

            
            AddBorderedtCell("Подпись", font, Element.ALIGN_CENTER);
            AddBorderedtCell("Дата", font, Element.ALIGN_CENTER);
            AddBorderedtCell("", font, Element.ALIGN_CENTER);
            labelCell = new PdfPCell(new Phrase("_", invisibleFont))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BorderWidth = _commonPresentationSettings.ThinBorder,
                MinimumHeight = 28f
            };
            _layoutTable.AddCell(labelCell);

            result.AddCell(new PdfPCell(_layoutTable)
            {
                BorderWidth = 0,
                PaddingTop = 12,
                PaddingLeft = 15,
                PaddingBottom = 10
            });


            return result;
        }


        private void AddTextCell(string label, Font font)
        {
            var labelCell = new PdfPCell(new Phrase(label, font))
            {
                PaddingTop = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = _commonPresentationSettings.WithoutBorder
            };
            _layoutTable.AddCell(labelCell);
        }

        private void AddBorderedtCell(string text, Font font, int horizontalAlignment)
        {
            var labelCell = new PdfPCell(new Phrase(text, font))
            {
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = horizontalAlignment,
                BorderWidth = _commonPresentationSettings.ThinBorder
            };
            _layoutTable.AddCell(labelCell);
        }
    }
}