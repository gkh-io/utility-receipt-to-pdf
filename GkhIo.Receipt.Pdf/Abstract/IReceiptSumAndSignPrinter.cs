using GkhIo.Receipt.Pdf.Models;
using iTextSharp.text.pdf;

namespace GkhIo.Receipt.Pdf.Abstract
{
    public interface IReceiptSumAndSignPrinter
    {
        PdfPTable Print(string receiptNumber, PaymentPeriod paymentPeriod, decimal sum);
    }
}