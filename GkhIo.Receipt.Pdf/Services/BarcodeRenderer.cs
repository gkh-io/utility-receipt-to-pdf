using iTextSharp.text;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    public sealed class BarcodeRenderer : IBarcodeRenderer
    {
        public PdfPCell Render(string barcodeMonth, PdfWriter writer)
        {
            var barcodeWriter = new Barcode128
            {
                Baseline = 6,
                Code = barcodeMonth,
                CodeType = Barcode.CODE128,
                ChecksumText = true,
                GenerateChecksum = true,
                StartStopText = true
            };

            var code128Image = barcodeWriter.CreateImageWithBarcode(writer.DirectContent, BaseColor.BLACK, BaseColor.BLACK);
            code128Image.ScaleAbsolute(155, 30);
            var cell = new PdfPCell(code128Image)
                {BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE};
            return cell;
        }
    }
}