using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Abstract
{
    /// <summary>
    /// Печать QR в PDF
    /// </summary>
    public interface IQrBlockPrinter
    {
        PdfPTable Print(string qr);
    }
}