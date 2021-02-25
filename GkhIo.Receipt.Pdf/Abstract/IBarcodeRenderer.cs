using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    public interface IBarcodeRenderer
    {
        PdfPCell Render(string barcodeMonth, PdfWriter writer);
    }
}