using System;
using GkhIo.Receipt.Pdf.Abstract;
using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    /// <summary>
    /// Печатаем информацию о получателе платежа
    /// </summary>
    public sealed class DetailsPaymentInfoPrinter : IDetailsPaymentInfoPrinter
    {
        private readonly CommonPresentationSettings _commonPresentationSettings;
        private Font Font => _commonPresentationSettings.SmallFont;
        private PdfPTable _layoutTable;
        private readonly IPersonFullFormFormatter _personFullFormFormatter;
        private readonly IAddressFormatter _addressFormatter;
        private readonly IFlatInfoPrinter _flatInfoPrinter;

        public DetailsPaymentInfoPrinter(CommonPresentationSettings commonPresentationSettings
            , IPersonFullFormFormatter personFullFormFormatter
            , IAddressFormatter addressFormatter
            , IFlatInfoPrinter flatInfoPrinter)
        {
            _commonPresentationSettings = commonPresentationSettings;
            _personFullFormFormatter = personFullFormFormatter;
            _addressFormatter = addressFormatter;
            _flatInfoPrinter = flatInfoPrinter;
        }

        public PdfPTable Print(PaymentReceiver receiver, Address address, Person payer, string personalAccount,
            FlatInfo flatInfo, int payUntil)
        {
            var boldFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.BOLD);
            _layoutTable = new PdfPTable(new[] {2f, 7f});
            AddLabelCell( "Получатель платежа:");
            AddInfoCell(receiver);

            AddLabelCell("Плательщик:");
            _layoutTable.AddCell(
                new PdfPCell(
                    new Phrase(_personFullFormFormatter.ToFullForm(payer).ToUpperInvariant(), boldFont))
                    {
                        BorderWidth = _commonPresentationSettings.WithoutBorder
                    }
                );

            var paragraph = new Paragraph
            {
                new Phrase("Адрес: ", Font),
                new Phrase(_addressFormatter.FormatAddressForReceipt(address), boldFont)
            };
            _layoutTable.AddCell(new PdfPCell(paragraph)
            {
                Colspan = 2,
                BorderWidth = _commonPresentationSettings.WithoutBorder,
                PaddingBottom = 15f
            });

            _layoutTable.AddCell(new PdfPCell(_flatInfoPrinter.Print(flatInfo, payUntil))
            {
                Colspan = 2,
                BorderWidth = _commonPresentationSettings.WithoutBorder,
                PaddingLeft = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks
            });
            
            return _layoutTable;
        }

        private void AddInfoCell(PaymentReceiver receiver)
        {
            var infoCell = new PdfPCell
            {
                PaddingTop = 0,
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = _commonPresentationSettings.WithoutBorder,
                PaddingBottom = 15
            };

            infoCell.AddElement(new Paragraph(new Phrase(receiver.CompanyName + Environment.NewLine +
                                                         "Р/с " + receiver.PaymentAccount + Environment.NewLine +
                                                         "в ПАО " + receiver.BankName + Environment.NewLine +
                                                         "К/с " + receiver.CorrespondenceAccount + " БИК " + receiver.Bic +
                                                         Environment.NewLine +
                                                         "ИНН " + receiver.INN + ", КПП " + receiver.Kpp, Font)));


            _layoutTable.AddCell(infoCell);
        }

        private void AddLabelCell(string label)
        {
            var labelCell = new PdfPCell(new Phrase(label, Font))
            {
                PaddingTop = _commonPresentationSettings.DefaultVerticalSpacingInsideBlocks,
                VerticalAlignment = Element.ALIGN_TOP,
                HorizontalAlignment = Element.ALIGN_LEFT,
                BorderWidth = _commonPresentationSettings.WithoutBorder
            };
            _layoutTable.AddCell(labelCell);
        }
    }
}