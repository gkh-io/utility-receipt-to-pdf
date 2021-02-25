using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GkhIo.Receipt.Pdf.Abstract;
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    /// <summary>
    ///     Формирует PDF документ для квитанции
    /// </summary>
    public sealed class ReceiptPdfPrinter : IReceiptPdfPrinter
    {
        private const float MarginLeft = 20;
        private const float MarginRight = 20;
        private const float MarginTop = 10;
        private const float MarginBottom = 20;

        private readonly IAddressFormatter _addressFormatter;
        private readonly IPersonalAccountTablePrinter _personalAccountTablePrinter;
        private readonly IMeterTablesPrinter _meterTablesPrinter;
        private readonly CommonPresentationSettings _commonPresentationSettings;
        private readonly IQrBlockPrinter _qrBlockPrinter;
        private readonly IPaymentInfoPrinter _paymentInfoPrinter;
        private readonly IReceiptSumAndSignPrinter _receiptSumAndSignPrinter;
        private readonly IQRAdditionalInfoPrinter _paymentBlockPrinter;
        private readonly IDetailsPaymentInfoPrinter _detailsPaymentInfo;
        private readonly IReceiptPersonPaymentsPrinter _receiptPersonPaymentsPrinter;
        private readonly IBarcodeBlockPrinter _barcodeBlockPrinter;
        private readonly IUtilitiesPrinter _utilitiesPrinter;

        public ReceiptPdfPrinter(IAddressFormatter addressFormatter,
            IPersonalAccountTablePrinter personalAccountTablePrinter
            , IMeterTablesPrinter meterTablesPrinter
            , CommonPresentationSettings commonPresentationSettings
            , IQrBlockPrinter qrBlockPrinter
            , IPaymentInfoPrinter paymentInfoPrinter
            , IReceiptSumAndSignPrinter receiptSumAndSignPrinter
            , IQRAdditionalInfoPrinter paymentBlockPrinter
            , IDetailsPaymentInfoPrinter detailsPaymentInfo
            , IReceiptPersonPaymentsPrinter receiptPersonPaymentsPrinter
            , IBarcodeBlockPrinter barcodeBlockPrinter
            , IUtilitiesPrinter utilitiesPrinter)
        {
            _addressFormatter = addressFormatter;
            _personalAccountTablePrinter = personalAccountTablePrinter;
            _meterTablesPrinter = meterTablesPrinter;
            _commonPresentationSettings = commonPresentationSettings;
            _qrBlockPrinter = qrBlockPrinter;
            _paymentInfoPrinter = paymentInfoPrinter;
            _receiptSumAndSignPrinter = receiptSumAndSignPrinter;
            _paymentBlockPrinter = paymentBlockPrinter;
            _detailsPaymentInfo = detailsPaymentInfo;
            _receiptPersonPaymentsPrinter = receiptPersonPaymentsPrinter;
            _barcodeBlockPrinter = barcodeBlockPrinter;
            _utilitiesPrinter = utilitiesPrinter;
        }

        /// <summary>
        ///     Распечатать квитанцию
        /// </summary>
        /// <param name="receipt">модель квитанции для печати</param>
        /// <param name="cancellationToken"></param>
        /// <returns>файл с pdf документом</returns>
        public async Task<LoadedFile> Print(Models.Receipt receipt, CancellationToken cancellationToken = default)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var result = new LoadedFile
            {
                FileName = Guid.NewGuid() + ".pdf",
                Stream = new MemoryStream()
            };

            FontFactory.RegisterDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Fonts));
            FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, Font.DEFAULTSIZE,
                Font.NORMAL);

            using var pdf = new Document(PageSize.A4, MarginLeft, MarginRight, MarginTop, MarginBottom);
            await using var memoryStream = new MemoryStream();
            using var writer = PdfWriter.GetInstance(pdf, memoryStream);
            writer.CloseStream = false;
            pdf.Open();

            PrintTitle(receipt, pdf);
            _personalAccountTablePrinter.Print(receipt.PersonalAccount, pdf, Element.ALIGN_LEFT);
            _meterTablesPrinter.Print(receipt.Meters, receipt.Payer, pdf);


            var layoutTable = new PdfPTable(new[] { 200f, 385f, 260f })
            {
                WidthPercentage = 100,
                SpacingBefore = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks
            };
            AddFirstBigTableRaw(receipt, layoutTable);
            AddSecondBigTableRaw(receipt, layoutTable);
            pdf.Add(layoutTable);

            AddBarcodeLayout(receipt, writer, pdf);

            AddInfoNotes(receipt, pdf);

            pdf.NewPage();

            PrintSecondPageHeader(receipt, pdf);

            var utilityTable = _utilitiesPrinter.Print(receipt.UtilityGroups, receipt.PaymentPeriod);

            pdf.Add(utilityTable);


            writer.Flush();
            pdf.Close();
            writer.Close();

            memoryStream.Seek(0, SeekOrigin.Begin);
            await memoryStream.CopyToAsync(result.Stream, cancellationToken);
            result.Stream.Seek(0, SeekOrigin.Begin);

            return result;
        }

        private void PrintSecondPageHeader(Models.Receipt receipt, Document pdf)
        {
            var formattedPaymentPeriod =
                $"{receipt.PaymentPeriod.GetMonthString().ToUpperInvariant()} {receipt.PaymentPeriod.Year} г.";

            pdf.Add(new Paragraph(new Phrase(
                $"Счет извещение № {receipt.Number} {formattedPaymentPeriod} по ЛС {receipt.PersonalAccount}"
                , _commonPresentationSettings.SmallBoldFont)));
        }

        private void AddInfoNotes(Models.Receipt receipt, Document pdf)
        {
            foreach (var note in receipt.BottomNotes)
            {
                var paragraph = new Paragraph(new Phrase(note, _commonPresentationSettings.SmallBoldFont))
                {
                    SpacingBefore = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks,
                    SpacingAfter = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks
                };
                pdf.Add(paragraph);
            }
        }

        private void AddBarcodeLayout(Models.Receipt receipt, PdfWriter writer, Document pdf)
        {
            var barcodeLayoutTable = new PdfPTable(new[] {3f, 4f})
            {
                WidthPercentage = 100,
                SpacingBefore = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks
            };

            var barcodeColumn = _barcodeBlockPrinter.Print(receipt.BarcodeForMonth, receipt.BarcodeForTotal, receipt.Payments,
                writer, receipt.PaymentPeriod);
            var barcodeCell = new PdfPCell(barcodeColumn)
            {
                BorderWidth = _commonPresentationSettings.WithoutBorder
            };
            barcodeLayoutTable.AddCell(barcodeCell);

            var infoColumn = AddInfoColumn(receipt.RightNotes);
            var infoCell = new PdfPCell(infoColumn)
            {
                BorderWidth = _commonPresentationSettings.WithoutBorder
            };
            barcodeLayoutTable.AddCell(infoCell);

            pdf.Add(barcodeLayoutTable);
        }

        private PdfPTable AddInfoColumn(string[] infos)
        {
            var infoColumn = new PdfPTable(1);

            foreach (string info in infos)
            {
                var paragraph = new Paragraph(new Phrase(info, _commonPresentationSettings.SmallBoldFont));
                infoColumn.AddCell(
                    new PdfPCell(paragraph)
                    {
                        BorderWidth = _commonPresentationSettings.WithoutBorder
                    });
            }

            return infoColumn;
        }

        private void AddFirstBigTableRaw(Models.Receipt receipt, PdfPTable layoutTable)
        {
            var qrCell = new PdfPCell(_qrBlockPrinter.Print(receipt.QR))
            {
                BorderWidthTop = _commonPresentationSettings.HeavyBorder,
                BorderWidthLeft = _commonPresentationSettings.WithoutBorder,
                BorderWidthBottom = _commonPresentationSettings.WithoutBorder,
                BorderWidthRight = _commonPresentationSettings.WithoutBorder,
                CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER)
            };
            layoutTable.AddCell(qrCell);

            var payTopCell = new PdfPCell(_paymentInfoPrinter.Print(receipt.PaymentReceiver,
                receipt.Address, receipt.Payer, receipt.PersonalAccount))
            {
                BorderWidthTop = _commonPresentationSettings.HeavyBorder,
                BorderWidthLeft = _commonPresentationSettings.ThinBorder,
                BorderWidthBottom = _commonPresentationSettings.WithoutBorder,
                BorderWidthRight = _commonPresentationSettings.WithoutBorder,
                CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER)
            };
            layoutTable.AddCell(payTopCell);

            var paymentSignCell =
                new PdfPCell(_receiptSumAndSignPrinter.Print(receipt.Number, receipt.PaymentPeriod, receipt.Sum))
                {
                    BorderWidthTop = _commonPresentationSettings.HeavyBorder,
                    BorderWidthLeft = _commonPresentationSettings.WithoutBorder,
                    BorderWidthBottom = _commonPresentationSettings.WithoutBorder,
                    BorderWidthRight = _commonPresentationSettings.WithoutBorder,
                    CellEvent = new DottedCell(Rectangle.BOTTOM_BORDER)
                };

            layoutTable.AddCell(paymentSignCell);
        }

        private void AddSecondBigTableRaw(Models.Receipt receipt, PdfPTable layoutTable)
        {
            
            var qrAdditionalInfoCell = new PdfPCell(_paymentBlockPrinter.Print(receipt.QRAdditionalInfo))
            {
                BorderWidthTop = _commonPresentationSettings.WithoutBorder,
                BorderWidthLeft = _commonPresentationSettings.WithoutBorder,
                BorderWidthBottom = _commonPresentationSettings.WithoutBorder,
                BorderWidthRight = _commonPresentationSettings.WithoutBorder
            };
            layoutTable.AddCell(qrAdditionalInfoCell);

            var payTopCell = new PdfPCell(_detailsPaymentInfo.Print(receipt.PaymentReceiver,
                receipt.Address, receipt.Payer, receipt.PersonalAccount, receipt.FlatInfo, receipt.PaidDay))
            {
                BorderWidthTop = _commonPresentationSettings.WithoutBorder,
                BorderWidthLeft = _commonPresentationSettings.ThinBorder,
                BorderWidthBottom = _commonPresentationSettings.WithoutBorder,
                BorderWidthRight = _commonPresentationSettings.WithoutBorder
            };
            layoutTable.AddCell(payTopCell);

            var receiptPersonPaymentsCell =
                new PdfPCell(_receiptPersonPaymentsPrinter.Print(receipt.Number, receipt.PersonalAccount, receipt.PaymentPeriod, receipt.Payments, receipt.PayUpTo))
                {
                    BorderWidthTop = _commonPresentationSettings.WithoutBorder,
                    BorderWidthLeft = _commonPresentationSettings.WithoutBorder,
                    BorderWidthBottom = _commonPresentationSettings.WithoutBorder,
                    BorderWidthRight = _commonPresentationSettings.WithoutBorder
                };

            layoutTable.AddCell(receiptPersonPaymentsCell);
        }

        private void PrintTitle(Models.Receipt receipt, Document pdf)
        {
            var fontTitle = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 10, Font.BOLD);
            var p = new Paragraph(_addressFormatter.FormatAddressForReceipt(receipt.Address), fontTitle) {Alignment = Element.ALIGN_CENTER};
            pdf.Add(p);
        }
    }
}