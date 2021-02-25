using System.Globalization;
using GkhIo.Receipt.Pdf.Abstract;
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using NodaTime;

namespace GkhIo.Receipt.Pdf.Services
{
    public sealed class ReceiptPersonPaymentsPrinter : IReceiptPersonPaymentsPrinter
    {
        private readonly CommonPresentationSettings _commonPresentationSettings;
        private readonly ITabledWordRenderer _tabledWordRenderer;
        
        private PdfPTable _layoutTable;

        public ReceiptPersonPaymentsPrinter(CommonPresentationSettings commonPresentationSettings
            , ITabledWordRenderer tabledWordRenderer)
        {
            _commonPresentationSettings = commonPresentationSettings;
            _tabledWordRenderer = tabledWordRenderer;
        }

        public PdfPTable Print(string receiptNumber, string personalAccount, PaymentPeriod paymentPeriod,
            PaymentsInfo payments, LocalDate payUpTo)
        {
            var result = new PdfPTable(1);

            var font = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.NORMAL);
            var boldFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.BOLD);
            var bigBoldFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10, Font.BOLD);

            _layoutTable = new PdfPTable(new[]{2f, 5f});
            AddTextCell("№ счёта-извещения:", font, Element.ALIGN_LEFT);
            AddTextCell(receiptNumber, boldFont, Element.ALIGN_LEFT);
            result.AddCell(new PdfPCell(_layoutTable)
            {
                BorderWidth = 0
            });


            _layoutTable = new PdfPTable(new[] { 2f, 7f });
            var cell = new PdfPCell(new Phrase("Лицевой счёт:", _commonPresentationSettings.SmallFont))
            {
                PaddingTop = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks,
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = _commonPresentationSettings.WithoutBorder
            };
            _layoutTable.AddCell(cell);

            _layoutTable.AddCell(
                new PdfPCell(
                    _tabledWordRenderer.Render(personalAccount))
                {
                    BorderWidth = _commonPresentationSettings.WithoutBorder,
                    PaddingTop = 5f,
                    PaddingBottom = 5f
                });

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
                BorderWidth = 0,
                PaddingTop = 45f
            });

            _layoutTable = new PdfPTable(new[] { 2.8f, 1f });

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            AddTextCell("Начислено за период:", font, Element.ALIGN_RIGHT);
            AddBorderedtCell(payments.ForPeriod.ToString("#,0.00", nfi), boldFont, Element.ALIGN_RIGHT);

            AddTextCell("Остаток на начало периода(+/-):", font, Element.ALIGN_RIGHT);
            AddBorderedtCell(payments.RestForBeggingOfPeriod.ToString("#,0.00", nfi), boldFont, Element.ALIGN_RIGHT);

            AddTextCell("Поступило за период:", font, Element.ALIGN_RIGHT);
            AddBorderedtCell(payments.IncomingInPeriod.ToString("#,0.00", nfi), boldFont, Element.ALIGN_RIGHT);

            AddTextCell("Дата последней оплаты:", font, Element.ALIGN_RIGHT);
            AddBorderedtCell(payments.LastPayment.Value.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture), boldFont, Element.ALIGN_RIGHT);

            AddTextCell("Итого к оплате:", font, Element.ALIGN_RIGHT);
            AddBorderedtCell(payments.TotalPayment.ToString("#,0.00", nfi), boldFont, Element.ALIGN_RIGHT);

            result.AddCell(new PdfPCell(_layoutTable)
            {
                BorderWidth = 0,
                PaddingTop = 5f
            });


            var upToTable = new PdfPTable(new[] { 1f, 1f });

            var phrase = new Phrase("Оплатить счёт до: ", font);
            var phraseCell = new PdfPCell(phrase)
            {
                BorderWidth = _commonPresentationSettings.WithoutBorder,
                VerticalAlignment = Element.ALIGN_BOTTOM,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };
            upToTable.AddCell(phraseCell);


            var phraseDate = new Phrase(payUpTo.ToString("dd MM yyyy", CultureInfo.InvariantCulture) + " г.", bigBoldFont);
            var dateCell = new PdfPCell(phraseDate)
            {
                BorderWidthTop = _commonPresentationSettings.WithoutBorder,
                BorderWidthLeft = _commonPresentationSettings.WithoutBorder,
                BorderWidthRight = _commonPresentationSettings.WithoutBorder,
                BorderWidthBottom = _commonPresentationSettings.ThinBorder,

                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_BOTTOM
            };

            upToTable.AddCell(dateCell);

            var upToCell = new PdfPCell(upToTable)
            {
                BorderWidth = _commonPresentationSettings.WithoutBorder
            };
            result.AddCell(upToCell);

            return result;
        }


        private void AddTextCell(string label, Font font, int horizontalAlignment)
        {
            var labelCell = new PdfPCell(new Phrase(label, font))
            {
                PaddingTop = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                HorizontalAlignment = horizontalAlignment,
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