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
    public sealed class PaymentInfoPrinter : IPaymentInfoPrinter
    {
        private readonly CommonPresentationSettings _commonPresentationSettings;
        private Font _font;
        private PdfPTable _layoutTable;
        private readonly ITabledWordRenderer _tabledWordRenderer;
        private readonly IPersonFullFormFormatter _personFullFormFormatter;
        private readonly IAddressFormatter _addressFormatter;

        public PaymentInfoPrinter(CommonPresentationSettings commonPresentationSettings
            , ITabledWordRenderer tabledWordRenderer
            , IPersonFullFormFormatter personFullFormFormatter
            , IAddressFormatter addressFormatter)
        {
            _commonPresentationSettings = commonPresentationSettings;
            _tabledWordRenderer = tabledWordRenderer;
            _personFullFormFormatter = personFullFormFormatter;
            _addressFormatter = addressFormatter;
        }

        public PdfPTable Print(PaymentReceiver receiver, Address address, Person payer, string personalAccount)
        {
            _font = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.NORMAL);
            var boldFont = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.BOLD);
            _layoutTable = new PdfPTable(new[] {2f, 7f});
            AddLabelCell( "Получатель платежа:");
            AddInfoCell(receiver);

            AddLabelCell( "Лицевой счёт:");
            _layoutTable.AddCell(
                new PdfPCell(
                _tabledWordRenderer.Render(personalAccount))
                {
                    BorderWidth = _commonPresentationSettings.WithoutBorder,
                    // эти отступы нужны для того, чтобы таблица с лицевым счётом не растягивалась на всю строку
                    PaddingTop = 5,
                    PaddingBottom = 5
                });
            AddLabelCell("Ф.И.О.:");
            _layoutTable.AddCell(
                new PdfPCell(
                    new Phrase(_personFullFormFormatter.ToFullForm(payer).ToUpperInvariant(), boldFont))
                    {
                        BorderWidth = _commonPresentationSettings.WithoutBorder
                    }
                );

            var paragraph = new Paragraph
            {
                new Phrase("Адрес: ", _font),
                new Phrase(_addressFormatter.FormatAddressForReceipt(address), boldFont)
            };
            _layoutTable.AddCell(new PdfPCell(paragraph)
            {
                Colspan = 2,
                BorderWidth = _commonPresentationSettings.WithoutBorder
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
                PaddingBottom = 40
            };

            infoCell.AddElement(new Paragraph(new Phrase(receiver.CompanyName + Environment.NewLine +
                                                         "Р/с " + receiver.PaymentAccount + Environment.NewLine +
                                                         "в ПАО " + receiver.BankName + Environment.NewLine +
                                                         "К/с " + receiver.CorrespondenceAccount + " БИК " + receiver.Bic +
                                                         Environment.NewLine +
                                                         "ИНН " + receiver.INN + ", КПП " + receiver.Kpp, _font)));


            _layoutTable.AddCell(infoCell);
        }

        private void AddLabelCell(string label)
        {
            var labelCell = new PdfPCell(new Phrase(label, _font))
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