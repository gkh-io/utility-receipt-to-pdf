using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Abstract
{
    public interface IQRAdditionalInfoPrinter
    {
        PdfPTable Print(string paymentAdvice);
    }
}