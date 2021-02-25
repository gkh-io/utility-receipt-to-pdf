using GkhIo.Receipt.Pdf.Abstract;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    public sealed class QRAdditionalInfoPrinter : IQRAdditionalInfoPrinter
    {
        private readonly CommonPresentationSettings _settings;
        private Font Font => _settings.SmallFont;
        private Font MiddleFont => _settings.MiddleFont;

        public QRAdditionalInfoPrinter(CommonPresentationSettings settings)
        {
            _settings = settings;
        }

        public PdfPTable Print(string paymentAdvice)
        {
            var layoutTable = new PdfPTable(1);

            layoutTable.AddCell(new PdfPCell(new Phrase("КВИТАНЦИЯ", Font))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                BorderWidth = _settings.WithoutBorder,
                PaddingTop = _settings.LabelTopSpacing
            });
            layoutTable.AddCell(new PdfPCell(new Phrase(paymentAdvice, MiddleFont))
            {
                PaddingTop = 10,
                BorderWidth = _settings.WithoutBorder,
                PaddingLeft = 10,
                PaddingRight = 10
            });

            return layoutTable;
        }
    }
}