using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Services
{
    public interface IBarcodeBlockPrinter
    {
        PdfPTable Print(string barcodeMonth, string barcodeTotal, PaymentsInfo payments, PdfWriter writer,
            PaymentPeriod period);
    }
}