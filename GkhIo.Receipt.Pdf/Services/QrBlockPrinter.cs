using System.Collections.Generic;
using GkhIo.Receipt.Pdf.Abstract;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.qrcode;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;

namespace GkhIo.Receipt.Pdf.Services
{
    /// <summary>
    /// Печать QR в PDF
    /// </summary>
    public sealed class QrBlockPrinter : IQrBlockPrinter
    {
        /// <summary>
        /// Размер QR
        /// </summary>
        private const float QRImageSize = 130f;
        /// <summary>
        /// Создание QR происходит только кратными размерами, если укажем меньше, то создаст сильно меньшее изображение
        /// (около 80)
        /// </summary>
        private const int CreatedQRImageSize = 160;

        private readonly CommonPresentationSettings _commonPresentationSettings;

        public QrBlockPrinter(CommonPresentationSettings commonPresentationSettings)
        {
            _commonPresentationSettings = commonPresentationSettings;
        }

        public PdfPTable Print(string qr)
        {
            var font = FontFactory.GetFont("Arial", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED, 8, Font.NORMAL);
            var layoutTable = new PdfPTable(1)
            {
                WidthPercentage = 100
            };
            var textCell = CreateLabel(font);
            layoutTable.AddCell(textCell);

            var codeCell = CreateQRCell(qr);
            layoutTable.AddCell(codeCell);
            return layoutTable;
        }

        private PdfPCell CreateQRCell(string qr)
        {
            var image = CreateQRImage(qr);

            var codeCell = new PdfPCell(image)
            {
                BorderWidth = _commonPresentationSettings.WithoutBorder,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };
            return codeCell;
        }

        private static Image CreateQRImage(string qr)
        {
            var paramQR = new Dictionary<EncodeHintType, object>
            {
                {EncodeHintType.CHARACTER_SET, "UTF-8"}
            };
            var barcodeWriter = new BarcodeQRCode(qr, CreatedQRImageSize, CreatedQRImageSize, paramQR);
            var image = barcodeWriter.GetImage();

            image.ScaleToFit(QRImageSize, QRImageSize);
            return image;
        }

        private PdfPCell CreateLabel(Font font)
        {
            var textCell = new PdfPCell(new Phrase("ИЗВЕЩЕНИЕ", font))
            {
                HorizontalAlignment = Element.ALIGN_CENTER,
                BorderWidth = _commonPresentationSettings.WithoutBorder,
                PaddingTop = _commonPresentationSettings.LabelTopSpacing
            };
            return textCell;
        }
    }
}